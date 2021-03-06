﻿using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.States;

namespace Igneel.Techniques
{
    public class ReflectiveNodeTechnique : BindableFrameTechnique<ReflectiveNodeTechnique>,IDeferreable
    {
        RenderTexture2D _rtReflection;
        RenderTexture2D _rtRefraction;
        Plane _plane;
        int _width;
        int _height;
        Format _rtFormat;
        Matrix _reflTransform;
        private bool _useReflection = true;     
        private bool _useRefraction = true;             
        Camera _camera;
        static RasterizerState _rasterizer;

        public ReflectiveNodeTechnique(int width, int height ,Plane plane, Format format = Format.B8G8R8X8_UNORM)            
        {
            _plane = plane;
            _width = width;
            _height = height;
            _rtFormat = format;                       

            _camera = new Camera();
            _reflTransform = Matrix.Reflection(plane);
        }

        public bool UseReflection
        {
            get { return _useReflection; }
            set 
            { 
                _useReflection = value;
                if (_useReflection && _rtReflection == null)
                {
                    _rtReflection = new RenderTexture2D(_width, _height, _rtFormat, Format.D16_UNORM, GraphicDeviceFactory.Device.BackBuffer.Sampling);
                }
                else if (!_useReflection && _rtReflection != null)
                {
                    _rtReflection.Dispose();
                    _rtReflection = null;
                }
            }
        }

        public bool UseRefraction
        {
            get { return _useRefraction; }
            set 
            {
                _useRefraction = value;
                if (_useRefraction && _rtRefraction == null)
                {
                    _rtRefraction = new RenderTexture2D(_width, _height, _rtFormat, sampling: GraphicDeviceFactory.Device.BackDepthBuffer.Sampling);
                }
                else if (!_useRefraction && _rtRefraction != null)
                {
                    _rtRefraction.Dispose();
                    _rtRefraction = null;
                }
            }
        }

        public Plane ReflectionPlane
        {
            get { return _plane; }
            set 
            {
                _plane = value;
                _reflTransform = Matrix.Reflection(_plane);
            }
        }
       
        public Size Size
        {
            get { return new Size(_width, _height); }
            set
            {
                _width = value.Width;
                _height = value.Height;                
            }
        }

        public Format RtFormat
        {
            get { return _rtFormat; }
            set { _rtFormat = value; }
        }

        public Matrix ReflTransform
        {
            get { return _reflTransform; }            
        }

        public Texture2D ReflectionTexture
        {
            get { return _rtReflection.Texture; }
        }

        public Texture2D RefractionTexture
        {
            get { return _rtRefraction.Texture; }
        }
      
        public override void Apply()
        {
            if (EngineState.Lighting.Reflection.Enable)
            {
                var graphic = GraphicDeviceFactory.Device;
                graphic.SaveRenderTarget();               

                var tech = Service.Require<ClipPlaneSceneTechnique>();
               
                #region RenderReflection

                if (_useReflection)
                {
                    if (_rasterizer == null)
                    {
                        _rasterizer = graphic.CreateRasterizerState(new RasterizerDesc(true)
                        {
                            Cull = CullMode.Front
                        });
                    }
                    graphic.RasterizerStack.Push(_rasterizer);

                    //var oldcamera = Engine.Scene.ActiveCamera;
                   
                    //camera.Projection = oldcamera.Projection;
                    //var front = Vector3.TransformNormal(oldcamera.Front, _reflTransform);
                    //var right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, front));
                    //var up = Vector3.Normalize(Vector3.Cross(front, right));

                    //camera.Position = Vector3.TransformCoordinates(oldcamera.Position, _reflTransform);
                    //camera.Right = right;
                    //camera.Up = up;
                    //camera.Front = front;
                    //camera.CommitChanges();
                    //Engine.Scene.ActiveCamera = camera;

                    _rtReflection.SetTarget(graphic);
                    graphic.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color4.Black, 1, 0);

                    tech.ReflectionMatrix = _reflTransform;
                    tech.Plane = _plane;

                    RenderManager.ApplyTechnique(tech);

                    tech.ReflectionMatrix = Matrix.Identity;
                    tech.Plane = new Plane();

                    graphic.RasterizerStack.Pop();
                    //Engine.Scene.ActiveCamera = oldcamera;
                }

                #endregion

                #region RenderRefraction
                if (_useRefraction)
                {                    
                    _rtRefraction.SetTarget(graphic);
                    GraphicDeviceFactory.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1, 0);

                    tech.ReflectionMatrix = Matrix.Identity;
                    RenderManager.ApplyTechnique(tech);

                }
                #endregion
               
                graphic.RestoreRenderTarget();               
            }
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                _rtReflection.Dispose();
                _rtRefraction.Dispose();
            }
            base.OnDispose(d);
        }       

        public void CommitChanges()
        {
            
        }
    }

    public class ClipPlaneSceneTechnique : BindableSceneTechnique<ClipPlaneSceneTechnique>
    {        
        public Matrix ReflectionMatrix;
        public Plane Plane;        

    }
   
}
