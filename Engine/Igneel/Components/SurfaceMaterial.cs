﻿using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public abstract class SurfaceMaterial : Resource
    {
        private LayerSurface _surface;
        private bool _containsAlpha;

        public SurfaceMaterial() 
        {
            _surface = LayerSurface.Default;
        }

        public SurfaceMaterial(string name) : 
            base(name, null) 
        {
            _surface = LayerSurface.Default;
        }

        public SurfaceMaterial(string name, ResourceManager manager) : base(name, manager) 
        {
            _surface = LayerSurface.Default;
        }

        public LayerSurface Surface
        {
            get { return _surface; }
            set { _surface = value; }
        }

        public Color3 Diffuse
        {
            get
            {
                return (Color3)_surface.Diffuse;
            }
            set
            {
                _surface.Diffuse = new Color4(value, _surface.Diffuse.A);
            }
        }

        public float SpecularIntensity
        {
            get
            {
                return _surface.Specular;
            }
            set
            {
                _surface.Specular = value;
            }
        }

        public float Alpha
        {
            get { return _surface.Diffuse.A; }
            set
            {
                _surface.Diffuse.A = value;
                if (value != 1)
                    _containsAlpha = true;
            }
        }

        public float Reflectivity { get { return _surface.Reflectivity; } set { _surface.Reflectivity = value; } }

        public float Refractitity { get { return _surface.Refractitity; } set { _surface.Refractitity = value; } }

        public float SpecularPower { get { return _surface.SpecularPower; } set { _surface.SpecularPower = value; } }

        public float EmisiveIntensity { get { return _surface.Emisive; } set { _surface.Emisive = value; } }

        public bool ContainsTrasparency { get { return _containsAlpha; } protected set { _containsAlpha = value; } }

    }

}
