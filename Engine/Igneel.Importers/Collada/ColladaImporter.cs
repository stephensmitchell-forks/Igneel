﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using ClrRuntime;
using System.IO;
using Igneel.Assets;
using Igneel.Animations;
using Igneel;
using Igneel.Animations;
using Igneel.Assets;
using Igneel.Components;
using Igneel.Rendering;
using Igneel.Graphics;
using Igneel.Rendering.Bindings;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Importers.Collada
{
    [ImportFormat(".dae")]
    public partial class ColladaImporter : ContentImporter, IAnimationImporter
    {

        Dictionary<string, Func<XElement, object, ElementRef>> _handlerLookup = new Dictionary<string, Func<XElement, object, ElementRef>>();
        Dictionary<string, ElementRef> _references = new Dictionary<string, ElementRef>();
        Dictionary<string, XElement> _skinReferences = new Dictionary<string, XElement>();

        XDocument _document;
        XElement _rootElement;
        bool _zUp;
        XElement _libraryImages, 
                 _libraryMaterials, 
                 _libraryEffects, 
                 _libraryGeometries, 
                 _libraryControllers, 
                 _libraryAnimations,
                 _libraryVisualScenes,
                 _libraryNodes,
                 _libraryLights,
                 _libraryCamera;

        List<BasicMaterial> _materials = new List<BasicMaterial>();
        //List<Frame> _frames = new List<Frame>();
        List<Mesh> _meshes = new List<Mesh>();
        List<Camera> _cameras = new List<Camera>();
        List<Light> _lights = new List<Light>();
        List<MeshSkin> _skins = new List<MeshSkin>();

        List<KeyFrameAnimation> _animations = new List<Animations.KeyFrameAnimation>();
        Dictionary<string, Texture2D> _texturesLookup = new Dictionary<string, Texture2D>();
        Dictionary<Texture2D, string> _textureFilesLookup = new Dictionary<Texture2D, string>();
        Dictionary<string, Frame> _framesLookup = new Dictionary<string, Frame>();
        IProgressReport _report;
        Dictionary<string, FrameAnimationController> _animationControllers = new Dictionary<string, Animations.FrameAnimationController>();
        

        Frame _visualSceneNode;     
        float _unit = 1.0f;
        string _directory;      
        //Dictionary<string, ICurveOutput> _channelsTargesLookup;
        //ColladaExecutionContext _executionContext;        
        Scene _scene;
        private bool _preserveOrder;

        public IProgressReport Report { get { return _report; } set { _report = value; } }

        public ColladaImporter()
        {
            var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            foreach (var m in methods)
            {
                var attr = (ParserOf[])m.GetCustomAttributes(typeof(ParserOf), true);
                if (attr.Length > 0)
                {
                    _handlerLookup.Add(attr[0].Element, (Func<XElement, object, ElementRef>)Delegate.CreateDelegate(typeof(Func<XElement, object, ElementRef>), this, m));
                }
            }          

        }

        public void InitializeImporting(string filename)
        {
            FileName = filename;
            _directory = Path.GetDirectoryName(filename);
            _document = XDocument.Load(filename);
            _rootElement = _document.Root;

            var asset = _rootElement.GetElementByTag("asset");
            if (asset != null)
            {
                var upAxis = asset.GetElementByTag("up_axis");
                if (upAxis != null)
                    _zUp = upAxis.Value == "Z_UP";
                var xunit = asset.GetElementByTag("unit");
                if (xunit != null)
                    _unit = float.Parse(xunit.GetAttribute("meter"));
                _preserveOrder = asset.GetElementByTag("contributor")?
                                   .GetElementByTag("authoring_tool")?
                                   .Value?.StartsWith("CINEMA4D") ?? false;              
            }
            _unit *= _scene.UnitOfDistance.Meters;

            _libraryImages = _rootElement.GetElementByTag("library_images");
            _libraryMaterials = _rootElement.GetElementByTag("library_materials");
            _libraryEffects = _rootElement.GetElementByTag("library_effects");
            _libraryGeometries = _rootElement.GetElementByTag("library_geometries");
            _libraryControllers = _rootElement.GetElementByTag("library_controllers");
            _libraryAnimations = _rootElement.GetElementByTag("library_animations");
            _libraryVisualScenes = _rootElement.GetElementByTag("library_visual_scenes");
            _libraryNodes = _rootElement.GetElementByTag("library_nodes");
            _libraryLights = _rootElement.GetElementByTag("library_lights");
            _libraryCamera = _rootElement.GetElementByTag("library_cameras");
        }

        protected override void ImportFile(Scene scene, string filename)
        {
            this._scene = scene;
            InitializeImporting(filename);

            ResolveElementByUrl(_libraryVisualScenes,
                                _rootElement.GetElementByTag("scene")
                                .GetDescendantByTag("instance_visual_scene")
                                .GetAttribute("url"));


            if (_libraryAnimations != null)
            {
                Parse_animations_lib(_libraryAnimations, null);
            }

            scene.Materials.AddRange(_materials);
            scene.Meshes.AddRange(_meshes);
        }

        public void ImportAnimation(Scene scene, string filename)
        {
            this._scene = scene;
            InitializeImporting(filename);           

            if (_libraryAnimations != null)
            {
                var animationResult = Parse_animations_lib(_libraryAnimations, null);             
            }           
        }

        public void ImportAnimation(Scene scene, string filename, Frame root, string fileRootName = null)
        {          
            ImportFile(scene, filename);
            Dictionary<string, Frame> lookup = new Dictionary<string, Frame>();

            //if (MapTreeStructure(fileRootName == null ? _visualSceneNode : _visualSceneNode.FindNode(fileRootName), root, lookup))
            //{
            //    foreach (var layer in _animations)
            //    {
            //        foreach (var node in layer.Nodes)
            //        {
            //            var context = node.Context;
            //            context.Target = lookup[((ITargetNamedContext)context).TargetName];
            //        }
            //    }
            //}            
        }

        public bool MapTreeStructure(Frame node, Frame srcNode, Dictionary<string, Frame> lookup)
        {
            if (node.Childrens.Count != srcNode.Childrens.Count)
                return false;

            lookup.Add(node.Name, srcNode);
            for (int i = 0; i < node.Childrens.Count; i++)
            {
                if (!MapTreeStructure(node.Childrens[i], srcNode.Childrens[i], lookup))
                    return false;
            }

            return true;
        }

        private string GetName(XElement node)
        {
            string name = node.GetName()??node.GetId();            
            int index = name.IndexOf("__");
            int lastIndex = name.LastIndexOf("__");
            if (index > 0)
            {                
                name = name.Substring(0, index) + (lastIndex + 2 < name.Length ? name.Substring(lastIndex + 2, name.Length - 2 - lastIndex) : "");
            }
            return name;
        }

        private Texture2D GetTexture(string filename)
        {
            Texture2D tex;
            if (!_texturesLookup.TryGetValue(filename, out tex))
            {
                tex = GraphicDeviceFactory.Device.CreateTexture2DFromFile(filename);
                _texturesLookup.Add(filename, tex);
                _textureFilesLookup.Add(tex, filename);
            }
            return tex;
        }

        private string GetFilename(Texture2D tex)
        {
            return _textureFilesLookup[tex];
           
        }

        private FrameAnimationController GetAnimationController(string targetName)
        {
            FrameAnimationController controller;
            if (!_animationControllers.TryGetValue(targetName, out controller))
            {
                Frame targetFrame = _visualSceneNode != null ? _visualSceneNode.FindNode(targetName) :
                                _scene.FindNode(targetName);
                if (targetFrame != null)
                {
                    controller = new Igneel.Animations.FrameAnimationController();
                    targetFrame.AnimationController = controller;
                }
                _animationControllers.Add(targetName, controller);
            }
            return controller;
        }

        #region Parser Methods

        #region Material

        [ParserOf("material")]
        private ElementRef Parse_material(XElement element, object @param)
        {
            BasicMaterial material = new BasicMaterial(Path.GetFileNameWithoutExtension(FileName) + "_" + element.GetName());
            _materials.Add(material);           

            ElementRef refe = new ElementRef { Element = element, Object = material };           

            var effect = _libraryEffects.GetElementById(element.GetElementByTag("instance_effect").GetAttribute("url").Substring(1));           
            var technique = effect.GetDescendantByTag("technique");
            var profile = technique.Elements().First();
            var shading = profile.Name.LocalName;
            var transparency = profile.GetElementByTag("transparent");
            bool invert = shading == "phong";

            if (transparency != null)
            {
                var colorElement = transparency.GetDescendantByTag("color");
                if (colorElement != null)
                {
                    var color = ParseVector4(colorElement.Value, false);
                    invert = color.X == 1;
                }
            }
            ReadColorInfo(effect, "emission", (color, textureUrl) =>
            {
                material.EmisiveIntensity = (float)color;                
            });

            ReadColorInfo(effect, "specular", (color, textureUrl) =>
            {
                material.SpecularIntensity = (float)color;
                if (textureUrl != null)
                {
                    try
                    {
                        material.SpecularMap = GetTexture(textureUrl);
                    }
                    catch (InvalidOperationException)
                    {
                        material.SpecularMap = null;
                    }
                }
            });

            ReadColorInfo(effect, "diffuse", (color, textureUrl) =>
            {
                material.Diffuse = new Color3(color.X, color.Y, color.Z);
                if (textureUrl != null)
                {
                    try
                    {
                        material.DiffuseMap = GetTexture(textureUrl);
                        SetMaps(material);
                    }
                    catch (InvalidOperationException)
                    {
                        material.DiffuseMap = null;
                    }
                }
            });

            ReadParamenterSurfaceInfo(effect, "normal_map-surface", url => material.NormalMap = GraphicDeviceFactory.Device.CreateTexture2DFromFile(url));            

            ReadFloatInfo(technique, "shininess", v => material.SpecularPower = v);
            ReadFloatInfo(technique, "reflectivity", v => material.Reflectivity = v);
            ReadFloatInfo(technique, "transparency", v => material.Alpha = invert ? 1 - v : v);

            return refe;
        }

        [ParserOf("newparam")]
        private ElementRef Parse_NewParam(XElement element, object @param)
        {
            EffectParameter parameter = new EffectParameter();
            var xsemantic = element.GetElementByTag("semantic");
            var modifier = element.GetElementByTag("modifier");
            if (xsemantic != null)
                parameter.Semantic = xsemantic.Value;
            if (modifier != null)
                parameter.Modifier = modifier.Value;
            var tags = new string[] { "sampler2D", "surface" };

            foreach (var tag in tags)
            {
                var node = element.GetElementByTag(tag);
                if (node != null)
                {
                    parameter.Value = ResolveElement<object>(node, param);
                    break;
                }
            }

            return new ElementRef { Element = element, Object = parameter };
        }

        [ParserOf("surface")]
        private ElementRef Parse_Surface(XElement element, object @param)
        {
            var type = element.GetAttribute("type");            
            string url = GetImageFilenameFromUrl(element.GetElementByTag("init_from").Value);
            ElementRef refe = new ElementRef { Element = element, Object = url != null ? new Uri(url) : null };
            return refe;
        }

        [ParserOf("sampler2D")]
        private ElementRef Parse_sampler2D(XElement element, object @param)
        {
            var xSource = element.GetElementByTag("source");
            var sourceValue = InvokeParser(_libraryEffects, "sid", xSource.Value, param);
            var parameter = (EffectParameter)sourceValue.Object;            
            Sampler2D sampler = new Sampler2D { Texture = (Uri)parameter.Value };
            return new ElementRef { Element = element, Object = sampler };
        }
       
        #endregion

        [ParserOf("visual_scene")]
        private ElementRef Parse_visual_scene(XElement element, object @params)
        {
            _visualSceneNode = new Frame(Path.GetFileNameWithoutExtension(FileName) + "_" + element.GetName())
            {
                Type = FrameType.Root
            };
            if (_report != null)
                _report.OnObjectCreated(_visualSceneNode);

            ElementRef refe = new ElementRef { Element = element, Object = _visualSceneNode };

            foreach (var item in element.GetElementsByTag("node"))
            {
                var childNode = ResolveElement(item);
                if (childNode.Object is Frame)
                {
                    _visualSceneNode.Childrens.Add((Frame)childNode.Object);
                }
            }

            //updates global transform
            _visualSceneNode.CommitChanges();

            _scene.Nodes.Add(_visualSceneNode);
            _visualSceneNode.OnSceneAttach(_scene);

            return refe;
        }

        [ParserOf("node")]
        private ElementRef Parse_node(XElement element, object @param)
        {
            var typeAttr = element.GetAttribute("type");                                       

            string id = element.GetId();
            string name = element.GetName()??id;
            string tag = null;
            int index = name.IndexOf("__");
            int lastIndex = name.LastIndexOf("__");
            if (index > 0)
            {
                tag = name.Substring(index, lastIndex - index + 2);
                name = name.Substring(0, index) + (lastIndex + 2 < name.Length ? name.Substring(lastIndex + 2, name.Length - 2 - lastIndex) : "");
            }

            Frame node = new Frame(name) { Tag = tag };
            if (_report != null)
                _report.OnObjectCreated(node);

            List<IGraphicsProvider> components = new List<IGraphicsProvider>();                     
            Matrix poseTransform = Matrix.Identity;
             int i = 0;

            foreach (var item in element.Elements())
            {
                #region Parse Transforms

                if (item.Name.LocalName == "translate")
                {
                    poseTransform = Matrix.Translate(ParseVector3(item.Value)) * poseTransform;
                }
                else if (item.Name.LocalName == "rotate")
                {
                    var rotvalue = ParseVector4(item.Value);
                    poseTransform = Matrix.RotationAxis(rotvalue.ToVector3(), -Numerics.ToRadians(rotvalue.W)) * poseTransform;
                }
                else if (item.Name.LocalName == "scale")
                {
                    poseTransform = Matrix.Scale(ParseVector3(item.Value)) * poseTransform;
                }
                else if (item.Name.LocalName == "matrix")
                {
                    var mat = ParseMatrix(item.Value);
                    poseTransform = mat * poseTransform;
                }
                #endregion

                var result = ResolveElement(item, element);
                if (result != null)
                {
                    if (result.Object is IGraphicsProvider)
                    {
                        if (result.Object is FrameSkin)
                        {
                            ((FrameSkin)result.Object).Skin.BindShapePose = poseTransform * ((FrameSkin)result.Object).Skin.BindShapePose;
                            //poseTransform = Matrix.Identity;
                        }
                        components.Add((IGraphicsProvider)result.Object);
                    }
                    else if (result.Object is Frame)
                    {
                        Frame child = (Frame)result.Object;
                        if (child.Name == name)
                            child.Name += i++;
                        node.Childrens.Add(child);
                    }
                } 
            }

            if (typeAttr != null && typeAttr.ToUpper() == "JOINT")
                node.Type = FrameType.Bone;

            node.LocalPose = poseTransform;
            var pivot = node.Childrens.FirstOrDefault(x => x.Name == name + "-Pivot");
            if (pivot != null && pivot.Component == null && pivot.Childrens.Count == 0)
            {               
                pivot.Remove();
            }

            if (components.Count > 0)
            {
                if (components.Count == 1)
                    node.Component = components[0];
                else
                {
                    var collection = new FrameComponentColletion();
                    foreach (var comp in components)
                        collection.Add(comp);

                    collection.Initialize();
                    node.Component = collection;
                }
            }

            if (node.Component is IGraphicObject)
            {
                _scene.Geometries.Add(node);
            }
           
            return new ElementRef { Element = element, Object = node };
        }
      

        private Dictionary<string, BasicMaterial> Parse_BindingMaterial(XElement element, object @param)
        {
            Dictionary<string, BasicMaterial> materials = new Dictionary<string, BasicMaterial>();

            foreach (var item in element.GetDescendantsByTag("instance_material"))
            {
                var key = item.GetAttribute("symbol");
                if (!materials.ContainsKey(key))
                {
                    var result = ResolveElementByUrl(_libraryMaterials, item.GetAttribute("target"));
                    BasicMaterial material;
                    if (result != null)
                        material = (BasicMaterial)result.Object;
                    else
                    {
                        material = BasicMaterial.CreateDefaultMaterial(Path.GetFileNameWithoutExtension(FileName) + "_" + item.GetAttribute("symbol"));
                        this._materials.Add(material);
                    }

                    materials.Add(key, material);
                }
            }

            return materials;
        }

        [ParserOf("instance_geometry")]
        private ElementRef Parse_StaticMeshNode(XElement element, object @param)
        {
            var bindMaterial = element.GetElementByTag("bind_material");
            Dictionary<string,BasicMaterial>materialsLookup = null;
            if (bindMaterial != null)
                materialsLookup = Parse_BindingMaterial(element.GetElementByTag("bind_material"), element);

            var geometryResult = ResolveElementByUrl(_libraryGeometries, 
                                                    element.GetAttribute("url"),
                                                    new CreateMeshInput());
            if (geometryResult == null)
                return null;

            var mesh = ((CreateMeshResult)geometryResult.Object).Mesh;
            var materialSlost = mesh.MaterialSlotNames;            
            if (mesh != null)
            {
                BasicMaterial[] materials;
                if (materialsLookup != null)
                    materials = materialSlost.Select(x => materialsLookup[x]).ToArray();
                else
                {
                    materials = new BasicMaterial[]
                    { 
                        BasicMaterial.CreateDefaultMaterial(Path.GetFileNameWithoutExtension(FileName) + "_" + element.GetAttribute("url").Substring(1))
                    };
                    this._materials.Add(materials[0]);
                }               

                FrameMesh meshInstance = new FrameMesh(materials, mesh);                
                return new ElementRef { Element = element, Object = meshInstance };
            }
            return null;
        }

        [ParserOf("instance_controller")]
        private ElementRef Parse_SkeletalMeshNode(XElement element, object @param)
        {
            var bindMaterial = element.GetElementByTag("bind_material");
            Dictionary<string, BasicMaterial> materialsLookup = null;

            if (bindMaterial != null)
                materialsLookup = Parse_BindingMaterial(bindMaterial, element);

            var controllerRef = ResolveElementByUrl(_libraryControllers, element.GetAttribute("url"), materialsLookup);

            MeshSkin skin = (MeshSkin)controllerRef.Object;
            if (skin == null)
                return null;

            BasicMaterial[] materials;
            if (materialsLookup != null)
                materials = skin.Mesh.MaterialSlotNames.Select(x => materialsLookup[x]).ToArray();
            else
            {
                materials = new BasicMaterial[]
                    { 
                        BasicMaterial.CreateDefaultMaterial(Path.GetFileNameWithoutExtension(FileName) + "_" + element.GetAttribute("url").Substring(1))
                    };
                _materials.AddRange(materials);
            }

            FrameSkin skinInstance = new FrameSkin(materials, skin);
            return new ElementRef { Element = element, Object = skinInstance };
        }

        [ParserOf("instance_node")]
        private ElementRef Parse_InstanceNode(XElement element, object @param)
        {
            return ResolveElementByUrl(_libraryNodes, element.GetAttribute("url"), param);
        }

        [ParserOf("instance_light")]
        private ElementRef Parse_LightNode(XElement element, object @param)
        {
            var light = (Light)ResolveElementByUrl(_libraryLights, element.GetAttribute("url"), param).Object;
            FrameLight instance = new FrameLight(light) { LocalDirection = new Vector3(0, -1, 0) };
            return new ElementRef { Element = element, Object = instance };
        }

        [ParserOf("instance_camera")]
        private ElementRef Parse_CameraNode(XElement element, object @param)
        {
            Camera camera = (Camera)ResolveElementByUrl(_libraryCamera, element.GetAttribute("url"), param).Object;            
            return new ElementRef { Element = element, Object = camera };
        }

        [ParserOf("geometry")]
        private ElementRef Parse_geometry(XElement element, object @param)
        {
            ElementRef refe = new ElementRef { Element = element };
            foreach (var item in element.Elements())
            {
                var result = ResolveElement(item, param);
                if (result == null)
                    return null;
                refe.Object = result.Object;
                if (refe.Object != null)
                {
                    Mesh mesh = refe.Object as Mesh;
                    if (mesh != null)
                    {
                        mesh.Name = element.GetName();
                    }
                    return refe;                    
                }

            }
            return null;
        }

        [ParserOf("mesh")]
        private unsafe ElementRef Parse_mesh(XElement element, object @param)
        {
            CreateMeshInput cinput = (CreateMeshInput)param;
            Mesh mesh = cinput.CreateMesh();
        
            
            var vd = mesh.VertexDescriptor;

            List<string> materialSlots = new List<string>();
            List<MeshPart> layers = new List<MeshPart>();
            var vertices = ResolveElement(element.GetElementByTag("vertices"));
            var vertexDefinition = (InputCollection)vertices.Object;
            var positions = vertexDefinition["POSITION"];

            VertexBufferBuilder vertexBuilder = new VertexBufferBuilder(positions.Accesor.Count, vd);
            List<int> indicesList = new List<int>();
            Input[] inputs = null;
            Input positionInput = new Input();
            Input normalInput = new Input();
            Input texCoordInput = new Input();
            byte[] vertexData = new byte[vd.Size];                   

            int posOffset = vd.OffsetOf(IASemantic.Position, 0);
            int normalOffset = vd.OffsetOf(IASemantic.Normal, 0);
            int texOffset = vd.OffsetOf(IASemantic.TextureCoordinate, 0);
            int vertexAttrStride = 0;

            string[] tags = { "polygons", "triangles", "polylist", "trifans", "tristrips" };

            fixed (byte* pVertexData = vertexData)
            {
                for (int itag = 0; itag < tags.Length; itag++)
                {
                    foreach (var polygonos in element.GetElementsByTag(tags[itag]))
                    {
                        int count = int.Parse(polygonos.GetAttribute("count"));
                        if (count == 0)
                            continue;

                        int startVertex = int.MaxValue;
                        int vertexCount = 0;
                        int startIndex = indicesList.Count;

                        #region Pick Material Slots

                        int materialIndex = 0;
                        string materialSlotName = polygonos.GetAttribute("material");
                        if (materialSlotName != null)
                        {
                            materialIndex = materialSlots.IndexOf(materialSlotName);
                            if (materialIndex < 0)
                            {
                                materialIndex = materialSlots.Count;
                                materialSlots.Add(materialSlotName);
                            }
                        }

                        #endregion

                        #region Pick Inputs

                        if (inputs == null)
                        {
                            var inputList = new List<Input>();
                            foreach (var input in polygonos.GetElementsByTag("input"))
                            {
                                var source = ResolveElementByUrl(element, input.GetAttribute("source"));
                                var _input = new Input
                                {
                                    Semantic = input.GetAttribute("semantic"),
                                    Offset = int.Parse(input.GetAttribute("offset")),
                                    UsageIndex = int.Parse(input.GetAttribute("set") ?? "0"),
                                    Definition = source.Object as InputCollection,
                                    Source = source.Object as Source
                                };
                                inputList.Add(_input);
                                if (_input.Semantic == "VERTEX")
                                {
                                    positionInput = _input.Definition.GetInput("POSITION", _input.Offset);
                                    normalInput = _input.Definition.GetInput("NORMAL", _input.Offset);
                                    texCoordInput = _input.Definition.GetInput("TEXCOORD", _input.Offset);

                                }
                                else if (_input.Semantic == "NORMAL")
                                    normalInput = _input;
                                else if (_input.Semantic == "TEXCOORD" && _input.UsageIndex == 0)
                                    texCoordInput = _input;
                            }

                            positionInput.Source.Lock();
                            if (normalInput.Source != null)
                                normalInput.Source.Lock();
                            if (texCoordInput.Source != null)
                                texCoordInput.Source.Lock();

                            inputList.Sort((x, y) => x.Offset.CompareTo(y.Offset));
                            inputs = inputList.ToArray();
                            vertexAttrStride = inputList.GroupBy(x => x.Offset).Count();
                        }

                        #endregion

                        #region Create Layers 
                        List<int> tesselateList = new List<int>();

                        var vcount = itag == 2 ? polygonos.GetElementByTag("vcount") : null;
                        int[] vcountList = vcount != null ? ParseIntArray(vcount.Value) : null;                       

                        foreach (var p in polygonos.GetElementsByTag("p"))
                        {
                            int vcountIndex = 0;
                            int vcountTaken = 0;

                            var polyIndices = ParseIntArray(p.Value);
                            int polyVerticesCount = polyIndices.Length / vertexAttrStride;                         

                            tesselateList.Clear();

                            for (int tVertex = 0; tVertex < polyVerticesCount; tVertex++)
                            {
                                var iStart = tVertex * vertexAttrStride;
                                var posIdx = polyIndices[iStart + positionInput.Offset];
                                var normalIdx = (normalInput.Source != null) ? polyIndices[iStart + normalInput.Offset] : -1;
                                var texCIdx = (texCoordInput.Source != null) ? polyIndices[iStart + texCoordInput.Offset] : -1;

                                var index = vertexBuilder.GetVertexIndex(posIdx, normalIdx, texCIdx);

                                if (index < 0)
                                {
                                    #region Copy Vertex Attributes

                                    Runtime.Copy((byte*)positionInput.Source.ArrayPointer + posIdx * 12, pVertexData + posOffset, 12);
                                    if (normalIdx >= 0)
                                        Runtime.Copy((byte*)normalInput.Source.ArrayPointer + normalIdx * 12, pVertexData + normalOffset, 12);
                                    if (texCIdx >= 0)
                                        Runtime.Copy((byte*)texCoordInput.Source.ArrayPointer + texCIdx * 8, pVertexData + texOffset, 8);

                                    index = vertexBuilder.WriteVertex(vertexData, posIdx, normalIdx, texCIdx);
                                    vertexCount++;

                                    #endregion
                                }

                                startVertex = Math.Min(startVertex, index);

                                tesselateList.Add(index);

                                vcountTaken++;
                                if (vcountList != null)
                                {
                                    if (vcountList[vcountIndex] == vcountTaken)
                                    {                                                                                
                                        Tesselate(itag, indicesList, tesselateList);

                                        vcountTaken = 0;
                                        vcountIndex++;
                                        tesselateList.Clear();
                                    }

                                }

                            }

                            if(tesselateList.Count > 0)
                                Tesselate(itag, indicesList, tesselateList);

                            //int polygon = 0;
                            //int lastVertexIndex = 0;
                            //int index;

                            //if (itag == 0)
                            //{
                            //    #region Pick Last Vertex Index

                            //    iStart = (polyVerticesCount - 1) * vertexAttrStride;
                            //    posIdx = polyIndices[iStart + positionInput.Offset];
                            //    normalIdx = (normalInput.Source != null) ? polyIndices[iStart + normalInput.Offset] : -1;
                            //    texCIdx = (texCoordInput.Source != null) ? polyIndices[iStart + texCoordInput.Offset] : -1;

                            //    lastVertexIndex = vertexBuilder.GetVertexIndex(posIdx, normalIdx, texCIdx);
                            //    if (lastVertexIndex < 0)
                            //    {
                            //        #region Copy Vertex Attributes

                            //        Runtime.Copy((byte*)positionInput.Source.ArrayPointer + posIdx * 12, pVertexData + posOffset, 12);
                            //        if (normalIdx >= 0)
                            //            Runtime.Copy((byte*)normalInput.Source.ArrayPointer + normalIdx * 12, pVertexData + normalOffset, 12);
                            //        if (texCIdx >= 0)
                            //            Runtime.Copy((byte*)texCoordInput.Source.ArrayPointer + texCIdx * 8, pVertexData + texOffset, 8);

                            //        #endregion

                            //        lastVertexIndex = vertexBuilder.WriteVertex(vertexData, posIdx, normalIdx, texCIdx);
                            //        vertexCount++;
                            //    }

                            //    startVertex = Math.Min(startVertex, lastVertexIndex);

                            //    #endregion

                            //    polyVerticesCount = polyVerticesCount - 1;
                            //}

                            //for (int tVertex = 0; tVertex < polyVerticesCount; tVertex++)
                            //{
                            //    if (itag == 0 && polygon == 2)
                            //    {
                            //        indicesList.Add(lastVertexIndex);
                            //        indicesList.Add(indicesList[indicesList.Count - 2]);
                            //        polygon = 1;
                            //    }

                            //    iStart = tVertex * vertexAttrStride;
                            //    posIdx = polyIndices[iStart + positionInput.Offset];
                            //    normalIdx = (normalInput.Source != null) ? polyIndices[iStart + normalInput.Offset] : -1;
                            //    texCIdx = (texCoordInput.Source != null) ? polyIndices[iStart + texCoordInput.Offset] : -1;

                            //    index = vertexBuilder.GetVertexIndex(posIdx, normalIdx, texCIdx);

                            //    if (index < 0)
                            //    {
                            //        #region Copy Vertex Attributes

                            //        Runtime.Copy((byte*)positionInput.Source.ArrayPointer + posIdx * 12, pVertexData + posOffset, 12);
                            //        if (normalIdx >= 0)
                            //            Runtime.Copy((byte*)normalInput.Source.ArrayPointer + normalIdx * 12, pVertexData + normalOffset, 12);
                            //        if (texCIdx >= 0)
                            //            Runtime.Copy((byte*)texCoordInput.Source.ArrayPointer + texCIdx * 8, pVertexData + texOffset, 8);

                            //        index = vertexBuilder.WriteVertex(vertexData, posIdx, normalIdx, texCIdx);
                            //        vertexCount++;

                            //        #endregion
                            //    }

                            //    startVertex = Math.Min(startVertex, index);
                            //    indicesList.Add(index);
                            //    polygon++;
                            //}

                            //if (itag == 0)
                            //    indicesList.Add(lastVertexIndex);
                         
                        }

                        #endregion

                        layers.Add(new MeshPart(startIndex, (indicesList.Count - startIndex) / 3, startVertex, vertexCount) { MaterialIndex = materialIndex });

                    }
                }                
            }
            if (vertexBuilder.Count == 0)
            {
                mesh.Dispose();
                return null;
            }
                                   
            mesh.Layers = layers.ToArray();
            mesh.MaterialSlotNames = materialSlots.ToArray();

            #region Create VertexBuffer

            byte[] buffer = vertexBuilder.GetBuffer();
            var profile = GraphicDeviceFactory.Device.Profile;

            fixed (byte* pBuffer = buffer)
            {
                var size = vd.Size;

                int[] offsets = new int[] { posOffset, normalOffset };
                Vector3* pv;

                for (int i = 0; i < vertexBuilder.Count; i++)
                {
                    //scale position to match the scene Unit Of Distance
                    //Vector3* pv = (Vector3*)(pBuffer + i * size + offsets[0]);
                    //*pv = (*pv) * _unit;

                    if (profile == DeviceProfile.Direct3D)
                    {
                        //invert texture v coord
                        Vector2* texCoord = (Vector2*)(pBuffer + i * size + texOffset);
                        texCoord->Y = 1 - texCoord->Y;
                    }

                    if (_zUp)
                    {
                        //invert z by y
                        for (int j = 0; j < 2; j++)
                        {
                            pv = (Vector3*)(pBuffer + i * size + offsets[j]);
                            Vector3 temp = *pv;
                            pv->Y = temp.Z;
                            pv->Z = temp.Y;
                        }
                    }

                }
            }            

            mesh.CreateVertexBuffer(buffer, vertexBuilder.Count);
            #endregion

            #region Create IndexBuffer

            if (mesh.VertexCount > ushort.MaxValue)
            {
                int[] indicesData = new int[indicesList.Count];              
                if (!GraphicDeviceFactory.Device.Rasterizer?.State.FrontCounterClockwise??true && !_preserveOrder)
                {
                    for (int i = 0; i < indicesData.Length - 2; i += 3)
                    {
                        indicesData[i] = indicesList[i + 2];
                        indicesData[i + 1] = indicesList[i + 1];
                        indicesData[i + 2] = indicesList[i];
                    }
                }
                else
                {
                    for (int i = 0; i < indicesData.Length; i++)                    
                        indicesData[i] = indicesList[i];                    
                }

                mesh.CreateIndexBuffer(indicesData);
            }
            else
            {
                ushort[] indicesData = new ushort[indicesList.Count];
                if (GraphicDeviceFactory.Device.Rasterizer != null && !GraphicDeviceFactory.Device.Rasterizer.State.FrontCounterClockwise && !_preserveOrder)
                {
                    for (int i = 0; i < indicesData.Length - 2; i += 3)
                    {
                        indicesData[i] = (ushort)indicesList[i + 2];
                        indicesData[i + 1] = (ushort)indicesList[i + 1];
                        indicesData[i + 2] = (ushort)indicesList[i];
                    }
                }
                else
                {
                    for (int i = 0; i < indicesData.Length; i++)
                        indicesData[i] = (ushort)indicesList[i];
                }
                mesh.CreateIndexBuffer(indicesData);
            }

            #endregion

            positionInput.Source.Unlock();
            mesh.ComputeBoundingVolumenes();

            if (normalInput.Source != null)            
                normalInput.Source.Unlock();                            
            else
                mesh.ComputeNormals();

            if (texCoordInput.Source != null)
                texCoordInput.Source.Unlock();
            else
                mesh.ComputeTextureCoords(CoordMappingType.Cylindrical);

            mesh.ComputeTangents();

            //mesh.Name = Path.GetFileNameWithoutExtension(FileName) + "_" + element.Parent.GetId();
            string name = element.Parent.GetName() ?? element.Parent.GetId();
            int tagindex = name.IndexOf("__");
            if (tagindex > 0)
            {
                name = name.Substring(0, tagindex);
            }
            mesh.Name = name;

            if (_report != null)
                _report.OnObjectCreated(mesh);
            
            _meshes.Add(mesh);

            return new ElementRef
            {
                Element = element,
                Object = new CreateMeshResult { Mesh = mesh, VertexBuilder = vertexBuilder }
            };
        }

        private void Tesselate(int itag, List<int> indicesList, List<int> tesselateList)
        {
            //tesselate primitives
            switch (itag)
            {
                case 0:
                case 2:
                    //polygons   
                    TesselatePolygons(indicesList, tesselateList);
                    break;
                case 1:
                    //triangles
                    indicesList.AddRange(tesselateList);
                    break;                
                case 3:
                    //trifans
                    break;
                case 4:
                    //tristrips
                    TesselateTriangleStrip(indicesList, tesselateList);
                    break;
            }
        }

        private void TesselatePolygons(List<int> triangles, List<int> tesselateList)
        {
            //if (tesselateList.Count % 2 == 0)
            //{
            //    for (int i = 0; i < tesselateList.Count / 2; i++)
            //    {
            //        triangles.Add(tesselateList[2 * i]);
            //        triangles.Add(tesselateList[2 * i + 1]);
            //        triangles.Add(tesselateList[2 * i + 2]);
            //    }

            //}

            triangles.Add(tesselateList[0]);
            triangles.Add(tesselateList[1]);
            triangles.Add(tesselateList[2]);

            for (int i = 3; i < tesselateList.Count; i++)
            {
                triangles.Add(tesselateList[0]);
                triangles.Add(tesselateList[i-1]);
                triangles.Add(tesselateList[i]);
            }          
        }

        private List<int> TesselatePolylist(List<int> tesselateList, int[]vcounts)
        {
            List<int> triangles = new List<int>();

            int index = 0;
            for (int ipoly = 0; ipoly < vcounts.Length; ipoly++)
            {
                var vcount = vcounts[ipoly];

                triangles.Add(tesselateList[index + 0]);
                triangles.Add(tesselateList[index + 1]);
                triangles.Add(tesselateList[index + 2]);

                for (int i = 3; i < vcount; i++)
                {
                    triangles.Add(tesselateList[index + 0]);
                    triangles.Add(tesselateList[index + i - 1]);
                    triangles.Add(tesselateList[index + i]);
                }

                index += vcount;

            }           

            return triangles;
        }

        private void TesselateTriangleStrip(List<int> triangles, List<int> tesselateList)
        {
            triangles.Add(tesselateList[0]);
            triangles.Add(tesselateList[1]);
            triangles.Add(tesselateList[2]);

            for (int i = 3; i < tesselateList.Count - 1; i++)
            {
                if (i % 2 == 0)
                {
                    triangles.Add(tesselateList[i - 2]);
                    triangles.Add(tesselateList[i]);
                    triangles.Add(tesselateList[i + 1]);
                }
                else
                {
                    triangles.Add(tesselateList[i - 1]);
                    triangles.Add(tesselateList[i]);
                    triangles.Add(tesselateList[i + 1]);
                }
            }
        }

        [ParserOf("source")]
        private ElementRef Parse_source(XElement element, object @params)
        {
            Source source = new Source { Id = element.GetId() };
            var array = element.GetElementByTag("float_array");
            
            if(array!=null)
            {
                source.Type = ColladaType.Float;
                source.ColladaTypeString = "float";
                source.Array = ParseFloatArray(array.Value);
            }
            array = element.GetElementByTag("Name_array");
            if (array != null)
            {
                source.Type = ColladaType.Name;
                source.ColladaTypeString = "name";
                source.Array = ParseNameArray(array.Value);
            }
            array = element.GetElementByTag("bool_array");
            if (array != null)
            {
                source.Type = ColladaType.Bool;
                source.Array =ParseBoolArray(array.Value);
                source.ColladaTypeString = "bool";
            }
            array = element.GetElementByTag("int_array");
            if (array != null)
            {
                source.Type = ColladaType.Int;
                source.ColladaTypeString = "int";
                source.Array = ParseIntArray(array.Value);
            }
            array = element.GetElementByTag("IDREF_array");
            if (array != null)
            {
                source.Type = ColladaType.IdRef;
                source.ColladaTypeString = "IDRef";
                source.Array = ParseNameArray(array.Value);
            }

            var accesorElement = element.GetDescendantByTag("accessor");
            if (accesorElement != null)
            {
                var accesor = new Accesor();
                source.Accesor = accesor;                
                accesor.Count = int.Parse(accesorElement.GetAttribute("count") ?? "0");
                accesor.Stride = int.Parse(accesorElement.GetAttribute("stride") ?? "1");
                int index = 0;
                foreach (var item in accesorElement.GetElementsByTag("param"))
                {
                    string name = item.GetAttribute("name") ?? "param" + index++;
                    string type = item.GetAttribute("type");
                    accesor.Parameters.Add(name, 
                            new AccesorParameter
                            {
                                Name = name,
                                Semantic = item.GetAttribute("semantic"),
                                Type = ParsePrimitiveType(type),
                                ColladaTypeString = type
                            });
                }
            }           

            return new ElementRef { Element = element, Object = source };
        }

        [ParserOf("vertices")]      
        private ElementRef Parse_InputCollection(XElement element, object @params)
        {
            InputCollection vd = new InputCollection();
            foreach (var item in element.GetElementsByTag("input"))
            {
                var semantic = item.GetAttribute("semantic");
                var sourceRef = ResolveElementByUrl(element.Parent, item.GetAttribute("source"));
                vd.Add(semantic, (Source)sourceRef.Object);
            }
            return new ElementRef { Element = element, Object = vd };
        }

        [ParserOf("controller")]
        private ElementRef Parse_controller(XElement element, object @param)
        {
            return ResolveElement(element.Elements().First(), param);
        }

        [ParserOf("skin")]
        private unsafe ElementRef Parse_skin(XElement element, object @param)
        {
            var crateMeshResult = (CreateMeshResult)ResolveElementByUrl(_libraryGeometries, element.GetAttribute("source"),
                                  new CreateSkelletalMeshInput { }).Object;

            var mesh = crateMeshResult.Mesh;
            MeshSkin skin = new MeshSkin(mesh);

            Frame[] bones = null;
            Matrix[] invBindBones = null;
            //Matrix bindShapeMatix = Matrix.Identity;

            var buffer = mesh.VertexBuffer.ToArray<byte>();
            fixed (byte* pBuffer = buffer)
            {
                var xbindShape = element.GetElementByTag("bind_shape_matrix");
                if (xbindShape != null)
                {
                    var bindShape = ParseMatrix(xbindShape.Value);                 

                    skin.BindShapePose = bindShape;
                }

                #region Parse Joints

                var xjoint = element.GetElementByTag("joints");
                if (xjoint != null)
                {
                    foreach (var xinput in xjoint.GetElementsByTag("input"))
                    {
                        Input input = new Input
                        {
                            Semantic = xinput.GetAttribute("semantic"),
                            Source = (Source)ResolveElementByUrl(element, xinput.GetAttribute("source")).Object
                        };
                        if (input.Semantic == "JOINT")
                            bones = GetMeshBones(input.Source);
                        else if (input.Semantic == "INV_BIND_MATRIX")
                            invBindBones = ConvertToMatrices(input.Source);

                    }
                }

                skin.Bones = bones;
                skin.BoneBindingMatrices = invBindBones;

                #endregion

                #region Parse Weights

                var xVertexWeights = element.GetElementByTag("vertex_weights");
                Input inputJoints = new Input();
                Input inputWeights = new Input();

                #region Parse Inputs

                foreach (var xinput in xVertexWeights.GetElementsByTag("input"))
                {
                    Input input = new Input
                    {
                        Semantic = xinput.GetAttribute("semantic"),
                        Source = (Source)ResolveElementByUrl(element, xinput.GetAttribute("source")).Object,
                        Offset = int.Parse(xinput.GetAttribute("offset") ?? "0")
                    };

                    if (input.Semantic == "JOINT")
                    {
                        if (bones == null)
                            bones = GetMeshBones(input.Source);
                        inputJoints = input;
                    }
                    else if (input.Semantic == "WEIGHT")
                        inputWeights = input;
                }

                #endregion

                #region Assign Skin

                /*Contains a list of integers, each specifying the number of
                  bones associated with one of the influences defined by
                  <vertex_weights>*/
                int[] bonesPerVertex = ParseIntArray(xVertexWeights.GetElementByTag("vcount").Value);

                /*Contains a list of indices that describe which bones and
                  weight are associated with each vertex. An index of -1
                  into the array of joints refers to the bind shape. Weights
                  should be normalized before use*/
                int[] skinIndexes = ParseIntArray(xVertexWeights.GetElementByTag("v").Value);


                VertexBufferBuilder vb = crateMeshResult.VertexBuilder;
                int maxVertexInfluences = int.MinValue;
                float[] weights = (float[])inputWeights.Source.Array;
                int offset = 0;

                BufferView<Vector4> iv = new BufferView<Vector4>((IntPtr)pBuffer, mesh.VertexDescriptor, IASemantic.BlendIndices, 0, mesh.VertexCount);//mesh.GetVertexViewStream<Int4>(DeclarationUsage.BlendIndices, 0);
                BufferView<Vector4> wv = new BufferView<Vector4>((IntPtr)pBuffer, mesh.VertexDescriptor, IASemantic.BlendWeight, 0, mesh.VertexCount); //mesh.GetVertexViewStream<Vector4>(DeclarationUsage.BlendWeight, 0);

                for (int i = 0; i < bonesPerVertex.Length; i++)
                {
                    Vector4 vBoneIndices = new Vector4();
                    Vector4 vWeights = new Vector4();

                    float* pvBoneIndices = (float*)&vBoneIndices;
                    float* pvWeights = (float*)&vWeights;
                    float weightsSum = 0;                    
                    int lastIndex = 0;
                    int bonesCount = bonesPerVertex[i];
                    //if (bonesCount > 4)
                    //    throw new InvalidOperationException("The number of bones per vertex must be equal or lest than 4");

                    maxVertexInfluences = Math.Max(maxVertexInfluences, bonesCount);
                    
                    for (int j = 0; j < bonesCount; j++)
                    {
                        int boneIndex = skinIndexes[offset + inputJoints.Offset];
                        if (boneIndex >= 0 && j < 4)
                        {
                            pvBoneIndices[j] = boneIndex;
                            pvWeights[j] = weights[skinIndexes[offset + inputWeights.Offset]];
                            weightsSum += pvWeights[j];
                            lastIndex = j;
                        }
                        else if(boneIndex == -1)
                        {
                            //the vertex is binded to the bind_shape_matrix
                            //for this to work An additional bone must be created which holds the bind_shape_matrix
                            // not implemented yet!!
                        }
                        offset += 2;
                    }
                    lastIndex++;
                    if (weightsSum < 1.0 - Numerics.Epsilon)
                    {
                        if (lastIndex < 4)
                            pvWeights[lastIndex] = 1 - weightsSum;
                    }

                    var vertexes = vb.GetVertices(i);
                    if (vertexes != null)
                    {
                        foreach (var item in vertexes)
                        {
                            iv[item.VertexBufferIndex] = vBoneIndices;
                            wv[item.VertexBufferIndex] = vWeights;
                        }
                    }

                }                

                if (maxVertexInfluences > 4)
                {
                    maxVertexInfluences = 4;
                    //throw new ArgumentOutOfRangeException("BoneIndice");
                }

                skin.MaxVertexInfluences = maxVertexInfluences;

                #endregion

                #endregion Parse Weights

                FrameSkin.RegisterRenders();

                var render = (IFrameSkinRender)RenderManager.GetRender<FrameSkin>();

                if (bones.Length > render.MaxPaletteMatrices)
                {
                    //mesh.SetVertexBufferData(buffer, 0, buffer.Length);
                    mesh.VertexBuffer.Write(buffer, 0, buffer.Length);
                    skin.SkinMesh(render.MaxPaletteMatrices);
                }
                else
                {
                    //for (int i = 0; i < mesh.VertexCount; i++)
                    //{
                    //    int bIndices = iv[i];
                    //    Vector4 bWeights = wv[i];
                    //    for (int j = 0; j < 4; j++)
                    //    {
                    //        if (((byte*)&bIndices)[j] == 255)
                    //        {
                    //            ((byte*)&bIndices)[j] = 0;
                    //            ((float*)&bWeights)[j] = 0;
                    //        }
                    //    }
                    //}

                    mesh.VertexBuffer.Write(buffer, 0, buffer.Length);
                    //mesh.SetVertexBufferData(buffer, 0, buffer.Length);
                }

            }
            _skins.Add(skin);
            if (_report != null)
                _report.OnObjectCreated(skin);

            return new ElementRef { Element = element, Object = skin };
        }               

        [ParserOf("light")]
        private ElementRef Parse_Light(XElement element, object @param)
        {            
            Light light = new Light();           
            var technique = element.GetElementByTag("technique_common");

            technique.FindElementByTag("directional/color", e =>
            {
                Vector3 color = ParseVector3(e.Value);
                light.Type = LightType.Directional;
                light.Diffuse = color;
                light.Specular = color;                                
            });

            technique.FindElementByTag("spot", e =>
            {
                light.Type = LightType.Spot;

                e.FindElementByTag("color", c =>
                {
                    Vector3 color = ParseVector3(c.Value);
                    light.Diffuse = color;
                    light.Specular = color;                    
                });

                e.FindElementByTag("constant_attenuation", c => light.Attenuation0 = float.Parse(c.Value));
                e.FindElementByTag("linear_attenuation", c => light.Attenuation1 = float.Parse(c.Value));
                e.FindElementByTag("quadratic_attenuation", c => light.Attenuation2 = float.Parse(c.Value));
                e.FindElementByTag("falloff_angle", a => light.SpotPower = (float)Math.Log(0.001, Math.Cos(double.Parse(a.Value))));
            });

            technique.FindElementByTag("point", e =>
            {
                light.Type = LightType.Point;

                e.FindElementByTag("color", c =>
                {
                    Vector3 color = ParseVector3(c.Value);
                    light.Diffuse = color;
                    light.Specular = color;
                });

                e.FindElementByTag("constant_attenuation", c => light.Attenuation0 = float.Parse(c.Value));
                e.FindElementByTag("linear_attenuation", c => light.Attenuation1 = float.Parse(c.Value));
                e.FindElementByTag("quadratic_attenuation", c => light.Attenuation2 = float.Parse(c.Value));
                e.FindElementByTag("zfar", a => light.EffectiveRange = float.Parse(a.Value));
            });

            element.FindDescendantByTag("intensity", x => light.Intensity = float.Parse(x.Value));

            if (light.Diffuse != Color3.Black)
                light.Enable = true;

            _lights.Add(light);
            if (_report != null)
                _report.OnObjectCreated(light);
            return new ElementRef { Element = element, Object = light };
        }

        [ParserOf("camera")]
        private ElementRef Parse_Camera(XElement element, object @param)
        {
            var colladaNode = (XElement)param;
            var camera = Camera.FromOrientation(GetName(colladaNode));
            element.FindDescendantByTag("technique_common", tech =>
            {
                tech.FindElementByTag("perspective", e =>
                {
                    camera.Type = ProjectionType.Perspective;
                    e.FindElementByTag("xfov", x => camera.FieldOfView = Numerics.ToRadians(float.Parse(x.Value)));
                    e.FindElementByTag("aspect_ratio", x => camera.AspectRatio = float.Parse(x.Value));
                    e.FindElementByTag("znear", x => camera.ZNear = float.Parse(x.Value));
                    e.FindElementByTag("zfar", x => camera.ZFar = float.Parse(x.Value));
                });
                tech.FindElementByTag("orthographic", e =>
                {
                    camera.Type = ProjectionType.Orthographic;
                    //e.FindElementByTag("xmag", x => camera.OrthoWidth = float.Parse(x.Value));
                    e.FindElementByTag("aspect_ratio", x => camera.AspectRatio = float.Parse(x.Value));
                    e.FindElementByTag("znear", x => camera.ZNear = float.Parse(x.Value));
                    e.FindElementByTag("zfar", x => camera.ZFar = float.Parse(x.Value));
                });
            });

            camera.LocalFrame = new Euler(0, Numerics.PIover2, 0).ToMatrix();
            camera.Transform(Matrix.Identity);

            _cameras.Add(camera);
            if (_report != null)
                _report.OnObjectCreated(camera);

            return new ElementRef { Element = element, Object = camera };
        }

        [ParserOf("library_animations")]
        private ElementRef Parse_animations_lib(XElement element, object @param)
        {            
            int index = 0;
            string name = Path.GetFileNameWithoutExtension(FileName);

            foreach (var item in element.GetElementsByTag("animation"))
            {
                var result = ResolveElement(item, param);
                if (result.Object is KeyFrameAnimation)
                {
                    _animations.Add((KeyFrameAnimation)result.Object);
                }
                else if (result.Object is CurvesContainer)
                {
                    if (_animations.Count == 0)
                        _animations.Add(new KeyFrameAnimation(name + index++));

                    KeyFrameAnimation layer = (KeyFrameAnimation)_animations.Last();
                    var node = (CurvesContainer)result.Object;
                    layer.Nodes.Add(node);                    
                }
            }

            if (_animations.Count == 0)
                return new ElementRef { Element = element, Object = null };

            return new ElementRef { Element = element, Object = _animations };
        }

        [ParserOf("animation")]
        private ElementRef Parse_animation(XElement element, object @param)
        {
            if (element.GetElementByTag("source") != null)
                return new ElementRef { Element = element, Object = ParseAnimationCurve(element) };

            CurvesContainer node = null;
            List<KeyFrameCurve> curves = new List<KeyFrameCurve>();
            List<float[]> keys = new List<float[]>();
            List<int> samplerKeys = new List<int>();
            KeyFrameAnimation layer = null;
            FrameAnimationController controller = null;

            foreach (var item in element.GetElementsByTag("animation"))
            {
                var result = Parse_animation(item, param);

                if (result.Object is AnimCurveResult)
                {
                    var curveInfo = (AnimCurveResult)result.Object;
                    if (curveInfo.Channels.Count == 0)
                        continue;

                    if (node == null)
                        node = new CurvesContainer(element.GetName());

                    foreach (var channel in curveInfo.Channels)
                    {
                        var animCurve = channel.Sampler.Sampler;
                        FixCurve(animCurve);

                        curves.Add(animCurve);                      

                        /*
                         *  for each channel create an AnimCurve and the corresponding 
                         *  IOutputChannel instance
                         */
                        if (node.Name != null && node.Name != channel.EntityName)
                            throw new InvalidDataException("Animation format not supperted");

                        if (controller == null)
                        {
                            controller = GetAnimationController(channel.EntityName);
                            if (controller != null)
                            {
                                controller.LinkAnimationCurve(node);
                            }
                        }

                        if (controller != null)
                        {                             
                            controller.LinkOutput(animCurve);                           
                        }
                       
                        int index = keys.FindIndex(x => Equals(x , curveInfo.Inputs[channel.Sampler.KeyInde]));
                        if (index < 0)
                        {
                            keys.Add(curveInfo.Inputs[channel.Sampler.KeyInde]);
                            samplerKeys.Add(keys.Count - 1);
                        }
                        else
                            samplerKeys.Add(index);
                    }

                }
                else if (result.Object is CurvesContainer)
                {
                    if (layer == null)
                        layer = new KeyFrameAnimation(element.GetName());

                    var childNode = (CurvesContainer)result.Object;
                    layer.Nodes.Add(childNode);
                }
            }

            if (_report != null)
                _report.OnObjectCreated(node);

            if (node != null)
            {
                node.CurveKeys = keys.ToArray();
                node.Curves = curves.ToArray();
                node.KeysIndices = samplerKeys.ToArray();

                return new ElementRef { Element = element, Object = node };
            }
            else if (layer != null)
                return new ElementRef { Element = element, Object = layer };            

            return new ElementRef { Element = element, Object = null };
        }

        private void FixCurve(Animations.KeyFrameCurve animCurve)
        {
            string[] parts = animCurve.Name.Split('/');
            if (parts.Length != 2)
                return;

            string transform = parts[1];
            if (_zUp)
            {
                if (transform == "translate.Y")
                {
                    animCurve.Name = parts[0] + "/translate.Z";
                }
                else if (transform == "translate.Z")
                {
                    animCurve.Name = parts[0] + "/translate.Y";
                }
                else if (transform == "rotateY.ANGLE")
                {
                    animCurve.Name = parts[0] + "/rotateZ.ANGLE";
                }
                else if (transform == "rotateZ.ANGLE")
                {
                    animCurve.Name = parts[0] + "/rotateY.ANGLE";
                }
            }

            if (transform.EndsWith("ANGLE"))
            {
                var output = animCurve.Output;
                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = -Numerics.ToRadians(output[i]);
                }
            }
        }

        [ParserOf("sampler")]
        private ElementRef ParseAnimationSampler(XElement element, object @param)
        {
            var result = (AnimCurveResult)param;
            KeyFrameCurve curveSampler = new KeyFrameCurve() { Name = element.GetId() };
            SamplerKeys tuple = new SamplerKeys() { Sampler = curveSampler };
            result.Samplers.Add(tuple);                    
            var sampler = (InputCollection)Parse_InputCollection(element, param).Object;

            var input = sampler.GetInput("INPUT");
            var keys = (float[])input.Source.Array;
            int index = result.Inputs.FindIndex(x => Equals(x, keys));
            if (index >= 0)
               tuple.KeyInde = index;
            else
            {                
                result.Inputs.Add(keys);
                tuple.KeyInde = result.Inputs.Count - 1;
            }

            var output = sampler.GetInput("OUTPUT");
            var inTangent = sampler.GetInput("IN_TANGENT");
            var outTangent = sampler.GetInput("OUT_TANGENT");
            var interpolation = sampler.GetInput("INTERPOLATION");

            ConvertInto(output.Source);

            curveSampler.Output = CheckForContantValue((float[])output.Source.Array);
            curveSampler.OutputDim = output.Source.Accesor.Stride;

            curveSampler.InTangent = inTangent.Source != null ? CheckForContantValue((float[])inTangent.Source.Array) : null;
            curveSampler.OutTangent = outTangent.Source != null ? CheckForContantValue((float[])outTangent.Source.Array) : null;
            curveSampler.InterpolationType = (InterpolationMethod)Enum.Parse(typeof(InterpolationMethod), ((string[])interpolation.Source.Array)[0]);

            if (curveSampler.OutputDim == 4)
                curveSampler.InterpolationType = InterpolationMethod.QuatSlerp;

            return new ElementRef { Element = element, Object = tuple };
        }

        private AnimCurveResult ParseAnimationCurve(XElement element)
        {
            AnimCurveResult result = new AnimCurveResult() { Name = element.GetName() };

            foreach (var channel in element.GetElementsByTag("channel"))
            {
                var target = channel.GetAttribute("target");
                string[] entity = target.Split('/');

                var sampler = (SamplerKeys)ResolveElementByUrl(element, channel.GetAttribute("source"), result).Object;
                sampler.Sampler.Name = target;
                result.Channels.Add(new ChannelInfo { Sampler = sampler, EntityName = entity[0], TargetString = target });
            }

            return result;
        }       


        #endregion
    }
}
