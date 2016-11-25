﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.OpenGLES2
{
    public enum All
    {
        False,
        NoError = 0,
        NoneOes = 0,
        Zero = 0,
        Points = 0,
        ColorBufferBit0Qcom,
        ColorBufferBit1Qcom,
        ColorBufferBit2Qcom = 4,
        ColorBufferBit3Qcom = 8,
        ColorBufferBit4Qcom = 16,
        ColorBufferBit5Qcom = 32,
        ColorBufferBit6Qcom = 64,
        ColorBufferBit7Qcom = 128,
        DepthBufferBit = 256,
        DepthBufferBit0Qcom = 256,
        DepthBufferBit1Qcom = 512,
        DepthBufferBit2Qcom = 1024,
        StencilBufferBit = 1024,
        DepthBufferBit3Qcom = 2048,
        DepthBufferBit4Qcom = 4096,
        DepthBufferBit5Qcom = 8192,
        ColorBufferBit = 16384,
        DepthBufferBit6Qcom = 16384,
        DepthBufferBit7Qcom = 32768,
        Lines = 1,
        StencilBufferBit0Qcom = 65536,
        LineLoop = 2,
        StencilBufferBit1Qcom = 131072,
        LineStrip = 3,
        Triangles,
        StencilBufferBit2Qcom = 262144,
        TriangleStrip = 5,
        TriangleFan,
        StencilBufferBit3Qcom = 524288,
        StencilBufferBit4Qcom = 1048576,
        StencilBufferBit5Qcom = 2097152,
        StencilBufferBit6Qcom = 4194304,
        StencilBufferBit7Qcom = 8388608,
        MultisampleBufferBit0Qcom = 16777216,
        Add = 260,
        Never = 512,
        MultisampleBufferBit1Qcom = 33554432,
        Less = 513,
        Equal,
        Lequal,
        Greater,
        Notequal,
        Gequal,
        Always,
        SrcColor = 768,
        OneMinusSrcColor,
        SrcAlpha,
        OneMinusSrcAlpha,
        DstAlpha,
        OneMinusDstAlpha,
        DstColor,
        OneMinusDstColor,
        SrcAlphaSaturate,
        MultisampleBufferBit2Qcom = 67108864,
        Front = 1028,
        Back,
        FrontAndBack = 1032,
        InvalidEnum = 1280,
        InvalidValue,
        InvalidOperation,
        StackOverflow,
        StackUnderflow,
        OutOfMemory,
        InvalidFramebufferOperationOes,
        Exp = 2048,
        MultisampleBufferBit3Qcom = 134217728,
        Exp2 = 2049,
        Cw = 2304,
        Ccw,
        CurrentColor = 2816,
        CurrentNormal = 2818,
        CurrentTextureCoords,
        PointSmooth = 2832,
        PointSize,
        SmoothPointSizeRange,
        LineSmooth = 2848,
        LineWidth,
        SmoothLineWidthRange,
        CullFace = 2884,
        CullFaceMode,
        FrontFace,
        Lighting = 2896,
        LightModelTwoSide = 2898,
        LightModelAmbient,
        ShadeModel,
        ColorMaterial = 2903,
        Fog = 2912,
        FogDensity = 2914,
        FogStart,
        FogEnd,
        FogMode,
        FogColor,
        DepthRange = 2928,
        DepthTest,
        DepthWritemask,
        DepthClearValue,
        DepthFunc,
        StencilTest = 2960,
        StencilClearValue,
        StencilFunc,
        StencilValueMask,
        StencilFail,
        StencilPassDepthFail,
        StencilPassDepthPass,
        StencilRef,
        StencilWritemask,
        MatrixMode = 2976,
        Normalize,
        Viewport,
        ModelviewStackDepth,
        ProjectionStackDepth,
        TextureStackDepth,
        ModelviewMatrix,
        ProjectionMatrix,
        TextureMatrix,
        AlphaTest = 3008,
        AlphaTestFunc,
        AlphaTestRef,
        Dither = 3024,
        BlendDst = 3040,
        BlendSrc,
        Blend,
        LogicOpMode = 3056,
        ColorLogicOp = 3058,
        ScissorBox = 3088,
        ScissorTest,
        ColorClearValue = 3106,
        ColorWritemask,
        PerspectiveCorrectionHint = 3152,
        PointSmoothHint,
        LineSmoothHint,
        FogHint = 3156,
        UnpackAlignment = 3317,
        PackAlignment = 3333,
        AlphaScale = 3356,
        MaxLights = 3377,
        MaxClipPlanes,
        MaxClipPlanesImg = 3378,
        MaxTextureSize,
        MaxModelviewStackDepth = 3382,
        MaxProjectionStackDepth = 3384,
        MaxTextureStackDepth,
        MaxViewportDims,
        SubpixelBits = 3408,
        RedBits = 3410,
        GreenBits,
        BlueBits,
        AlphaBits,
        DepthBits,
        StencilBits,
        Texture2D = 3553,
        MultisampleBufferBit4Qcom = 268435456,
        DontCare = 4352,
        Fastest,
        Nicest,
        Ambient = 4608,
        Diffuse,
        Specular,
        Position,
        SpotDirection,
        SpotExponent,
        SpotCutoff,
        ConstantAttenuation,
        LinearAttenuation,
        QuadraticAttenuation,
        Byte = 5120,
        UnsignedByte,
        Short,
        UnsignedShort,
        UnsignedInt = 5125,
        Float,
        Fixed = 5132,
        FixedOes = 5132,
        Clear = 5376,
        And,
        AndReverse,
        Copy,
        AndInverted,
        Noop,
        Xor,
        Or,
        Nor,
        Equiv,
        Invert,
        OrReverse,
        CopyInverted,
        OrInverted,
        Nand,
        Set,
        Emission = 5632,
        Shininess,
        AmbientAndDiffuse,
        Modelview = 5888,
        Projection,
        Texture,
        ColorExt = 6144,
        DepthExt,
        StencilExt,
        Alpha = 6406,
        Rgb,
        Rgba,
        Luminance,
        LuminanceAlpha,
        Flat = 7424,
        Smooth,
        Keep = 7680,
        Replace,
        Incr,
        Decr,
        Vendor = 7936,
        Renderer,
        Version,
        Extensions,
        MultisampleBufferBit5Qcom = 536870912,
        Modulate = 8448,
        Decal,
        TextureEnvMode = 8704,
        TextureEnvColor,
        TextureEnv = 8960,
        TextureGenModeOes = 9472,
        Nearest = 9728,
        Linear,
        NearestMipmapNearest = 9984,
        LinearMipmapNearest,
        NearestMipmapLinear,
        LinearMipmapLinear,
        TextureMagFilter = 10240,
        TextureMinFilter,
        TextureWrapS,
        TextureWrapT,
        Repeat = 10497,
        PolygonOffsetUnits = 10752,
        ClipPlane0 = 12288,
        ClipPlane0Img = 12288,
        ClipPlane1,
        ClipPlane1Img = 12289,
        ClipPlane2,
        ClipPlane2Img = 12290,
        ClipPlane3,
        ClipPlane3Img = 12291,
        ClipPlane4,
        ClipPlane4Img = 12292,
        ClipPlane5,
        ClipPlane5Img = 12293,
        Light0 = 16384,
        MultisampleBufferBit6Qcom = 1073741824,
        Light1 = 16385,
        Light2,
        Light3,
        Light4,
        Light5,
        Light6,
        Light7,
        MultisampleBufferBit7Qcom = -2147483648,
        FuncAddOes = 32774,
        MinExt,
        MaxExt,
        BlendEquationOes,
        BlendEquationRgbOes = 32777,
        FuncSubtractOes,
        FuncReverseSubtractOes,
        UnsignedShort4444 = 32819,
        UnsignedShort5551,
        PolygonOffsetFill = 32823,
        PolygonOffsetFactor,
        RescaleNormal = 32826,
        Rgb8Oes = 32849,
        Rgba4Oes = 32854,
        Rgb5A1Oes,
        Rgba8Oes,
        TextureBinding2D = 32873,
        VertexArray = 32884,
        NormalArray,
        ColorArray,
        TextureCoordArray = 32888,
        VertexArraySize = 32890,
        VertexArrayType,
        VertexArrayStride,
        NormalArrayType = 32894,
        NormalArrayStride,
        ColorArraySize = 32897,
        ColorArrayType,
        ColorArrayStride,
        TextureCoordArraySize = 32904,
        TextureCoordArrayType,
        TextureCoordArrayStride,
        VertexArrayPointer = 32910,
        NormalArrayPointer,
        ColorArrayPointer,
        TextureCoordArrayPointer = 32914,
        Multisample = 32925,
        SampleAlphaToCoverage,
        SampleAlphaToOne,
        SampleCoverage,
        SampleBuffers = 32936,
        Samples,
        SampleCoverageValue,
        SampleCoverageInvert,
        BlendDstRgbOes = 32968,
        BlendSrcRgbOes,
        BlendDstAlphaOes,
        BlendSrcAlphaOes,
        BgraExt = 32993,
        BgraImg = 32993,
        PointSizeMin = 33062,
        PointSizeMax,
        PointFadeThresholdSize,
        PointDistanceAttenuation,
        ClampToEdge = 33071,
        GenerateMipmap = 33169,
        GenerateMipmapHint,
        DepthComponent16Oes = 33189,
        DepthComponent24Oes,
        DepthComponent32Oes,
        UnsignedShort565 = 33635,
        UnsignedShort4444RevExt = 33637,
        UnsignedShort4444RevImg = 33637,
        UnsignedShort1555RevExt,
        MirroredRepeatOes = 33648,
        AliasedPointSizeRange = 33901,
        AliasedLineWidthRange,
        Texture0 = 33984,
        Texture1,
        Texture2,
        Texture3,
        Texture4,
        Texture5,
        Texture6,
        Texture7,
        Texture8,
        Texture9,
        Texture10,
        Texture11,
        Texture12,
        Texture13,
        Texture14,
        Texture15,
        Texture16,
        Texture17,
        Texture18,
        Texture19,
        Texture20,
        Texture21,
        Texture22,
        Texture23,
        Texture24,
        Texture25,
        Texture26,
        Texture27,
        Texture28,
        Texture29,
        Texture30,
        Texture31,
        ActiveTexture,
        ClientActiveTexture,
        MaxTextureUnits,
        Subtract = 34023,
        MaxRenderbufferSizeOes,
        AllCompletedNv = 34034,
        FenceStatusNv,
        FenceConditionNv,
        DepthStencilOes = 34041,
        UnsignedInt248Oes,
        MaxTextureLodBiasExt = 34045,
        TextureMaxAnisotropyExt,
        MaxTextureMaxAnisotropyExt,
        TextureFilterControlExt,
        TextureLodBiasExt,
        IncrWrapOes = 34055,
        DecrWrapOes,
        NormalMapOes = 34065,
        ReflectionMapOes,
        TextureCubeMapOes,
        TextureBindingCubeMapOes,
        TextureCubeMapPositiveXOes,
        TextureCubeMapNegativeXOes,
        TextureCubeMapPositiveYOes,
        TextureCubeMapNegativeYOes,
        TextureCubeMapPositiveZOes,
        TextureCubeMapNegativeZOes,
        MaxCubeMapTextureSizeOes = 34076,
        Combine = 34160,
        CombineRgb,
        CombineAlpha,
        RgbScale,
        AddSigned,
        Interpolate,
        Constant,
        PrimaryColor,
        Previous,
        Src0Rgb = 34176,
        Src1Rgb,
        Src2Rgb,
        Src0Alpha = 34184,
        Src1Alpha,
        Src2Alpha,
        Operand0Rgb = 34192,
        Operand1Rgb,
        Operand2Rgb,
        Operand0Alpha = 34200,
        Operand1Alpha,
        Operand2Alpha,
        VertexArrayBindingOes = 34229,
        NumCompressedTextureFormats = 34466,
        CompressedTextureFormats,
        MaxVertexUnitsOes,
        WeightArrayTypeOes = 34473,
        WeightArrayStrideOes,
        WeightArraySizeOes,
        WeightArrayPointerOes,
        WeightArrayOes,
        Dot3Rgb,
        Dot3Rgba,
        Dot3RgbaImg = 34479,
        BufferSize = 34660,
        BufferUsage,
        AtcRgbaInterpolatedAlphaAmd = 34798,
        GL_3DcXAmd = 34809,
        GL_3DcXyAmd,
        WriteonlyRenderingQcom = 34851,
        BlendEquationAlphaOes = 34877,
        MatrixPaletteOes = 34880,
        MaxPaletteMatricesOes = 34882,
        CurrentPaletteMatrixOes,
        MatrixIndexArrayOes,
        MatrixIndexArraySizeOes = 34886,
        MatrixIndexArrayTypeOes,
        MatrixIndexArrayStrideOes,
        MatrixIndexArrayPointerOes,
        PointSpriteOes = 34913,
        CoordReplaceOes,
        ArrayBuffer = 34962,
        ElementArrayBuffer,
        ArrayBufferBinding,
        ElementArrayBufferBinding,
        VertexArrayBufferBinding,
        NormalArrayBufferBinding,
        ColorArrayBufferBinding,
        TextureCoordArrayBufferBinding = 34970,
        WeightArrayBufferBindingOes = 34974,
        WriteOnlyOes = 35001,
        BufferAccessOes = 35003,
        BufferMappedOes,
        BufferMapPointerOes,
        StaticDraw = 35044,
        DynamicDraw = 35048,
        Depth24Stencil8Oes = 35056,
        PointSizeArrayTypeOes = 35210,
        PointSizeArrayStrideOes,
        PointSizeArrayPointerOes,
        ModelviewMatrixFloatAsIntBitsOes,
        ProjectionMatrixFloatAsIntBitsOes,
        TextureMatrixFloatAsIntBitsOes,
        Palette4Rgb8Oes = 35728,
        Palette4Rgba8Oes,
        Palette4R5G6B5Oes,
        Palette4Rgba4Oes,
        Palette4Rgb5A1Oes,
        Palette8Rgb8Oes,
        Palette8Rgba8Oes,
        Palette8R5G6B5Oes,
        Palette8Rgba4Oes,
        Palette8Rgb5A1Oes,
        ImplementationColorReadTypeOes,
        ImplementationColorReadFormatOes,
        PointSizeArrayOes,
        TextureCropRectOes,
        MatrixIndexArrayBufferBindingOes,
        PointSizeArrayBufferBindingOes,
        TextureWidthQcom = 35794,
        TextureHeightQcom,
        TextureDepthQcom,
        TextureInternalFormatQcom,
        TextureFormatQcom,
        TextureTypeQcom,
        TextureImageValidQcom,
        TextureNumLevelsQcom,
        TextureTargetQcom,
        TextureObjectValidQcom,
        StateRestore,
        CompressedRgbPvrtc4Bppv1Img = 35840,
        CompressedRgbPvrtc2Bppv1Img,
        CompressedRgbaPvrtc4Bppv1Img,
        CompressedRgbaPvrtc2Bppv1Img,
        ModulateColorImg,
        RecipAddSignedAlphaImg,
        TextureAlphaModulateImg,
        FactorAlphaModulateImg,
        FragmentAlphaModulateImg,
        AddBlendImg,
        AtcRgbAmd = 35986,
        AtcRgbaExplicitAlphaAmd,
        FramebufferBindingOes = 36006,
        RenderbufferBindingOes,
        FramebufferAttachmentObjectTypeOes = 36048,
        FramebufferAttachmentObjectNameOes,
        FramebufferAttachmentTextureLevelOes,
        FramebufferAttachmentTextureCubeMapFaceOes,
        FramebufferCompleteOes = 36053,
        FramebufferIncompleteAttachmentOes,
        FramebufferIncompleteMissingAttachmentOes,
        FramebufferIncompleteDimensionsOes = 36057,
        FramebufferIncompleteFormatsOes,
        FramebufferUnsupportedOes = 36061,
        ColorAttachment0Oes = 36064,
        DepthAttachmentOes = 36096,
        StencilAttachmentOes = 36128,
        FramebufferOes = 36160,
        RenderbufferOes,
        RenderbufferWidthOes,
        RenderbufferHeightOes,
        RenderbufferInternalFormatOes,
        StencilIndex1Oes = 36166,
        StencilIndex4Oes,
        StencilIndex8Oes,
        RenderbufferRedSizeOes = 36176,
        RenderbufferGreenSizeOes,
        RenderbufferBlueSizeOes,
        RenderbufferAlphaSizeOes,
        RenderbufferDepthSizeOes,
        RenderbufferStencilSizeOes,
        TextureGenStrOes = 36192,
        Rgb565Oes = 36194,
        Etc1Rgb8Oes = 36196,
        TextureExternalOes,
        SamplerExternalOes,
        TextureBindingExternalOes,
        RequiredTextureImageUnitsOes,
        PerfmonGlobalModeQcom = 36768,
        RenderbufferSamplesImg = 37171,
        FramebufferIncompleteMultisampleImg,
        MaxSamplesImg,
        TextureSamplesImg,
        AmdCompressed3DcTexture = 1,
        AmdCompressedAtcTexture = 1,
        AppleTexture2DLimitedNpot = 1,
        ExtBlendMinmax = 1,
        ExtDiscardFramebuffer = 1,
        ExtMultiDrawArrays = 1,
        ExtReadFormatBgra = 1,
        ExtTextureFilterAnisotropic = 1,
        ExtTextureFormatBgra8888 = 1,
        ExtTextureLodBias = 1,
        ImgMultisampledRenderToTexture = 1,
        ImgReadFormat = 1,
        ImgTextureCompressionPvrtc = 1,
        ImgTextureEnvEnhancedFixedFunction = 1,
        ImgUserClipPlane = 1,
        NvFence = 1,
        OesBlendEquationSeparate = 1,
        OesBlendFuncSeparate = 1,
        OesBlendSubtract = 1,
        OesByteCoordinates = 1,
        OesCompressedEtc1Rgb8Texture = 1,
        OesCompressedPalettedTexture = 1,
        OesDepth24 = 1,
        OesDepth32 = 1,
        OesDrawTexture = 1,
        OesEglImage = 1,
        OesEglImageExternal = 1,
        OesElementIndexUint = 1,
        OesExtendedMatrixPalette = 1,
        OesFboRenderMipmap = 1,
        OesFixedPoint = 1,
        OesFramebufferObject = 1,
        OesMapbuffer = 1,
        OesMatrixGet = 1,
        OesMatrixPalette = 1,
        OesPackedDepthStencil = 1,
        OesPointSizeArray = 1,
        OesPointSprite = 1,
        OesQueryMatrix = 1,
        OesReadFormat = 1,
        OesRgb8Rgba8 = 1,
        OesSinglePrecision = 1,
        OesStencil1 = 1,
        OesStencil4 = 1,
        OesStencil8 = 1,
        OesStencilWrap = 1,
        OesTextureCubeMap = 1,
        OesTextureEnvCrossbar = 1,
        OesTextureMirroredRepeat = 1,
        OesVertexArrayObject = 1,
        One = 1,
        QcomDriverControl = 1,
        QcomExtendedGet = 1,
        QcomExtendedGet2 = 1,
        QcomPerfmonGlobalMode = 1,
        QcomTiledRendering = 1,
        QcomWriteonlyRendering = 1,
        True = 1,
        VersionEsCl10 = 1,
        VersionEsCl11 = 1,
        VersionEsCm10 = 1,
        VersionEsCm11 = 1
    }
}