﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorWrite
    {        
        public bool R;
        public bool G;
        public bool B;
        public bool A;
        public ColorWrite(bool r, bool g, bool b, bool a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlendDesc
    {
        public bool AlphaToCoverageEnable;
        public bool BlendEnable;
        public Blend SrcBlend;
        public Blend DestBlend;
        public BlendOperation BlendOp;
        public Blend SrcBlendAlpha;
        public Blend DestBlendAlpha;
        public BlendOperation BlendOpAlpha;
        public ColorWrite WriteMask;
        public Color4 BlendFactor;

        public BlendDesc(bool alphaToCoverageEnable = false,
                        bool blendEnable = false,
                        Blend srcBlend = Blend.One,
                        Blend destBlend = Blend.Zero,
                        BlendOperation blendOp = BlendOperation.Add,
                        Blend srcBlendAlpha = Blend.One,
                        Blend destBlendAlpha = Blend.Zero,
                        BlendOperation blendOpAlpha = BlendOperation.Add,
                        ColorWrite? colorWrite = null,
                        Color4? blendFactor = null)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            BlendEnable = blendEnable;
            SrcBlend = srcBlend;
            DestBlend = destBlend;
            BlendOp = blendOp;
            SrcBlendAlpha = srcBlendAlpha;
            DestBlendAlpha = destBlendAlpha;
            BlendOpAlpha = blendOpAlpha;
            WriteMask = colorWrite ?? new ColorWrite(true, true, true, true);
            BlendFactor = blendFactor ?? new Color4(1, 1, 1, 1);
        }

        public BlendDesc(BlendDesc other)
        {
            unsafe
            {
                fixed(BlendDesc* p = &this)
                {
                    *p = other;
                }
            }
        }

        public BlendDesc(bool setdefault)
        {
            AlphaToCoverageEnable = false;
            BlendEnable = false;
            SrcBlend = Blend.One;
            DestBlend = Blend.Zero;
            BlendOp = BlendOperation.Add;
            SrcBlendAlpha = Blend.One;
            DestBlendAlpha = Blend.Zero;
            BlendOpAlpha = BlendOperation.Add;
            WriteMask = new ColorWrite(true, true, true, true);           
            BlendFactor = new Color4(1, 1, 1, 1);
        }


        public void SetDefaults()
        {
            AlphaToCoverageEnable = false;
            BlendEnable = false;
            SrcBlend = Blend.One;
            DestBlend = Blend.Zero;
            BlendOp = BlendOperation.Add;
            SrcBlendAlpha = Blend.One;
            DestBlendAlpha = Blend.Zero;
            BlendOpAlpha = BlendOperation.Add;
            WriteMask = new ColorWrite(true, true, true, true);            
            BlendFactor = new Color4(1, 1, 1, 1);
        }

        public static BlendDesc Default
        {
            get
            {
                BlendDesc b = new BlendDesc();
                b.SetDefaults();
                return b;
            }
        }
    }

    public abstract class BlendState : PipelineState<BlendDesc>
    {
        public BlendState(BlendDesc state)
            : base(state)
        {
          
        }          
    }
}
