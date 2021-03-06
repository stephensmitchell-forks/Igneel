﻿using Igneel.Graphics;
using Igneel.Rendering;

namespace Igneel.Effects
{
    public class HdrEffect:Effect
    {
        public const int DownSampler4X4 = 0;
        public const int SampleAvgLum = 1;
        public const int ResampleAvgLum = 2;
        public const int ResampleAvgLumExp = 3;
        public const int CalculateAdaptedLum = 4;
        public const int BrightPassFilter = 5;
        public const int GaussBlur5X5 = 6;
        public const int DownSampler2X2 = 7;
        public const int Bloom = 8;
        public const int Star = 9;
        public const int MergeTexture = 10;
        public const int FinalScenePass = 11;

        public HdrEffect(GraphicDevice device)
            : base(device) { }


        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("DownScale4x4").Pass<VertexPTxH>("RenderQuadVS",         "DownScale4x4PS"),
                Tech("SampleLumInitial").Pass<VertexPTxH>("RenderQuadVS",     "SampleLumInitialPS"),
                Tech("SampleLumIterative").Pass<VertexPTxH>("RenderQuadVS",   "SampleLumIterativePS"),
                Tech("SampleLumFinal").Pass<VertexPTxH>("RenderQuadVS",       "SampleLumFinalPS"),
                Tech("CalculateAdaptedLum").Pass<VertexPTxH>("RenderQuadVS",  "CalculateAdaptedLumPS"),
                Tech("BrightPassFilter").Pass<VertexPTxH>("RenderQuadVS",     "BrightPassFilterPS"),
                Tech("GaussianBlur5x5").Pass<VertexPTxH>("RenderQuadVS",      "GaussianBlur5x5PS"),
                Tech("DownScale2x2").Pass<VertexPTxH>("RenderQuadVS",         "DownScale2x2PS"),
                Tech("GaussianBlur15").Pass<VertexPTxH>("RenderQuadVS",       "GaussianBlur15PS"),
                Tech("GaussianBlur8").Pass<VertexPTxH>("RenderQuadVS",        "GaussianBlur8PS"),
                Tech("MergeTexture").Pass<VertexPTxH>("RenderQuadVS",         "MergeTexturePS"),
                Tech("FinalScenePass").Pass<VertexPTxH>("RenderQuadVS",       "FinalScenePassPS")
            };
        }
    }
}
