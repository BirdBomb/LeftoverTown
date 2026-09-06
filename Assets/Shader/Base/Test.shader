Shader "Shader Graphs/Water"
{
    Properties
    {
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        _BaseColor("BaseColor", Color) = (1, 0, 0.7421513, 0)
        _EdgeWaveColor("EdgeWaveColor", Color) = (1, 0, 0, 0)
        _EdgeWaveHighLightColor("EdgeWaveHighLightColor", Color) = (0, 1, 0.03308368, 0)
        _EdgeWaveLength("EdgeWaveLength", Float) = 5
        _EdgeWaveSpeed("EdgeWaveSpeed", Float) = 1
        _EdgeWaveShakeSpeed("EdgeWaveShakeSpeed", Float) = 0.5
        _EdgeWaveHeight("EdgeWaveHeight", Float) = 0.0625
        _EdgeWaveColorNoiseScale("EdgeWaveColorNoiseScale", Float) = 2
        _BottomNoiseStrengh("BottomNoiseStrengh", Float) = 0.1
        _BottomNoiseScale("BottomNoiseScale", Float) = 5
        _BottomScale("BottomScale", Float) = 1
        _BottomStrengh("BottomStrengh", Float) = 1
        _BottomSpeed("BottomSpeed", Float) = 1
        _BottomAngleSpeed("BottomAngleSpeed", Float) = 0
        _BottomAlpha("BottomAlpha", Float) = 0.5
        _BottomPixelCount("BottomPixelCount", Vector) = (960, 640, 0, 0)
        [NoScaleOffset]_CausticBaseTex("CausticBaseTex", 2D) = "white" {}
        _CausticBaseColor("CausticBaseColor", Color) = (0, 0, 0, 0)
        _CausticBaseStrength("CausticBaseStrength", Float) = 0
        _MaskSpeed("MaskSpeed", Vector) = (1, 1, 0, 0)
        _MaskScale("MaskScale", Float) = 1
        _MaskRange("MaskRange", Float) = 0.5
        _CausticScale("CausticScale", Float) = 1
        _CausticSpeed("CausticSpeed", Float) = 1
        _CausitcBlend("CausitcBlend", Float) = 0.5
        _CausticNoiseScale("CausticNoiseScale", Float) = 2
        _MaskStrength("MaskStrength", Float) = 0
        _FloatingSpeed("FloatingSpeed", Float) = 1
        _FloatingStep("FloatingStep", Float) = 0.25
        _FloatingScale("FloatingScale", Float) = 5
        _FloatingStrength("FloatingStrength", Float) = 1
        _FloatingMaskSpeed("FloatingMaskSpeed", Float) = 0
        _FloatingMaskScale("FloatingMaskScale", Float) = 16
        [NoScaleOffset]_DistortTex("DistortTex", 2D) = "white" {}
        _DistortTran("DistortTran", Vector) = (0, 0, 0, 0)
        _DistortColor("DistortColor", Color) = (0, 0, 0, 0)
        _DistortUVStrength("DistortUVStrength", Float) = 0.5
        [NonModifiableTextureData][NoScaleOffset]_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D("Texture2D", 2D) = "white" {}
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteLitSubTarget"
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
             float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float4 screenPosition : INTERP2;
             float3 positionWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.screenPosition.xyzw = input.screenPosition;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.screenPosition = input.screenPosition.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D_TexelSize;
        float4 _MainTex_TexelSize;
        float4 _BaseColor;
        float4 _EdgeWaveColor;
        float _EdgeWaveSpeed;
        float _EdgeWaveLength;
        float _EdgeWaveHeight;
        float _EdgeWaveShakeSpeed;
        float _EdgeWaveColorNoiseScale;
        float4 _EdgeWaveHighLightColor;
        float _BottomSpeed;
        float _BottomNoiseScale;
        float _BottomNoiseStrengh;
        float _BottomAlpha;
        float2 _BottomPixelCount;
        float _BottomScale;
        float _BottomStrengh;
        float _CausticSpeed;
        float _CausitcBlend;
        float _CausticScale;
        float4 _CausticBaseColor;
        float4 _CausticBaseTex_TexelSize;
        float _CausticBaseStrength;
        float _FloatingSpeed;
        float _FloatingScale;
        float _FloatingStep;
        float _FloatingStrength;
        float _FloatingMaskSpeed;
        float _FloatingMaskScale;
        float2 _MaskSpeed;
        float _MaskScale;
        float _MaskRange;
        float4 _DistortTex_TexelSize;
        float _DistortUVStrength;
        float4 _DistortColor;
        float4 _DistortTran;
        float _BottomAngleSpeed;
        float _CausticNoiseScale;
        float _MaskStrength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_CausticBaseTex);
        SAMPLER(sampler_CausticBaseTex);
        TEXTURE2D(_DistortTex);
        SAMPLER(sampler_DistortTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float4(float4 In, out float4 Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A / B;
        }
        
        struct Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float
        {
        };
        
        void SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float(float4 _Input, float _Grid, Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float IN, out float4 New_0)
        {
        float4 _Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4 = _Input;
        float _Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float = _Grid;
        float4 _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4);
        float4 _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4;
        Unity_Floor_float4(_Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4, _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4);
        float4 _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        Unity_Divide_float4(_Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4);
        New_0 = _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
        float2 i = floor(uv);
        float2 f = frac(uv);
        f = f * f * (3.0 - 2.0 * f);
        uv = abs(frac(uv) - 0.5);
        float2 c0 = i + float2(0.0, 0.0);
        float2 c1 = i + float2(1.0, 0.0);
        float2 c2 = i + float2(0.0, 1.0);
        float2 c3 = i + float2(1.0, 1.0);
        float r0; Hash_Tchou_2_1_float(c0, r0);
        float r1; Hash_Tchou_2_1_float(c1, r1);
        float r2; Hash_Tchou_2_1_float(c2, r2);
        float r3; Hash_Tchou_2_1_float(c3, r3);
        float bottomOfGrid = lerp(r0, r1, f.x);
        float topOfGrid = lerp(r2, r3, f.x);
        float t = lerp(bottomOfGrid, topOfGrid, f.y);
        return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
        float freq, amp;
        Out = 0.0f;
        freq = pow(2.0, float(0));
        amp = pow(0.5, float(3-0));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(1));
        amp = pow(0.5, float(3-1));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(2));
        amp = pow(0.5, float(3-2));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        struct Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float
        {
        float3 WorldSpacePosition;
        };
        
        void SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(float4 _DistortTran, UnityTexture2D _DistortTex, float _DistortUVStrength, float4 _DistortColor, Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float IN, out float4 Color_0, out float Distort_1)
        {
        float4 _Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4 = _DistortColor;
        float _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float = 16;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_d759dae6c2594e7d8ec5b45a94779549;
        float4 _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float, _ToGrid_d759dae6c2594e7d8ec5b45a94779549, _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4);
        float _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4.xy), 25, _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float);
        UnityTexture2D _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D = _DistortTex;
        float4 _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4 = _DistortTran;
        float _Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[0];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[1];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[2];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[3];
        float2 _Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676;
        float4 _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4);
        float2 _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float);
        float2 _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2;
        Unity_Subtract_float2((_ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4.xy), _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2);
        float2 _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2, _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2);
        float2 _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2;
        Unity_Add_float2(_Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2, float2(0.5, 0.5), _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2);
        float4 _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.tex, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.samplerstate, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.GetTransformedUV(_Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2) );
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.r;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_G_5_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.g;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_B_6_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.b;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_A_7_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.a;
        float _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float;
        Unity_Multiply_float_float(_SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float);
        float _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float, 5, _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float);
        float _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float;
        Unity_Floor_float(_Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float, _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float);
        float _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float;
        Unity_Divide_float(_Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float, 5, _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float);
        float4 _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4, (_Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float.xxxx), _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4);
        float _Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float = _DistortUVStrength;
        float _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        Unity_Multiply_float_float(_Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float);
        Color_0 = _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Distort_1 = _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
        float x; Hash_Tchou_2_1_float(p, x);
        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
        float2 p = UV * Scale.xy;
        float2 ip = floor(p);
        float2 fp = frac(p);
        float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
        float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
        float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
        float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        struct Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float
        {
        };
        
        void SG_FloatToPolar_d32299cdd298f07439c149bb0b195311_float(float _Value, float _Strength, Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float IN, out float2 OutVector2_1)
        {
        float _Property_d31c4d35f15b4d97be7577a22ceb143f_Out_0_Float = _Value;
        float Constant_a26c58304c444c2dad6d66d54b8cd98d = 3.141593;
        float _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float;
        Unity_Multiply_float_float(Constant_a26c58304c444c2dad6d66d54b8cd98d, 2, _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float);
        float _Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float;
        Unity_Multiply_float_float(_Property_d31c4d35f15b4d97be7577a22ceb143f_Out_0_Float, _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float, _Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float);
        float _Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float;
        Unity_Cosine_float(_Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float, _Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float);
        float _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float;
        Unity_Sine_float(_Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float, _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float);
        float2 _Vector2_ecd6022db98d475db804ef747cd313f1_Out_0_Vector2 = float2(_Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float, _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float);
        float _Property_557076e218104d538d6e410a74d0cd36_Out_0_Float = _Strength;
        float2 _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_ecd6022db98d475db804ef747cd313f1_Out_0_Vector2, (_Property_557076e218104d538d6e410a74d0cd36_Out_0_Float.xx), _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2);
        OutVector2_1 = _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2;
        }
        
        struct Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_UVCaustic_7660615f0780a3a47ab0e632916466ff_float(float _CausticSpeed, float _CausticBlend, float _CausticScale, float _Distort, float _CausticNoiseScale, Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float IN, out float2 New_0)
        {
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_a81b27dd86034a94abb2a30b48b0192c;
        float4 _ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_a81b27dd86034a94abb2a30b48b0192c, _ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4);
        float _Property_ac7db366df844e4990fb5aafebd5921b_Out_0_Float = _CausticScale;
        float4 _Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4;
        Unity_Divide_float4(_ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4, (_Property_ac7db366df844e4990fb5aafebd5921b_Out_0_Float.xxxx), _Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4);
        float _Property_b1507ced237e4e97811b7632476b1641_Out_0_Float = _Distort;
        float4 _Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4;
        Unity_Add_float4(_Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4, (_Property_b1507ced237e4e97811b7632476b1641_Out_0_Float.xxxx), _Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4);
        float _Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float = _CausticSpeed;
        float2 _Vector2_4eedbb6ddec7420db830b4c2c1d5ab5e_Out_0_Vector2 = float2(_Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float, _Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float);
        float2 _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2;
        Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Vector2_4eedbb6ddec7420db830b4c2c1d5ab5e_Out_0_Vector2, _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2);
        float2 _Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4.xy), _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2, _Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2);
        float _Property_1d12d5d6305243e58faec8b8aee91847_Out_0_Float = _CausticNoiseScale;
        float _GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2, _Property_1d12d5d6305243e58faec8b8aee91847_Out_0_Float, _GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float);
        float _Property_d391d04563db4da1bab793dc2c87b6d7_Out_0_Float = _CausticBlend;
        Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1;
        float2 _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2;
        SG_FloatToPolar_d32299cdd298f07439c149bb0b195311_float(_GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float, _Property_d391d04563db4da1bab793dc2c87b6d7_Out_0_Float, _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1, _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2);
        float2 _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2;
        Unity_Add_float2((_Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4.xy), _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2, _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2);
        New_0 = _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Maximum_float(float A, float B, out float Out)
        {
            Out = max(A, B);
        }
        
        struct Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float
        {
        float3 TimeParameters;
        };
        
        void SG_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float(float4 _CausticBaseColor, UnityTexture2D _CasusitcBaseTex, float _CausitcBaseStrength, float2 _MaskSpeed, float _MaskScale, float _MaskRange, float _MaskStrength, float2 _UV, Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float IN, out float OutVector1_1, out float4 New_2)
        {
        float _Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float = _MaskRange;
        float _Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float;
        Unity_Subtract_float(0.5, _Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float, _Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float);
        float _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float;
        Unity_Add_float(_Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float, 0.5, _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float);
        float2 _Property_4bd9fd7272d34a208a97a1bed01732f1_Out_0_Vector2 = _MaskSpeed;
        float2 _Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2;
        Unity_Divide_float2(_Property_4bd9fd7272d34a208a97a1bed01732f1_Out_0_Vector2, float2(10, 10), _Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2);
        float2 _Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2);
        float2 _Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2 = _UV;
        float2 _Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2;
        Unity_Add_float2(_Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2, _Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2, _Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2);
        float _Property_03882e3ee2374621b36aa67eb4b6e6a7_Out_0_Float = _MaskScale;
        float _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float;
        Unity_Divide_float(_Property_03882e3ee2374621b36aa67eb4b6e6a7_Out_0_Float, 16, _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float);
        float _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2, _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float, _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float);
        float _Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float;
        Unity_Smoothstep_float(_Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float, _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float, _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float, _Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float);
        float _Property_606338ee7850446496b7e64baa713052_Out_0_Float = _MaskStrength;
        float _Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float;
        Unity_Multiply_float_float(_Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float, _Property_606338ee7850446496b7e64baa713052_Out_0_Float, _Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float);
        float _Property_4c16863732d749b0b2fe45fbd80ab963_Out_0_Float = _CausitcBaseStrength;
        float4 _Property_353355bab0d040bc9968c896e6ae626c_Out_0_Vector4 = _CausticBaseColor;
        UnityTexture2D _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D = _CasusitcBaseTex;
        float4 _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.tex, _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.samplerstate, _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.GetTransformedUV(_Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2) );
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_R_4_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.r;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_G_5_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.g;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_B_6_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.b;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_A_7_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.a;
        float4 _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_353355bab0d040bc9968c896e6ae626c_Out_0_Vector4, _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4, _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4);
        float4 _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4;
        Unity_Multiply_float4_float4((_Property_4c16863732d749b0b2fe45fbd80ab963_Out_0_Float.xxxx), _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4, _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4);
        float4 _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4;
        Unity_Multiply_float4_float4((_Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float.xxxx), _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4, _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4);
        float _Split_28d235cb3e744c15ad540ea4b55baea8_R_1_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[0];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_G_2_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[1];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_B_3_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[2];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_A_4_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[3];
        float _Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float;
        Unity_Maximum_float(_Split_28d235cb3e744c15ad540ea4b55baea8_R_1_Float, _Split_28d235cb3e744c15ad540ea4b55baea8_G_2_Float, _Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float);
        float _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float;
        Unity_Maximum_float(_Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float, _Split_28d235cb3e744c15ad540ea4b55baea8_B_3_Float, _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float);
        OutVector1_1 = _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float;
        New_2 = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4;
        }
        
        struct Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(float _SpeedR, float _SpeedL, float _PixelUnit, float _Scale, float _Strenght, Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float IN, out float New_0)
        {
        float _Property_a57dea734ae842c2910db02cb2e57e28_Out_0_Float = _PixelUnit;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_8bd8de1107214c50b05661f2893264e1;
        float4 _ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Property_a57dea734ae842c2910db02cb2e57e28_Out_0_Float, _ToGrid_8bd8de1107214c50b05661f2893264e1, _ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4);
        float2 _Vector2_3bc84328f110402bb37f382ab0c588a0_Out_0_Vector2 = float2(1, 0);
        float _Property_62c2aa2262a24fe3b9d690b4515a955e_Out_0_Float = _SpeedR;
        float _Float_59aac4a4aaf241ec8ad201b3507f57b5_Out_0_Float = _Property_62c2aa2262a24fe3b9d690b4515a955e_Out_0_Float;
        float2 _Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_3bc84328f110402bb37f382ab0c588a0_Out_0_Vector2, (_Float_59aac4a4aaf241ec8ad201b3507f57b5_Out_0_Float.xx), _Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2);
        float2 _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2);
        float2 _Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4.xy), _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2, _Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2);
        float _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float = _Scale;
        float _GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2, _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float, _GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float);
        float2 _Vector2_b3e9026a96064661915e65401c5ef998_Out_0_Vector2 = float2(-1, 0);
        float _Property_5db7cfbaab2847daa7d8b4b583b68d89_Out_0_Float = _SpeedL;
        float _Float_7ee06057e8e549cf8720690a5c06f38a_Out_0_Float = _Property_5db7cfbaab2847daa7d8b4b583b68d89_Out_0_Float;
        float2 _Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_b3e9026a96064661915e65401c5ef998_Out_0_Vector2, (_Float_7ee06057e8e549cf8720690a5c06f38a_Out_0_Float.xx), _Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2);
        float2 _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2);
        float2 _Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4.xy), _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2, _Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2);
        float _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2, _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float, _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float);
        float _Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float;
        Unity_Multiply_float_float(_GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float, _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float, _Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float);
        float _Property_c3cd5c2888684348bdb0dcb89b2d3a55_Out_0_Float = _Strenght;
        float _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float, _Property_c3cd5c2888684348bdb0dcb89b2d3a55_Out_0_Float, _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float);
        New_0 = _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float;
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        struct Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float(float _FloatingMaskScale, float _FloatingSpeed, float _FloatingScale, float _FloatingStep, float _FloatingStrength, float _AreaAlpha, Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float IN, out float New_0)
        {
        float _Property_768d4c11e1d74c038c611c29a7fe7bee_Out_0_Float = _FloatingStep;
        float _Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float = _FloatingSpeed;
        float _Property_12f8969d6c634c5dbece900f136fc702_Out_0_Float = _FloatingScale;
        Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1;
        _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1.WorldSpacePosition = IN.WorldSpacePosition;
        _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1.TimeParameters = IN.TimeParameters;
        float _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float;
        SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(_Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float, _Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float, 16, _Property_12f8969d6c634c5dbece900f136fc702_Out_0_Float, 2, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float);
        float _Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float;
        Unity_Step_float(_Property_768d4c11e1d74c038c611c29a7fe7bee_Out_0_Float, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float, _Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float);
        float _Property_10ae58a5122440539849cc655abb1e9e_Out_0_Float = _AreaAlpha;
        float _Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float;
        Unity_Multiply_float_float(_Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float, _Property_10ae58a5122440539849cc655abb1e9e_Out_0_Float, _Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0;
        float4 _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0, _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4);
        float _Property_425de77259b74a9088e05b137bbe6009_Out_0_Float = _FloatingSpeed;
        float _Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_425de77259b74a9088e05b137bbe6009_Out_0_Float, _Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float);
        float4 _Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4;
        Unity_Add_float4(_ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4, (_Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float.xxxx), _Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4);
        float _Property_348d99c06d5346dcaf5fc7679b3d10ce_Out_0_Float = _FloatingMaskScale;
        float _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4.xy), _Property_348d99c06d5346dcaf5fc7679b3d10ce_Out_0_Float, _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float);
        float _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float;
        Unity_Step_float(0.4, _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float, _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float);
        float _Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float, _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float, _Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float);
        float _Property_55168c628bf74e3b87cd33712139ad59_Out_0_Float = _FloatingStrength;
        float _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float, _Property_55168c628bf74e3b87cd33712139ad59_Out_0_Float, _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float);
        New_0 = _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
        {
        Hash_Tchou_2_2_float(UV, UV);
        return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);
        for (int y = -1; y <= 1; y++)
        {
        for (int x = -1; x <= 1; x++)
        {
        float2 lattice = float2(x, y);
        float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
        float d = distance(lattice + offset, f);
        if (d < res.x)
        {
        res = float3(d, offset.x, offset.y);
        Out = res.x;
        Cells = res.y;
        }
        }
        }
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        struct Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float
        {
        float3 WorldSpacePosition;
        float2 NDCPosition;
        float3 TimeParameters;
        };
        
        void SG_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float(float _LiquidBottomSpeed, float _LiquidBottomAngleSpeed, float _LiquidBottomNoiseScale, float _LiquidBottomNoiseStrengh, float2 _PixelCell, float _Scale, float _Distort, Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float IN, out float4 OutVector4_1)
        {
        float4 _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
        float _Split_babc1f5e31a04264ac52c77e08f160df_R_1_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[0];
        float _Split_babc1f5e31a04264ac52c77e08f160df_G_2_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[1];
        float _Split_babc1f5e31a04264ac52c77e08f160df_B_3_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[2];
        float _Split_babc1f5e31a04264ac52c77e08f160df_A_4_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[3];
        float2 _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2 = _PixelCell;
        float _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float = _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2[0];
        float _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float = _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2[1];
        float _Split_21288f26c09f404a9991b72c6c6549e5_B_3_Float = 0;
        float _Split_21288f26c09f404a9991b72c6c6549e5_A_4_Float = 0;
        float _Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float;
        Unity_Multiply_float_float(_Split_babc1f5e31a04264ac52c77e08f160df_R_1_Float, _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float, _Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float);
        float _Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float;
        Unity_Divide_float(_Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float, _Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float);
        float _Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float;
        Unity_Multiply_float_float(_Split_babc1f5e31a04264ac52c77e08f160df_G_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float, _Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float);
        float _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float;
        Unity_Divide_float(_Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float, _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float);
        float4 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGBA_4_Vector4;
        float3 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGB_5_Vector3;
        float2 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2;
        Unity_Combine_float(_Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float, _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float, 0, 0, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGBA_4_Vector4, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGB_5_Vector3, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2);
        float _Property_a65d4b108b87421787112feacdc2ad90_Out_0_Float = _LiquidBottomSpeed;
        float _Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_a65d4b108b87421787112feacdc2ad90_Out_0_Float, _Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_025acfae81ee4923b89c42d5e95e3073;
        float4 _ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_025acfae81ee4923b89c42d5e95e3073, _ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4);
        float _Property_632061910c294796a237dd943f583f7a_Out_0_Float = _Distort;
        float4 _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4;
        Unity_Add_float4(_ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4, (_Property_632061910c294796a237dd943f583f7a_Out_0_Float.xxxx), _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4);
        float4 _Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4;
        Unity_Add_float4((_Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float.xxxx), _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4, _Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4);
        float _Property_cd3a650be5e74143bdb6ef87564a6459_Out_0_Float = _LiquidBottomAngleSpeed;
        float _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float;
        Unity_Multiply_float_float(_Property_cd3a650be5e74143bdb6ef87564a6459_Out_0_Float, IN.TimeParameters.x, _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float);
        float _Property_dd1d1cc3b97841399706968720371ee0_Out_0_Float = _LiquidBottomNoiseScale;
        float _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float;
        float _Voronoi_56b538e009a446cd850ace505427cf1b_Cells_4_Float;
        Unity_Voronoi_Deterministic_float((_Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4.xy), _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float, _Property_dd1d1cc3b97841399706968720371ee0_Out_0_Float, _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float, _Voronoi_56b538e009a446cd850ace505427cf1b_Cells_4_Float);
        float _Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float;
        Unity_Step_float(0.5, _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float, _Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float);
        float _Property_953201261ff4463b82ab45ac766060fa_Out_0_Float = _LiquidBottomNoiseStrengh;
        float _Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float;
        Unity_Multiply_float_float(_Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float, _Property_953201261ff4463b82ab45ac766060fa_Out_0_Float, _Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float);
        float2 _TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2;
        Unity_TilingAndOffset_float(_Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2, float2 (1, 1), (_Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float.xx), _TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2);
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).tex, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).samplerstate, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).GetTransformedUV(_TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2) );
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_R_4_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.r;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_G_5_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.g;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_B_6_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.b;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_A_7_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.a;
        OutVector4_1 = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4;
        }
        
        struct Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float
        {
        half4 uv0;
        };
        
        void SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(UnityTexture2D _MainTex, Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D = _MainTex;
        float4 _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.tex, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.samplerstate, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_R_4_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.r;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_G_5_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.g;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.b;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.a;
        float _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float, _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float, _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float);
        OutVector1_1 = _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        struct Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float
        {
        float3 WorldSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(float4 _UV, float _WaveSpeed, float _WaveLength, float _WaveHeight, float _WaveShakeSpeed, float _WaveDistort, Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float IN, out float2 OutVector2_1)
        {
        float4 _UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4 = IN.uv0;
        float _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float = _WaveDistort;
        float _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float;
        Unity_Step_float(0.2, _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float, _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float);
        float _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float;
        Unity_OneMinus_float(_Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float, _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float);
        float _Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float = _WaveSpeed;
        float _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float;
        Unity_Multiply_float_float(_Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float, IN.TimeParameters.x, _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_ece7a048a124490eb96c1c943b36de7e;
        float4 _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_ece7a048a124490eb96c1c943b36de7e, _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4);
        float _Split_ae769ff284874f9a8245510995a1f39d_R_1_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[0];
        float _Split_ae769ff284874f9a8245510995a1f39d_G_2_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[1];
        float _Split_ae769ff284874f9a8245510995a1f39d_B_3_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[2];
        float _Split_ae769ff284874f9a8245510995a1f39d_A_4_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[3];
        float _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float = _WaveLength;
        float _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float;
        Unity_Multiply_float_float(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float);
        float _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float;
        Unity_Add_float(_Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float, _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float);
        float _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float;
        Unity_Sine_float(_Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float, _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float);
        float _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float = _WaveShakeSpeed;
        float _Multiply_70a55d008a294763835173cb84629169_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float2 _Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2 = float2(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float(_Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2, 10, _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float);
        float _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float;
        Unity_Subtract_float(_SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float, 0.5, _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float);
        float _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float;
        Unity_Multiply_float_float(_Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float, 3, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float);
        float _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float;
        Unity_Step_float(_Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float);
        float _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float;
        Unity_Minimum_float(_OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float, _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float);
        float _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float = _WaveHeight;
        float _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float;
        Unity_Multiply_float_float(_Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float, _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float);
        float4 _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4;
        float3 _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3;
        float2 _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2;
        Unity_Combine_float(0, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float, 0, 0, _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4, _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3, _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2);
        float2 _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        Unity_Add_float2((_UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4.xy), _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2, _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2);
        OutVector2_1 = _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        }
        
        struct Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float
        {
        };
        
        void SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(UnityTexture2D _MainTex, float4 _UV, Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D = _MainTex;
        float4 _Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4 = _UV;
        float4 _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.tex, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.samplerstate, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.GetTransformedUV((_Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4.xy)) );
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_R_4_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.r;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.g;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_B_6_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.b;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.a;
        float _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float, _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float, _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float);
        OutVector1_1 = _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float4 SpriteMask;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4 = _DistortTran;
            UnityTexture2D _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_DistortTex);
            float _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float = _DistortUVStrength;
            float4 _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4 = _DistortColor;
            Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float _WaterDistort_535e0977d51441b3927c7ec02959bc97;
            _WaterDistort_535e0977d51441b3927c7ec02959bc97.WorldSpacePosition = IN.WorldSpacePosition;
            float4 _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4;
            float _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float;
            SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(_Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4, _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D, _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float, _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float);
            float4 _Property_c07fc1928f0146aab38ffc698890428e_Out_0_Vector4 = _CausticBaseColor;
            UnityTexture2D _Property_23c78aa0988448d193906bb14bcbd874_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_CausticBaseTex);
            float _Property_0919388d90484df7b47156bdff8991e4_Out_0_Float = _CausticBaseStrength;
            float2 _Property_3df02cd1b2b54257a5f27cbedaa58dc5_Out_0_Vector2 = _MaskSpeed;
            float _Property_ed5a798690254c86b56f751c75170330_Out_0_Float = _MaskScale;
            float _Property_0c3340ffe9b94bc882f54dbc156c47d0_Out_0_Float = _MaskRange;
            float _Property_ce5b21ef3eaf4b5bb7954300c744b09e_Out_0_Float = _MaskStrength;
            float _Property_40b8633fc13d4ee997a5c69df3d82f87_Out_0_Float = _CausticSpeed;
            float _Property_5e6fecce44e247f5a33ac030c2ad9e21_Out_0_Float = _CausitcBlend;
            float _Property_d5308f1b3b514b8da3799eb0f61a694d_Out_0_Float = _CausticScale;
            float _Property_5c2585358f944caa8d34cf98b5694651_Out_0_Float = _CausticNoiseScale;
            Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb;
            _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb.WorldSpacePosition = IN.WorldSpacePosition;
            _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb.TimeParameters = IN.TimeParameters;
            float2 _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2;
            SG_UVCaustic_7660615f0780a3a47ab0e632916466ff_float(_Property_40b8633fc13d4ee997a5c69df3d82f87_Out_0_Float, _Property_5e6fecce44e247f5a33ac030c2ad9e21_Out_0_Float, _Property_d5308f1b3b514b8da3799eb0f61a694d_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _Property_5c2585358f944caa8d34cf98b5694651_Out_0_Float, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2);
            Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2;
            _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2.TimeParameters = IN.TimeParameters;
            float _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float;
            float4 _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4;
            SG_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float(_Property_c07fc1928f0146aab38ffc698890428e_Out_0_Vector4, _Property_23c78aa0988448d193906bb14bcbd874_Out_0_Texture2D, _Property_0919388d90484df7b47156bdff8991e4_Out_0_Float, _Property_3df02cd1b2b54257a5f27cbedaa58dc5_Out_0_Vector2, _Property_ed5a798690254c86b56f751c75170330_Out_0_Float, _Property_0c3340ffe9b94bc882f54dbc156c47d0_Out_0_Float, _Property_ce5b21ef3eaf4b5bb7954300c744b09e_Out_0_Float, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4);
            float _Property_2470e4696d544eb9bdbd493f89e33be9_Out_0_Float = _FloatingMaskScale;
            float _Property_edc40ab4725a443989cbdc4861756e9b_Out_0_Float = _FloatingSpeed;
            float _Property_27f3ddc40e54460ca0d0c2983d883803_Out_0_Float = _FloatingScale;
            float _Property_f76b3b80b7dd403ca709d91f624d7dcd_Out_0_Float = _FloatingStep;
            float _Property_219c9e59f3f24b30a2c77917c412b0b6_Out_0_Float = _FloatingStrength;
            Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float _WaterFloating_12c80082bf2c4682a4f593c5690670d5;
            _WaterFloating_12c80082bf2c4682a4f593c5690670d5.WorldSpacePosition = IN.WorldSpacePosition;
            _WaterFloating_12c80082bf2c4682a4f593c5690670d5.TimeParameters = IN.TimeParameters;
            float _WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float;
            SG_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float(_Property_2470e4696d544eb9bdbd493f89e33be9_Out_0_Float, _Property_edc40ab4725a443989cbdc4861756e9b_Out_0_Float, _Property_27f3ddc40e54460ca0d0c2983d883803_Out_0_Float, _Property_f76b3b80b7dd403ca709d91f624d7dcd_Out_0_Float, _Property_219c9e59f3f24b30a2c77917c412b0b6_Out_0_Float, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float, _WaterFloating_12c80082bf2c4682a4f593c5690670d5, _WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float);
            float4 _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4;
            Unity_Add_float4(_WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4, (_WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float.xxxx), _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4);
            float4 _Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4;
            Unity_Add_float4(_WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4, _Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4);
            float _Property_16a4ee35313f48afacbb28383a2d74ca_Out_0_Float = _BottomSpeed;
            float _Property_bfa36bdf8a3942399424d5b51d83dd06_Out_0_Float = _BottomAngleSpeed;
            float _Property_1a75e1efb86c457087c5f5bd2bab3e11_Out_0_Float = _BottomNoiseScale;
            float _Property_e2d3edae3a34460784d651b854f86f5e_Out_0_Float = _BottomNoiseStrengh;
            float2 _Property_801c129985d747aba3e7481ad87dd0b8_Out_0_Vector2 = _BottomPixelCount;
            float _Property_9db9f5b047a64b2483dd9c946159309b_Out_0_Float = _BottomScale;
            Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float _CatchLiquidBottom_f94162af491145679d1c11914ad36b53;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.WorldSpacePosition = IN.WorldSpacePosition;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.NDCPosition = IN.NDCPosition;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.TimeParameters = IN.TimeParameters;
            float4 _CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4;
            SG_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float(_Property_16a4ee35313f48afacbb28383a2d74ca_Out_0_Float, _Property_bfa36bdf8a3942399424d5b51d83dd06_Out_0_Float, _Property_1a75e1efb86c457087c5f5bd2bab3e11_Out_0_Float, _Property_e2d3edae3a34460784d651b854f86f5e_Out_0_Float, _Property_801c129985d747aba3e7481ad87dd0b8_Out_0_Vector2, _Property_9db9f5b047a64b2483dd9c946159309b_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _CatchLiquidBottom_f94162af491145679d1c11914ad36b53, _CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4);
            float _Property_70d5267ba184430a8fed1cd11db83493_Out_0_Float = _BottomStrengh;
            float4 _Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4;
            Unity_Multiply_float4_float4(_CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4, (_Property_70d5267ba184430a8fed1cd11db83493_Out_0_Float.xxxx), _Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4);
            UnityTexture2D _Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3;
            _RGBCutB_e130f7613eb140988bc99e39b3f57fb3.uv0 = IN.uv0;
            float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float;
            SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(_Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float);
            UnityTexture2D _Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4 = IN.uv0;
            float _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float = _EdgeWaveSpeed;
            float _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float = _EdgeWaveLength;
            float _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float = _EdgeWaveHeight;
            float _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float = _EdgeWaveShakeSpeed;
            Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.WorldSpacePosition = IN.WorldSpacePosition;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.uv0 = IN.uv0;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.TimeParameters = IN.TimeParameters;
            float2 _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2;
            SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(_UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4, _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float, _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float, _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float, _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2);
            Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852;
            float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float;
            SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(_Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D, (float4(_UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2, 0.0, 1.0)), _RGBCutG_62abc9b5809b4b07a9fae459f7e96852, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float);
            float _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float;
            Unity_Step_float(0.1, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float);
            float _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float;
            Unity_Subtract_float(_RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float, _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float);
            float _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float;
            Unity_Clamp_float(_Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float, 0, 1, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float);
            float4 _Property_e7e806c9a8a64fd59d0a70cd6598a43b_Out_0_Vector4 = _BaseColor;
            float4 _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float.xxxx), _Property_e7e806c9a8a64fd59d0a70cd6598a43b_Out_0_Vector4, _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4);
            float _Property_d3385627294b41949fa94f7d49df2d80_Out_0_Float = _BottomAlpha;
            float4 _Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4;
            Unity_Lerp_float4(_Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4, _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4, (_Property_d3385627294b41949fa94f7d49df2d80_Out_0_Float.xxxx), _Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4);
            float4 _Property_5156b57acc9f49efa11c95a30950d9a2_Out_0_Vector4 = _EdgeWaveColor;
            float4 _Property_81b01f5ed69941dd8601ee9432af1a5f_Out_0_Vector4 = _EdgeWaveHighLightColor;
            float _Property_7a60d176b9994138bfe5bd4b0415dade_Out_0_Float = _EdgeWaveColorNoiseScale;
            Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7;
            _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7.WorldSpacePosition = IN.WorldSpacePosition;
            _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7.TimeParameters = IN.TimeParameters;
            float _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float;
            SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(1, 1, 16, _Property_7a60d176b9994138bfe5bd4b0415dade_Out_0_Float, 0.5, _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7, _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float);
            float _Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float;
            Unity_Multiply_float_float(_EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float, 10, _Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float);
            float _Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float;
            Unity_Floor_float(_Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float, _Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float);
            float _Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float;
            Unity_Divide_float(_Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float, 5, _Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float);
            float4 _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_81b01f5ed69941dd8601ee9432af1a5f_Out_0_Vector4, (_Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float.xxxx), _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4);
            float4 _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4;
            Unity_Add_float4(_Property_5156b57acc9f49efa11c95a30950d9a2_Out_0_Vector4, _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4, _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4);
            float4 _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4;
            Unity_Multiply_float4_float4((_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float.xxxx), _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4, _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4);
            float4 _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4;
            Unity_Add_float4(_Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4, _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4, _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4);
            float4 _Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4;
            Unity_Add_float4(_Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4, _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4, _Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4);
            float _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            Unity_Add_float(_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float, _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float);
            surface.BaseColor = (_Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4.xyz);
            surface.Alpha = _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Sprite Normal"
            Tags
            {
                "LightMode" = "NormalsRendering"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITENORMAL
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float3 positionWS : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D_TexelSize;
        float4 _MainTex_TexelSize;
        float4 _BaseColor;
        float4 _EdgeWaveColor;
        float _EdgeWaveSpeed;
        float _EdgeWaveLength;
        float _EdgeWaveHeight;
        float _EdgeWaveShakeSpeed;
        float _EdgeWaveColorNoiseScale;
        float4 _EdgeWaveHighLightColor;
        float _BottomSpeed;
        float _BottomNoiseScale;
        float _BottomNoiseStrengh;
        float _BottomAlpha;
        float2 _BottomPixelCount;
        float _BottomScale;
        float _BottomStrengh;
        float _CausticSpeed;
        float _CausitcBlend;
        float _CausticScale;
        float4 _CausticBaseColor;
        float4 _CausticBaseTex_TexelSize;
        float _CausticBaseStrength;
        float _FloatingSpeed;
        float _FloatingScale;
        float _FloatingStep;
        float _FloatingStrength;
        float _FloatingMaskSpeed;
        float _FloatingMaskScale;
        float2 _MaskSpeed;
        float _MaskScale;
        float _MaskRange;
        float4 _DistortTex_TexelSize;
        float _DistortUVStrength;
        float4 _DistortColor;
        float4 _DistortTran;
        float _BottomAngleSpeed;
        float _CausticNoiseScale;
        float _MaskStrength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_CausticBaseTex);
        SAMPLER(sampler_CausticBaseTex);
        TEXTURE2D(_DistortTex);
        SAMPLER(sampler_DistortTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float4(float4 In, out float4 Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A / B;
        }
        
        struct Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float
        {
        };
        
        void SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float(float4 _Input, float _Grid, Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float IN, out float4 New_0)
        {
        float4 _Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4 = _Input;
        float _Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float = _Grid;
        float4 _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4);
        float4 _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4;
        Unity_Floor_float4(_Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4, _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4);
        float4 _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        Unity_Divide_float4(_Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4);
        New_0 = _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
        float2 i = floor(uv);
        float2 f = frac(uv);
        f = f * f * (3.0 - 2.0 * f);
        uv = abs(frac(uv) - 0.5);
        float2 c0 = i + float2(0.0, 0.0);
        float2 c1 = i + float2(1.0, 0.0);
        float2 c2 = i + float2(0.0, 1.0);
        float2 c3 = i + float2(1.0, 1.0);
        float r0; Hash_Tchou_2_1_float(c0, r0);
        float r1; Hash_Tchou_2_1_float(c1, r1);
        float r2; Hash_Tchou_2_1_float(c2, r2);
        float r3; Hash_Tchou_2_1_float(c3, r3);
        float bottomOfGrid = lerp(r0, r1, f.x);
        float topOfGrid = lerp(r2, r3, f.x);
        float t = lerp(bottomOfGrid, topOfGrid, f.y);
        return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
        float freq, amp;
        Out = 0.0f;
        freq = pow(2.0, float(0));
        amp = pow(0.5, float(3-0));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(1));
        amp = pow(0.5, float(3-1));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(2));
        amp = pow(0.5, float(3-2));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        struct Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float
        {
        float3 WorldSpacePosition;
        };
        
        void SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(float4 _DistortTran, UnityTexture2D _DistortTex, float _DistortUVStrength, float4 _DistortColor, Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float IN, out float4 Color_0, out float Distort_1)
        {
        float4 _Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4 = _DistortColor;
        float _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float = 16;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_d759dae6c2594e7d8ec5b45a94779549;
        float4 _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float, _ToGrid_d759dae6c2594e7d8ec5b45a94779549, _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4);
        float _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4.xy), 25, _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float);
        UnityTexture2D _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D = _DistortTex;
        float4 _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4 = _DistortTran;
        float _Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[0];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[1];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[2];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[3];
        float2 _Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676;
        float4 _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4);
        float2 _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float);
        float2 _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2;
        Unity_Subtract_float2((_ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4.xy), _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2);
        float2 _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2, _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2);
        float2 _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2;
        Unity_Add_float2(_Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2, float2(0.5, 0.5), _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2);
        float4 _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.tex, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.samplerstate, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.GetTransformedUV(_Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2) );
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.r;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_G_5_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.g;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_B_6_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.b;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_A_7_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.a;
        float _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float;
        Unity_Multiply_float_float(_SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float);
        float _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float, 5, _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float);
        float _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float;
        Unity_Floor_float(_Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float, _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float);
        float _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float;
        Unity_Divide_float(_Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float, 5, _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float);
        float4 _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4, (_Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float.xxxx), _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4);
        float _Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float = _DistortUVStrength;
        float _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        Unity_Multiply_float_float(_Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float);
        Color_0 = _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Distort_1 = _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
        float x; Hash_Tchou_2_1_float(p, x);
        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
        float2 p = UV * Scale.xy;
        float2 ip = floor(p);
        float2 fp = frac(p);
        float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
        float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
        float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
        float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        struct Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float
        {
        };
        
        void SG_FloatToPolar_d32299cdd298f07439c149bb0b195311_float(float _Value, float _Strength, Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float IN, out float2 OutVector2_1)
        {
        float _Property_d31c4d35f15b4d97be7577a22ceb143f_Out_0_Float = _Value;
        float Constant_a26c58304c444c2dad6d66d54b8cd98d = 3.141593;
        float _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float;
        Unity_Multiply_float_float(Constant_a26c58304c444c2dad6d66d54b8cd98d, 2, _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float);
        float _Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float;
        Unity_Multiply_float_float(_Property_d31c4d35f15b4d97be7577a22ceb143f_Out_0_Float, _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float, _Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float);
        float _Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float;
        Unity_Cosine_float(_Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float, _Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float);
        float _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float;
        Unity_Sine_float(_Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float, _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float);
        float2 _Vector2_ecd6022db98d475db804ef747cd313f1_Out_0_Vector2 = float2(_Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float, _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float);
        float _Property_557076e218104d538d6e410a74d0cd36_Out_0_Float = _Strength;
        float2 _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_ecd6022db98d475db804ef747cd313f1_Out_0_Vector2, (_Property_557076e218104d538d6e410a74d0cd36_Out_0_Float.xx), _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2);
        OutVector2_1 = _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2;
        }
        
        struct Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_UVCaustic_7660615f0780a3a47ab0e632916466ff_float(float _CausticSpeed, float _CausticBlend, float _CausticScale, float _Distort, float _CausticNoiseScale, Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float IN, out float2 New_0)
        {
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_a81b27dd86034a94abb2a30b48b0192c;
        float4 _ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_a81b27dd86034a94abb2a30b48b0192c, _ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4);
        float _Property_ac7db366df844e4990fb5aafebd5921b_Out_0_Float = _CausticScale;
        float4 _Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4;
        Unity_Divide_float4(_ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4, (_Property_ac7db366df844e4990fb5aafebd5921b_Out_0_Float.xxxx), _Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4);
        float _Property_b1507ced237e4e97811b7632476b1641_Out_0_Float = _Distort;
        float4 _Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4;
        Unity_Add_float4(_Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4, (_Property_b1507ced237e4e97811b7632476b1641_Out_0_Float.xxxx), _Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4);
        float _Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float = _CausticSpeed;
        float2 _Vector2_4eedbb6ddec7420db830b4c2c1d5ab5e_Out_0_Vector2 = float2(_Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float, _Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float);
        float2 _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2;
        Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Vector2_4eedbb6ddec7420db830b4c2c1d5ab5e_Out_0_Vector2, _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2);
        float2 _Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4.xy), _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2, _Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2);
        float _Property_1d12d5d6305243e58faec8b8aee91847_Out_0_Float = _CausticNoiseScale;
        float _GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2, _Property_1d12d5d6305243e58faec8b8aee91847_Out_0_Float, _GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float);
        float _Property_d391d04563db4da1bab793dc2c87b6d7_Out_0_Float = _CausticBlend;
        Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1;
        float2 _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2;
        SG_FloatToPolar_d32299cdd298f07439c149bb0b195311_float(_GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float, _Property_d391d04563db4da1bab793dc2c87b6d7_Out_0_Float, _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1, _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2);
        float2 _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2;
        Unity_Add_float2((_Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4.xy), _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2, _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2);
        New_0 = _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Maximum_float(float A, float B, out float Out)
        {
            Out = max(A, B);
        }
        
        struct Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float
        {
        float3 TimeParameters;
        };
        
        void SG_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float(float4 _CausticBaseColor, UnityTexture2D _CasusitcBaseTex, float _CausitcBaseStrength, float2 _MaskSpeed, float _MaskScale, float _MaskRange, float _MaskStrength, float2 _UV, Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float IN, out float OutVector1_1, out float4 New_2)
        {
        float _Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float = _MaskRange;
        float _Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float;
        Unity_Subtract_float(0.5, _Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float, _Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float);
        float _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float;
        Unity_Add_float(_Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float, 0.5, _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float);
        float2 _Property_4bd9fd7272d34a208a97a1bed01732f1_Out_0_Vector2 = _MaskSpeed;
        float2 _Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2;
        Unity_Divide_float2(_Property_4bd9fd7272d34a208a97a1bed01732f1_Out_0_Vector2, float2(10, 10), _Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2);
        float2 _Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2);
        float2 _Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2 = _UV;
        float2 _Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2;
        Unity_Add_float2(_Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2, _Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2, _Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2);
        float _Property_03882e3ee2374621b36aa67eb4b6e6a7_Out_0_Float = _MaskScale;
        float _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float;
        Unity_Divide_float(_Property_03882e3ee2374621b36aa67eb4b6e6a7_Out_0_Float, 16, _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float);
        float _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2, _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float, _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float);
        float _Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float;
        Unity_Smoothstep_float(_Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float, _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float, _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float, _Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float);
        float _Property_606338ee7850446496b7e64baa713052_Out_0_Float = _MaskStrength;
        float _Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float;
        Unity_Multiply_float_float(_Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float, _Property_606338ee7850446496b7e64baa713052_Out_0_Float, _Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float);
        float _Property_4c16863732d749b0b2fe45fbd80ab963_Out_0_Float = _CausitcBaseStrength;
        float4 _Property_353355bab0d040bc9968c896e6ae626c_Out_0_Vector4 = _CausticBaseColor;
        UnityTexture2D _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D = _CasusitcBaseTex;
        float4 _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.tex, _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.samplerstate, _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.GetTransformedUV(_Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2) );
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_R_4_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.r;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_G_5_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.g;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_B_6_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.b;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_A_7_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.a;
        float4 _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_353355bab0d040bc9968c896e6ae626c_Out_0_Vector4, _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4, _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4);
        float4 _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4;
        Unity_Multiply_float4_float4((_Property_4c16863732d749b0b2fe45fbd80ab963_Out_0_Float.xxxx), _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4, _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4);
        float4 _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4;
        Unity_Multiply_float4_float4((_Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float.xxxx), _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4, _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4);
        float _Split_28d235cb3e744c15ad540ea4b55baea8_R_1_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[0];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_G_2_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[1];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_B_3_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[2];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_A_4_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[3];
        float _Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float;
        Unity_Maximum_float(_Split_28d235cb3e744c15ad540ea4b55baea8_R_1_Float, _Split_28d235cb3e744c15ad540ea4b55baea8_G_2_Float, _Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float);
        float _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float;
        Unity_Maximum_float(_Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float, _Split_28d235cb3e744c15ad540ea4b55baea8_B_3_Float, _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float);
        OutVector1_1 = _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float;
        New_2 = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4;
        }
        
        struct Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(float _SpeedR, float _SpeedL, float _PixelUnit, float _Scale, float _Strenght, Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float IN, out float New_0)
        {
        float _Property_a57dea734ae842c2910db02cb2e57e28_Out_0_Float = _PixelUnit;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_8bd8de1107214c50b05661f2893264e1;
        float4 _ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Property_a57dea734ae842c2910db02cb2e57e28_Out_0_Float, _ToGrid_8bd8de1107214c50b05661f2893264e1, _ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4);
        float2 _Vector2_3bc84328f110402bb37f382ab0c588a0_Out_0_Vector2 = float2(1, 0);
        float _Property_62c2aa2262a24fe3b9d690b4515a955e_Out_0_Float = _SpeedR;
        float _Float_59aac4a4aaf241ec8ad201b3507f57b5_Out_0_Float = _Property_62c2aa2262a24fe3b9d690b4515a955e_Out_0_Float;
        float2 _Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_3bc84328f110402bb37f382ab0c588a0_Out_0_Vector2, (_Float_59aac4a4aaf241ec8ad201b3507f57b5_Out_0_Float.xx), _Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2);
        float2 _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2);
        float2 _Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4.xy), _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2, _Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2);
        float _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float = _Scale;
        float _GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2, _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float, _GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float);
        float2 _Vector2_b3e9026a96064661915e65401c5ef998_Out_0_Vector2 = float2(-1, 0);
        float _Property_5db7cfbaab2847daa7d8b4b583b68d89_Out_0_Float = _SpeedL;
        float _Float_7ee06057e8e549cf8720690a5c06f38a_Out_0_Float = _Property_5db7cfbaab2847daa7d8b4b583b68d89_Out_0_Float;
        float2 _Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_b3e9026a96064661915e65401c5ef998_Out_0_Vector2, (_Float_7ee06057e8e549cf8720690a5c06f38a_Out_0_Float.xx), _Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2);
        float2 _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2);
        float2 _Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4.xy), _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2, _Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2);
        float _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2, _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float, _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float);
        float _Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float;
        Unity_Multiply_float_float(_GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float, _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float, _Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float);
        float _Property_c3cd5c2888684348bdb0dcb89b2d3a55_Out_0_Float = _Strenght;
        float _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float, _Property_c3cd5c2888684348bdb0dcb89b2d3a55_Out_0_Float, _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float);
        New_0 = _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float;
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        struct Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float(float _FloatingMaskScale, float _FloatingSpeed, float _FloatingScale, float _FloatingStep, float _FloatingStrength, float _AreaAlpha, Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float IN, out float New_0)
        {
        float _Property_768d4c11e1d74c038c611c29a7fe7bee_Out_0_Float = _FloatingStep;
        float _Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float = _FloatingSpeed;
        float _Property_12f8969d6c634c5dbece900f136fc702_Out_0_Float = _FloatingScale;
        Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1;
        _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1.WorldSpacePosition = IN.WorldSpacePosition;
        _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1.TimeParameters = IN.TimeParameters;
        float _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float;
        SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(_Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float, _Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float, 16, _Property_12f8969d6c634c5dbece900f136fc702_Out_0_Float, 2, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float);
        float _Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float;
        Unity_Step_float(_Property_768d4c11e1d74c038c611c29a7fe7bee_Out_0_Float, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float, _Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float);
        float _Property_10ae58a5122440539849cc655abb1e9e_Out_0_Float = _AreaAlpha;
        float _Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float;
        Unity_Multiply_float_float(_Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float, _Property_10ae58a5122440539849cc655abb1e9e_Out_0_Float, _Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0;
        float4 _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0, _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4);
        float _Property_425de77259b74a9088e05b137bbe6009_Out_0_Float = _FloatingSpeed;
        float _Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_425de77259b74a9088e05b137bbe6009_Out_0_Float, _Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float);
        float4 _Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4;
        Unity_Add_float4(_ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4, (_Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float.xxxx), _Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4);
        float _Property_348d99c06d5346dcaf5fc7679b3d10ce_Out_0_Float = _FloatingMaskScale;
        float _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4.xy), _Property_348d99c06d5346dcaf5fc7679b3d10ce_Out_0_Float, _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float);
        float _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float;
        Unity_Step_float(0.4, _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float, _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float);
        float _Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float, _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float, _Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float);
        float _Property_55168c628bf74e3b87cd33712139ad59_Out_0_Float = _FloatingStrength;
        float _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float, _Property_55168c628bf74e3b87cd33712139ad59_Out_0_Float, _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float);
        New_0 = _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
        {
        Hash_Tchou_2_2_float(UV, UV);
        return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);
        for (int y = -1; y <= 1; y++)
        {
        for (int x = -1; x <= 1; x++)
        {
        float2 lattice = float2(x, y);
        float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
        float d = distance(lattice + offset, f);
        if (d < res.x)
        {
        res = float3(d, offset.x, offset.y);
        Out = res.x;
        Cells = res.y;
        }
        }
        }
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        struct Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float
        {
        float3 WorldSpacePosition;
        float2 NDCPosition;
        float3 TimeParameters;
        };
        
        void SG_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float(float _LiquidBottomSpeed, float _LiquidBottomAngleSpeed, float _LiquidBottomNoiseScale, float _LiquidBottomNoiseStrengh, float2 _PixelCell, float _Scale, float _Distort, Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float IN, out float4 OutVector4_1)
        {
        float4 _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
        float _Split_babc1f5e31a04264ac52c77e08f160df_R_1_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[0];
        float _Split_babc1f5e31a04264ac52c77e08f160df_G_2_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[1];
        float _Split_babc1f5e31a04264ac52c77e08f160df_B_3_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[2];
        float _Split_babc1f5e31a04264ac52c77e08f160df_A_4_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[3];
        float2 _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2 = _PixelCell;
        float _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float = _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2[0];
        float _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float = _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2[1];
        float _Split_21288f26c09f404a9991b72c6c6549e5_B_3_Float = 0;
        float _Split_21288f26c09f404a9991b72c6c6549e5_A_4_Float = 0;
        float _Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float;
        Unity_Multiply_float_float(_Split_babc1f5e31a04264ac52c77e08f160df_R_1_Float, _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float, _Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float);
        float _Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float;
        Unity_Divide_float(_Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float, _Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float);
        float _Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float;
        Unity_Multiply_float_float(_Split_babc1f5e31a04264ac52c77e08f160df_G_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float, _Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float);
        float _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float;
        Unity_Divide_float(_Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float, _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float);
        float4 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGBA_4_Vector4;
        float3 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGB_5_Vector3;
        float2 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2;
        Unity_Combine_float(_Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float, _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float, 0, 0, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGBA_4_Vector4, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGB_5_Vector3, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2);
        float _Property_a65d4b108b87421787112feacdc2ad90_Out_0_Float = _LiquidBottomSpeed;
        float _Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_a65d4b108b87421787112feacdc2ad90_Out_0_Float, _Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_025acfae81ee4923b89c42d5e95e3073;
        float4 _ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_025acfae81ee4923b89c42d5e95e3073, _ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4);
        float _Property_632061910c294796a237dd943f583f7a_Out_0_Float = _Distort;
        float4 _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4;
        Unity_Add_float4(_ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4, (_Property_632061910c294796a237dd943f583f7a_Out_0_Float.xxxx), _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4);
        float4 _Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4;
        Unity_Add_float4((_Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float.xxxx), _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4, _Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4);
        float _Property_cd3a650be5e74143bdb6ef87564a6459_Out_0_Float = _LiquidBottomAngleSpeed;
        float _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float;
        Unity_Multiply_float_float(_Property_cd3a650be5e74143bdb6ef87564a6459_Out_0_Float, IN.TimeParameters.x, _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float);
        float _Property_dd1d1cc3b97841399706968720371ee0_Out_0_Float = _LiquidBottomNoiseScale;
        float _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float;
        float _Voronoi_56b538e009a446cd850ace505427cf1b_Cells_4_Float;
        Unity_Voronoi_Deterministic_float((_Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4.xy), _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float, _Property_dd1d1cc3b97841399706968720371ee0_Out_0_Float, _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float, _Voronoi_56b538e009a446cd850ace505427cf1b_Cells_4_Float);
        float _Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float;
        Unity_Step_float(0.5, _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float, _Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float);
        float _Property_953201261ff4463b82ab45ac766060fa_Out_0_Float = _LiquidBottomNoiseStrengh;
        float _Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float;
        Unity_Multiply_float_float(_Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float, _Property_953201261ff4463b82ab45ac766060fa_Out_0_Float, _Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float);
        float2 _TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2;
        Unity_TilingAndOffset_float(_Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2, float2 (1, 1), (_Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float.xx), _TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2);
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).tex, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).samplerstate, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).GetTransformedUV(_TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2) );
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_R_4_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.r;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_G_5_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.g;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_B_6_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.b;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_A_7_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.a;
        OutVector4_1 = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4;
        }
        
        struct Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float
        {
        half4 uv0;
        };
        
        void SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(UnityTexture2D _MainTex, Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D = _MainTex;
        float4 _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.tex, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.samplerstate, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_R_4_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.r;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_G_5_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.g;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.b;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.a;
        float _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float, _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float, _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float);
        OutVector1_1 = _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        struct Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float
        {
        float3 WorldSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(float4 _UV, float _WaveSpeed, float _WaveLength, float _WaveHeight, float _WaveShakeSpeed, float _WaveDistort, Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float IN, out float2 OutVector2_1)
        {
        float4 _UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4 = IN.uv0;
        float _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float = _WaveDistort;
        float _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float;
        Unity_Step_float(0.2, _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float, _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float);
        float _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float;
        Unity_OneMinus_float(_Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float, _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float);
        float _Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float = _WaveSpeed;
        float _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float;
        Unity_Multiply_float_float(_Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float, IN.TimeParameters.x, _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_ece7a048a124490eb96c1c943b36de7e;
        float4 _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_ece7a048a124490eb96c1c943b36de7e, _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4);
        float _Split_ae769ff284874f9a8245510995a1f39d_R_1_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[0];
        float _Split_ae769ff284874f9a8245510995a1f39d_G_2_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[1];
        float _Split_ae769ff284874f9a8245510995a1f39d_B_3_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[2];
        float _Split_ae769ff284874f9a8245510995a1f39d_A_4_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[3];
        float _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float = _WaveLength;
        float _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float;
        Unity_Multiply_float_float(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float);
        float _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float;
        Unity_Add_float(_Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float, _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float);
        float _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float;
        Unity_Sine_float(_Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float, _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float);
        float _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float = _WaveShakeSpeed;
        float _Multiply_70a55d008a294763835173cb84629169_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float2 _Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2 = float2(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float(_Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2, 10, _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float);
        float _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float;
        Unity_Subtract_float(_SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float, 0.5, _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float);
        float _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float;
        Unity_Multiply_float_float(_Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float, 3, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float);
        float _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float;
        Unity_Step_float(_Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float);
        float _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float;
        Unity_Minimum_float(_OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float, _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float);
        float _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float = _WaveHeight;
        float _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float;
        Unity_Multiply_float_float(_Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float, _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float);
        float4 _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4;
        float3 _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3;
        float2 _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2;
        Unity_Combine_float(0, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float, 0, 0, _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4, _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3, _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2);
        float2 _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        Unity_Add_float2((_UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4.xy), _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2, _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2);
        OutVector2_1 = _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        }
        
        struct Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float
        {
        };
        
        void SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(UnityTexture2D _MainTex, float4 _UV, Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D = _MainTex;
        float4 _Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4 = _UV;
        float4 _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.tex, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.samplerstate, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.GetTransformedUV((_Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4.xy)) );
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_R_4_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.r;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.g;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_B_6_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.b;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.a;
        float _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float, _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float, _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float);
        OutVector1_1 = _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4 = _DistortTran;
            UnityTexture2D _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_DistortTex);
            float _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float = _DistortUVStrength;
            float4 _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4 = _DistortColor;
            Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float _WaterDistort_535e0977d51441b3927c7ec02959bc97;
            _WaterDistort_535e0977d51441b3927c7ec02959bc97.WorldSpacePosition = IN.WorldSpacePosition;
            float4 _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4;
            float _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float;
            SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(_Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4, _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D, _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float, _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float);
            float4 _Property_c07fc1928f0146aab38ffc698890428e_Out_0_Vector4 = _CausticBaseColor;
            UnityTexture2D _Property_23c78aa0988448d193906bb14bcbd874_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_CausticBaseTex);
            float _Property_0919388d90484df7b47156bdff8991e4_Out_0_Float = _CausticBaseStrength;
            float2 _Property_3df02cd1b2b54257a5f27cbedaa58dc5_Out_0_Vector2 = _MaskSpeed;
            float _Property_ed5a798690254c86b56f751c75170330_Out_0_Float = _MaskScale;
            float _Property_0c3340ffe9b94bc882f54dbc156c47d0_Out_0_Float = _MaskRange;
            float _Property_ce5b21ef3eaf4b5bb7954300c744b09e_Out_0_Float = _MaskStrength;
            float _Property_40b8633fc13d4ee997a5c69df3d82f87_Out_0_Float = _CausticSpeed;
            float _Property_5e6fecce44e247f5a33ac030c2ad9e21_Out_0_Float = _CausitcBlend;
            float _Property_d5308f1b3b514b8da3799eb0f61a694d_Out_0_Float = _CausticScale;
            float _Property_5c2585358f944caa8d34cf98b5694651_Out_0_Float = _CausticNoiseScale;
            Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb;
            _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb.WorldSpacePosition = IN.WorldSpacePosition;
            _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb.TimeParameters = IN.TimeParameters;
            float2 _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2;
            SG_UVCaustic_7660615f0780a3a47ab0e632916466ff_float(_Property_40b8633fc13d4ee997a5c69df3d82f87_Out_0_Float, _Property_5e6fecce44e247f5a33ac030c2ad9e21_Out_0_Float, _Property_d5308f1b3b514b8da3799eb0f61a694d_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _Property_5c2585358f944caa8d34cf98b5694651_Out_0_Float, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2);
            Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2;
            _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2.TimeParameters = IN.TimeParameters;
            float _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float;
            float4 _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4;
            SG_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float(_Property_c07fc1928f0146aab38ffc698890428e_Out_0_Vector4, _Property_23c78aa0988448d193906bb14bcbd874_Out_0_Texture2D, _Property_0919388d90484df7b47156bdff8991e4_Out_0_Float, _Property_3df02cd1b2b54257a5f27cbedaa58dc5_Out_0_Vector2, _Property_ed5a798690254c86b56f751c75170330_Out_0_Float, _Property_0c3340ffe9b94bc882f54dbc156c47d0_Out_0_Float, _Property_ce5b21ef3eaf4b5bb7954300c744b09e_Out_0_Float, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4);
            float _Property_2470e4696d544eb9bdbd493f89e33be9_Out_0_Float = _FloatingMaskScale;
            float _Property_edc40ab4725a443989cbdc4861756e9b_Out_0_Float = _FloatingSpeed;
            float _Property_27f3ddc40e54460ca0d0c2983d883803_Out_0_Float = _FloatingScale;
            float _Property_f76b3b80b7dd403ca709d91f624d7dcd_Out_0_Float = _FloatingStep;
            float _Property_219c9e59f3f24b30a2c77917c412b0b6_Out_0_Float = _FloatingStrength;
            Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float _WaterFloating_12c80082bf2c4682a4f593c5690670d5;
            _WaterFloating_12c80082bf2c4682a4f593c5690670d5.WorldSpacePosition = IN.WorldSpacePosition;
            _WaterFloating_12c80082bf2c4682a4f593c5690670d5.TimeParameters = IN.TimeParameters;
            float _WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float;
            SG_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float(_Property_2470e4696d544eb9bdbd493f89e33be9_Out_0_Float, _Property_edc40ab4725a443989cbdc4861756e9b_Out_0_Float, _Property_27f3ddc40e54460ca0d0c2983d883803_Out_0_Float, _Property_f76b3b80b7dd403ca709d91f624d7dcd_Out_0_Float, _Property_219c9e59f3f24b30a2c77917c412b0b6_Out_0_Float, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float, _WaterFloating_12c80082bf2c4682a4f593c5690670d5, _WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float);
            float4 _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4;
            Unity_Add_float4(_WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4, (_WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float.xxxx), _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4);
            float4 _Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4;
            Unity_Add_float4(_WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4, _Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4);
            float _Property_16a4ee35313f48afacbb28383a2d74ca_Out_0_Float = _BottomSpeed;
            float _Property_bfa36bdf8a3942399424d5b51d83dd06_Out_0_Float = _BottomAngleSpeed;
            float _Property_1a75e1efb86c457087c5f5bd2bab3e11_Out_0_Float = _BottomNoiseScale;
            float _Property_e2d3edae3a34460784d651b854f86f5e_Out_0_Float = _BottomNoiseStrengh;
            float2 _Property_801c129985d747aba3e7481ad87dd0b8_Out_0_Vector2 = _BottomPixelCount;
            float _Property_9db9f5b047a64b2483dd9c946159309b_Out_0_Float = _BottomScale;
            Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float _CatchLiquidBottom_f94162af491145679d1c11914ad36b53;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.WorldSpacePosition = IN.WorldSpacePosition;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.NDCPosition = IN.NDCPosition;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.TimeParameters = IN.TimeParameters;
            float4 _CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4;
            SG_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float(_Property_16a4ee35313f48afacbb28383a2d74ca_Out_0_Float, _Property_bfa36bdf8a3942399424d5b51d83dd06_Out_0_Float, _Property_1a75e1efb86c457087c5f5bd2bab3e11_Out_0_Float, _Property_e2d3edae3a34460784d651b854f86f5e_Out_0_Float, _Property_801c129985d747aba3e7481ad87dd0b8_Out_0_Vector2, _Property_9db9f5b047a64b2483dd9c946159309b_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _CatchLiquidBottom_f94162af491145679d1c11914ad36b53, _CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4);
            float _Property_70d5267ba184430a8fed1cd11db83493_Out_0_Float = _BottomStrengh;
            float4 _Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4;
            Unity_Multiply_float4_float4(_CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4, (_Property_70d5267ba184430a8fed1cd11db83493_Out_0_Float.xxxx), _Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4);
            UnityTexture2D _Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3;
            _RGBCutB_e130f7613eb140988bc99e39b3f57fb3.uv0 = IN.uv0;
            float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float;
            SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(_Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float);
            UnityTexture2D _Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4 = IN.uv0;
            float _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float = _EdgeWaveSpeed;
            float _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float = _EdgeWaveLength;
            float _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float = _EdgeWaveHeight;
            float _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float = _EdgeWaveShakeSpeed;
            Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.WorldSpacePosition = IN.WorldSpacePosition;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.uv0 = IN.uv0;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.TimeParameters = IN.TimeParameters;
            float2 _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2;
            SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(_UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4, _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float, _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float, _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float, _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2);
            Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852;
            float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float;
            SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(_Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D, (float4(_UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2, 0.0, 1.0)), _RGBCutG_62abc9b5809b4b07a9fae459f7e96852, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float);
            float _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float;
            Unity_Step_float(0.1, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float);
            float _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float;
            Unity_Subtract_float(_RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float, _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float);
            float _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float;
            Unity_Clamp_float(_Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float, 0, 1, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float);
            float4 _Property_e7e806c9a8a64fd59d0a70cd6598a43b_Out_0_Vector4 = _BaseColor;
            float4 _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float.xxxx), _Property_e7e806c9a8a64fd59d0a70cd6598a43b_Out_0_Vector4, _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4);
            float _Property_d3385627294b41949fa94f7d49df2d80_Out_0_Float = _BottomAlpha;
            float4 _Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4;
            Unity_Lerp_float4(_Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4, _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4, (_Property_d3385627294b41949fa94f7d49df2d80_Out_0_Float.xxxx), _Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4);
            float4 _Property_5156b57acc9f49efa11c95a30950d9a2_Out_0_Vector4 = _EdgeWaveColor;
            float4 _Property_81b01f5ed69941dd8601ee9432af1a5f_Out_0_Vector4 = _EdgeWaveHighLightColor;
            float _Property_7a60d176b9994138bfe5bd4b0415dade_Out_0_Float = _EdgeWaveColorNoiseScale;
            Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7;
            _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7.WorldSpacePosition = IN.WorldSpacePosition;
            _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7.TimeParameters = IN.TimeParameters;
            float _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float;
            SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(1, 1, 16, _Property_7a60d176b9994138bfe5bd4b0415dade_Out_0_Float, 0.5, _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7, _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float);
            float _Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float;
            Unity_Multiply_float_float(_EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float, 10, _Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float);
            float _Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float;
            Unity_Floor_float(_Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float, _Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float);
            float _Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float;
            Unity_Divide_float(_Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float, 5, _Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float);
            float4 _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_81b01f5ed69941dd8601ee9432af1a5f_Out_0_Vector4, (_Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float.xxxx), _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4);
            float4 _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4;
            Unity_Add_float4(_Property_5156b57acc9f49efa11c95a30950d9a2_Out_0_Vector4, _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4, _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4);
            float4 _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4;
            Unity_Multiply_float4_float4((_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float.xxxx), _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4, _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4);
            float4 _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4;
            Unity_Add_float4(_Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4, _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4, _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4);
            float4 _Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4;
            Unity_Add_float4(_Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4, _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4, _Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4);
            float _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            Unity_Add_float(_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float, _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float);
            surface.BaseColor = (_Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4.xyz);
            surface.Alpha = _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteNormalPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D_TexelSize;
        float4 _MainTex_TexelSize;
        float4 _BaseColor;
        float4 _EdgeWaveColor;
        float _EdgeWaveSpeed;
        float _EdgeWaveLength;
        float _EdgeWaveHeight;
        float _EdgeWaveShakeSpeed;
        float _EdgeWaveColorNoiseScale;
        float4 _EdgeWaveHighLightColor;
        float _BottomSpeed;
        float _BottomNoiseScale;
        float _BottomNoiseStrengh;
        float _BottomAlpha;
        float2 _BottomPixelCount;
        float _BottomScale;
        float _BottomStrengh;
        float _CausticSpeed;
        float _CausitcBlend;
        float _CausticScale;
        float4 _CausticBaseColor;
        float4 _CausticBaseTex_TexelSize;
        float _CausticBaseStrength;
        float _FloatingSpeed;
        float _FloatingScale;
        float _FloatingStep;
        float _FloatingStrength;
        float _FloatingMaskSpeed;
        float _FloatingMaskScale;
        float2 _MaskSpeed;
        float _MaskScale;
        float _MaskRange;
        float4 _DistortTex_TexelSize;
        float _DistortUVStrength;
        float4 _DistortColor;
        float4 _DistortTran;
        float _BottomAngleSpeed;
        float _CausticNoiseScale;
        float _MaskStrength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_CausticBaseTex);
        SAMPLER(sampler_CausticBaseTex);
        TEXTURE2D(_DistortTex);
        SAMPLER(sampler_DistortTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float4(float4 In, out float4 Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A / B;
        }
        
        struct Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float
        {
        };
        
        void SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float(float4 _Input, float _Grid, Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float IN, out float4 New_0)
        {
        float4 _Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4 = _Input;
        float _Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float = _Grid;
        float4 _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4);
        float4 _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4;
        Unity_Floor_float4(_Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4, _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4);
        float4 _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        Unity_Divide_float4(_Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4);
        New_0 = _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
        float2 i = floor(uv);
        float2 f = frac(uv);
        f = f * f * (3.0 - 2.0 * f);
        uv = abs(frac(uv) - 0.5);
        float2 c0 = i + float2(0.0, 0.0);
        float2 c1 = i + float2(1.0, 0.0);
        float2 c2 = i + float2(0.0, 1.0);
        float2 c3 = i + float2(1.0, 1.0);
        float r0; Hash_Tchou_2_1_float(c0, r0);
        float r1; Hash_Tchou_2_1_float(c1, r1);
        float r2; Hash_Tchou_2_1_float(c2, r2);
        float r3; Hash_Tchou_2_1_float(c3, r3);
        float bottomOfGrid = lerp(r0, r1, f.x);
        float topOfGrid = lerp(r2, r3, f.x);
        float t = lerp(bottomOfGrid, topOfGrid, f.y);
        return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
        float freq, amp;
        Out = 0.0f;
        freq = pow(2.0, float(0));
        amp = pow(0.5, float(3-0));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(1));
        amp = pow(0.5, float(3-1));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(2));
        amp = pow(0.5, float(3-2));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        struct Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float
        {
        float3 WorldSpacePosition;
        };
        
        void SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(float4 _DistortTran, UnityTexture2D _DistortTex, float _DistortUVStrength, float4 _DistortColor, Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float IN, out float4 Color_0, out float Distort_1)
        {
        float4 _Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4 = _DistortColor;
        float _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float = 16;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_d759dae6c2594e7d8ec5b45a94779549;
        float4 _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float, _ToGrid_d759dae6c2594e7d8ec5b45a94779549, _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4);
        float _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4.xy), 25, _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float);
        UnityTexture2D _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D = _DistortTex;
        float4 _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4 = _DistortTran;
        float _Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[0];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[1];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[2];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[3];
        float2 _Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676;
        float4 _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4);
        float2 _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float);
        float2 _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2;
        Unity_Subtract_float2((_ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4.xy), _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2);
        float2 _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2, _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2);
        float2 _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2;
        Unity_Add_float2(_Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2, float2(0.5, 0.5), _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2);
        float4 _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.tex, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.samplerstate, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.GetTransformedUV(_Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2) );
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.r;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_G_5_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.g;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_B_6_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.b;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_A_7_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.a;
        float _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float;
        Unity_Multiply_float_float(_SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float);
        float _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float, 5, _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float);
        float _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float;
        Unity_Floor_float(_Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float, _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float);
        float _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float;
        Unity_Divide_float(_Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float, 5, _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float);
        float4 _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4, (_Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float.xxxx), _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4);
        float _Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float = _DistortUVStrength;
        float _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        Unity_Multiply_float_float(_Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float);
        Color_0 = _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Distort_1 = _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        struct Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float
        {
        float3 WorldSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(float4 _UV, float _WaveSpeed, float _WaveLength, float _WaveHeight, float _WaveShakeSpeed, float _WaveDistort, Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float IN, out float2 OutVector2_1)
        {
        float4 _UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4 = IN.uv0;
        float _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float = _WaveDistort;
        float _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float;
        Unity_Step_float(0.2, _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float, _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float);
        float _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float;
        Unity_OneMinus_float(_Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float, _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float);
        float _Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float = _WaveSpeed;
        float _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float;
        Unity_Multiply_float_float(_Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float, IN.TimeParameters.x, _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_ece7a048a124490eb96c1c943b36de7e;
        float4 _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_ece7a048a124490eb96c1c943b36de7e, _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4);
        float _Split_ae769ff284874f9a8245510995a1f39d_R_1_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[0];
        float _Split_ae769ff284874f9a8245510995a1f39d_G_2_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[1];
        float _Split_ae769ff284874f9a8245510995a1f39d_B_3_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[2];
        float _Split_ae769ff284874f9a8245510995a1f39d_A_4_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[3];
        float _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float = _WaveLength;
        float _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float;
        Unity_Multiply_float_float(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float);
        float _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float;
        Unity_Add_float(_Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float, _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float);
        float _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float;
        Unity_Sine_float(_Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float, _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float);
        float _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float = _WaveShakeSpeed;
        float _Multiply_70a55d008a294763835173cb84629169_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float2 _Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2 = float2(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float(_Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2, 10, _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float);
        float _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float;
        Unity_Subtract_float(_SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float, 0.5, _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float);
        float _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float;
        Unity_Multiply_float_float(_Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float, 3, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float);
        float _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float;
        Unity_Step_float(_Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float);
        float _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float;
        Unity_Minimum_float(_OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float, _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float);
        float _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float = _WaveHeight;
        float _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float;
        Unity_Multiply_float_float(_Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float, _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float);
        float4 _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4;
        float3 _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3;
        float2 _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2;
        Unity_Combine_float(0, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float, 0, 0, _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4, _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3, _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2);
        float2 _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        Unity_Add_float2((_UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4.xy), _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2, _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2);
        OutVector2_1 = _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        }
        
        struct Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float
        {
        };
        
        void SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(UnityTexture2D _MainTex, float4 _UV, Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D = _MainTex;
        float4 _Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4 = _UV;
        float4 _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.tex, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.samplerstate, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.GetTransformedUV((_Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4.xy)) );
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_R_4_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.r;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.g;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_B_6_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.b;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.a;
        float _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float, _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float, _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float);
        OutVector1_1 = _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        }
        
        struct Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float
        {
        half4 uv0;
        };
        
        void SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(UnityTexture2D _MainTex, Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D = _MainTex;
        float4 _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.tex, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.samplerstate, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_R_4_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.r;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_G_5_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.g;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.b;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.a;
        float _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float, _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float, _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float);
        OutVector1_1 = _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4 = IN.uv0;
            float _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float = _EdgeWaveSpeed;
            float _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float = _EdgeWaveLength;
            float _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float = _EdgeWaveHeight;
            float _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float = _EdgeWaveShakeSpeed;
            float4 _Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4 = _DistortTran;
            UnityTexture2D _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_DistortTex);
            float _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float = _DistortUVStrength;
            float4 _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4 = _DistortColor;
            Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float _WaterDistort_535e0977d51441b3927c7ec02959bc97;
            _WaterDistort_535e0977d51441b3927c7ec02959bc97.WorldSpacePosition = IN.WorldSpacePosition;
            float4 _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4;
            float _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float;
            SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(_Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4, _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D, _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float, _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float);
            Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.WorldSpacePosition = IN.WorldSpacePosition;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.uv0 = IN.uv0;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.TimeParameters = IN.TimeParameters;
            float2 _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2;
            SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(_UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4, _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float, _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float, _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float, _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2);
            Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852;
            float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float;
            SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(_Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D, (float4(_UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2, 0.0, 1.0)), _RGBCutG_62abc9b5809b4b07a9fae459f7e96852, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float);
            UnityTexture2D _Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3;
            _RGBCutB_e130f7613eb140988bc99e39b3f57fb3.uv0 = IN.uv0;
            float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float;
            SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(_Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float);
            float _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float;
            Unity_Step_float(0.1, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float);
            float _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float;
            Unity_Subtract_float(_RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float, _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float);
            float _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float;
            Unity_Clamp_float(_Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float, 0, 1, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float);
            float _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            Unity_Add_float(_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float, _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float);
            surface.Alpha = _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D_TexelSize;
        float4 _MainTex_TexelSize;
        float4 _BaseColor;
        float4 _EdgeWaveColor;
        float _EdgeWaveSpeed;
        float _EdgeWaveLength;
        float _EdgeWaveHeight;
        float _EdgeWaveShakeSpeed;
        float _EdgeWaveColorNoiseScale;
        float4 _EdgeWaveHighLightColor;
        float _BottomSpeed;
        float _BottomNoiseScale;
        float _BottomNoiseStrengh;
        float _BottomAlpha;
        float2 _BottomPixelCount;
        float _BottomScale;
        float _BottomStrengh;
        float _CausticSpeed;
        float _CausitcBlend;
        float _CausticScale;
        float4 _CausticBaseColor;
        float4 _CausticBaseTex_TexelSize;
        float _CausticBaseStrength;
        float _FloatingSpeed;
        float _FloatingScale;
        float _FloatingStep;
        float _FloatingStrength;
        float _FloatingMaskSpeed;
        float _FloatingMaskScale;
        float2 _MaskSpeed;
        float _MaskScale;
        float _MaskRange;
        float4 _DistortTex_TexelSize;
        float _DistortUVStrength;
        float4 _DistortColor;
        float4 _DistortTran;
        float _BottomAngleSpeed;
        float _CausticNoiseScale;
        float _MaskStrength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_CausticBaseTex);
        SAMPLER(sampler_CausticBaseTex);
        TEXTURE2D(_DistortTex);
        SAMPLER(sampler_DistortTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float4(float4 In, out float4 Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A / B;
        }
        
        struct Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float
        {
        };
        
        void SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float(float4 _Input, float _Grid, Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float IN, out float4 New_0)
        {
        float4 _Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4 = _Input;
        float _Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float = _Grid;
        float4 _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4);
        float4 _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4;
        Unity_Floor_float4(_Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4, _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4);
        float4 _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        Unity_Divide_float4(_Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4);
        New_0 = _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
        float2 i = floor(uv);
        float2 f = frac(uv);
        f = f * f * (3.0 - 2.0 * f);
        uv = abs(frac(uv) - 0.5);
        float2 c0 = i + float2(0.0, 0.0);
        float2 c1 = i + float2(1.0, 0.0);
        float2 c2 = i + float2(0.0, 1.0);
        float2 c3 = i + float2(1.0, 1.0);
        float r0; Hash_Tchou_2_1_float(c0, r0);
        float r1; Hash_Tchou_2_1_float(c1, r1);
        float r2; Hash_Tchou_2_1_float(c2, r2);
        float r3; Hash_Tchou_2_1_float(c3, r3);
        float bottomOfGrid = lerp(r0, r1, f.x);
        float topOfGrid = lerp(r2, r3, f.x);
        float t = lerp(bottomOfGrid, topOfGrid, f.y);
        return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
        float freq, amp;
        Out = 0.0f;
        freq = pow(2.0, float(0));
        amp = pow(0.5, float(3-0));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(1));
        amp = pow(0.5, float(3-1));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(2));
        amp = pow(0.5, float(3-2));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        struct Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float
        {
        float3 WorldSpacePosition;
        };
        
        void SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(float4 _DistortTran, UnityTexture2D _DistortTex, float _DistortUVStrength, float4 _DistortColor, Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float IN, out float4 Color_0, out float Distort_1)
        {
        float4 _Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4 = _DistortColor;
        float _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float = 16;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_d759dae6c2594e7d8ec5b45a94779549;
        float4 _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float, _ToGrid_d759dae6c2594e7d8ec5b45a94779549, _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4);
        float _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4.xy), 25, _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float);
        UnityTexture2D _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D = _DistortTex;
        float4 _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4 = _DistortTran;
        float _Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[0];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[1];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[2];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[3];
        float2 _Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676;
        float4 _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4);
        float2 _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float);
        float2 _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2;
        Unity_Subtract_float2((_ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4.xy), _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2);
        float2 _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2, _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2);
        float2 _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2;
        Unity_Add_float2(_Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2, float2(0.5, 0.5), _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2);
        float4 _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.tex, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.samplerstate, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.GetTransformedUV(_Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2) );
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.r;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_G_5_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.g;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_B_6_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.b;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_A_7_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.a;
        float _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float;
        Unity_Multiply_float_float(_SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float);
        float _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float, 5, _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float);
        float _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float;
        Unity_Floor_float(_Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float, _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float);
        float _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float;
        Unity_Divide_float(_Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float, 5, _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float);
        float4 _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4, (_Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float.xxxx), _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4);
        float _Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float = _DistortUVStrength;
        float _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        Unity_Multiply_float_float(_Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float);
        Color_0 = _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Distort_1 = _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        struct Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float
        {
        float3 WorldSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(float4 _UV, float _WaveSpeed, float _WaveLength, float _WaveHeight, float _WaveShakeSpeed, float _WaveDistort, Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float IN, out float2 OutVector2_1)
        {
        float4 _UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4 = IN.uv0;
        float _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float = _WaveDistort;
        float _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float;
        Unity_Step_float(0.2, _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float, _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float);
        float _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float;
        Unity_OneMinus_float(_Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float, _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float);
        float _Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float = _WaveSpeed;
        float _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float;
        Unity_Multiply_float_float(_Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float, IN.TimeParameters.x, _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_ece7a048a124490eb96c1c943b36de7e;
        float4 _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_ece7a048a124490eb96c1c943b36de7e, _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4);
        float _Split_ae769ff284874f9a8245510995a1f39d_R_1_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[0];
        float _Split_ae769ff284874f9a8245510995a1f39d_G_2_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[1];
        float _Split_ae769ff284874f9a8245510995a1f39d_B_3_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[2];
        float _Split_ae769ff284874f9a8245510995a1f39d_A_4_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[3];
        float _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float = _WaveLength;
        float _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float;
        Unity_Multiply_float_float(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float);
        float _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float;
        Unity_Add_float(_Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float, _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float);
        float _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float;
        Unity_Sine_float(_Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float, _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float);
        float _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float = _WaveShakeSpeed;
        float _Multiply_70a55d008a294763835173cb84629169_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float2 _Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2 = float2(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float(_Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2, 10, _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float);
        float _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float;
        Unity_Subtract_float(_SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float, 0.5, _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float);
        float _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float;
        Unity_Multiply_float_float(_Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float, 3, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float);
        float _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float;
        Unity_Step_float(_Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float);
        float _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float;
        Unity_Minimum_float(_OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float, _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float);
        float _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float = _WaveHeight;
        float _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float;
        Unity_Multiply_float_float(_Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float, _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float);
        float4 _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4;
        float3 _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3;
        float2 _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2;
        Unity_Combine_float(0, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float, 0, 0, _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4, _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3, _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2);
        float2 _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        Unity_Add_float2((_UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4.xy), _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2, _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2);
        OutVector2_1 = _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        }
        
        struct Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float
        {
        };
        
        void SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(UnityTexture2D _MainTex, float4 _UV, Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D = _MainTex;
        float4 _Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4 = _UV;
        float4 _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.tex, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.samplerstate, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.GetTransformedUV((_Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4.xy)) );
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_R_4_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.r;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.g;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_B_6_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.b;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.a;
        float _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float, _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float, _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float);
        OutVector1_1 = _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        }
        
        struct Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float
        {
        half4 uv0;
        };
        
        void SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(UnityTexture2D _MainTex, Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D = _MainTex;
        float4 _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.tex, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.samplerstate, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_R_4_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.r;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_G_5_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.g;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.b;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.a;
        float _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float, _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float, _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float);
        OutVector1_1 = _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4 = IN.uv0;
            float _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float = _EdgeWaveSpeed;
            float _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float = _EdgeWaveLength;
            float _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float = _EdgeWaveHeight;
            float _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float = _EdgeWaveShakeSpeed;
            float4 _Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4 = _DistortTran;
            UnityTexture2D _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_DistortTex);
            float _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float = _DistortUVStrength;
            float4 _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4 = _DistortColor;
            Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float _WaterDistort_535e0977d51441b3927c7ec02959bc97;
            _WaterDistort_535e0977d51441b3927c7ec02959bc97.WorldSpacePosition = IN.WorldSpacePosition;
            float4 _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4;
            float _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float;
            SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(_Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4, _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D, _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float, _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float);
            Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.WorldSpacePosition = IN.WorldSpacePosition;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.uv0 = IN.uv0;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.TimeParameters = IN.TimeParameters;
            float2 _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2;
            SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(_UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4, _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float, _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float, _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float, _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2);
            Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852;
            float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float;
            SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(_Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D, (float4(_UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2, 0.0, 1.0)), _RGBCutG_62abc9b5809b4b07a9fae459f7e96852, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float);
            UnityTexture2D _Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3;
            _RGBCutB_e130f7613eb140988bc99e39b3f57fb3.uv0 = IN.uv0;
            float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float;
            SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(_Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float);
            float _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float;
            Unity_Step_float(0.1, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float);
            float _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float;
            Unity_Subtract_float(_RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float, _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float);
            float _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float;
            Unity_Clamp_float(_Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float, 0, 1, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float);
            float _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            Unity_Add_float(_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float, _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float);
            surface.Alpha = _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Sprite Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D_TexelSize;
        float4 _MainTex_TexelSize;
        float4 _BaseColor;
        float4 _EdgeWaveColor;
        float _EdgeWaveSpeed;
        float _EdgeWaveLength;
        float _EdgeWaveHeight;
        float _EdgeWaveShakeSpeed;
        float _EdgeWaveColorNoiseScale;
        float4 _EdgeWaveHighLightColor;
        float _BottomSpeed;
        float _BottomNoiseScale;
        float _BottomNoiseStrengh;
        float _BottomAlpha;
        float2 _BottomPixelCount;
        float _BottomScale;
        float _BottomStrengh;
        float _CausticSpeed;
        float _CausitcBlend;
        float _CausticScale;
        float4 _CausticBaseColor;
        float4 _CausticBaseTex_TexelSize;
        float _CausticBaseStrength;
        float _FloatingSpeed;
        float _FloatingScale;
        float _FloatingStep;
        float _FloatingStrength;
        float _FloatingMaskSpeed;
        float _FloatingMaskScale;
        float2 _MaskSpeed;
        float _MaskScale;
        float _MaskRange;
        float4 _DistortTex_TexelSize;
        float _DistortUVStrength;
        float4 _DistortColor;
        float4 _DistortTran;
        float _BottomAngleSpeed;
        float _CausticNoiseScale;
        float _MaskStrength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_CausticBaseTex);
        SAMPLER(sampler_CausticBaseTex);
        TEXTURE2D(_DistortTex);
        SAMPLER(sampler_DistortTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float4(float4 In, out float4 Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A / B;
        }
        
        struct Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float
        {
        };
        
        void SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float(float4 _Input, float _Grid, Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float IN, out float4 New_0)
        {
        float4 _Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4 = _Input;
        float _Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float = _Grid;
        float4 _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_4877dd40a49c4419b54b194b94f9c4cb_Out_0_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4);
        float4 _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4;
        Unity_Floor_float4(_Multiply_a7238e5d750d46eabeabfa2fce0dcacf_Out_2_Vector4, _Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4);
        float4 _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        Unity_Divide_float4(_Floor_a2570d0ab4dd412cac9687fa40d8274a_Out_1_Vector4, (_Property_9998558e48034d74a4ca09ab40b4fafd_Out_0_Float.xxxx), _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4);
        New_0 = _Divide_b691b0663ffb41a3970f80b8e3251cef_Out_2_Vector4;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
        float2 i = floor(uv);
        float2 f = frac(uv);
        f = f * f * (3.0 - 2.0 * f);
        uv = abs(frac(uv) - 0.5);
        float2 c0 = i + float2(0.0, 0.0);
        float2 c1 = i + float2(1.0, 0.0);
        float2 c2 = i + float2(0.0, 1.0);
        float2 c3 = i + float2(1.0, 1.0);
        float r0; Hash_Tchou_2_1_float(c0, r0);
        float r1; Hash_Tchou_2_1_float(c1, r1);
        float r2; Hash_Tchou_2_1_float(c2, r2);
        float r3; Hash_Tchou_2_1_float(c3, r3);
        float bottomOfGrid = lerp(r0, r1, f.x);
        float topOfGrid = lerp(r2, r3, f.x);
        float t = lerp(bottomOfGrid, topOfGrid, f.y);
        return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
        float freq, amp;
        Out = 0.0f;
        freq = pow(2.0, float(0));
        amp = pow(0.5, float(3-0));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(1));
        amp = pow(0.5, float(3-1));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        freq = pow(2.0, float(2));
        amp = pow(0.5, float(3-2));
        Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        struct Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float
        {
        float3 WorldSpacePosition;
        };
        
        void SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(float4 _DistortTran, UnityTexture2D _DistortTex, float _DistortUVStrength, float4 _DistortColor, Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float IN, out float4 Color_0, out float Distort_1)
        {
        float4 _Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4 = _DistortColor;
        float _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float = 16;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_d759dae6c2594e7d8ec5b45a94779549;
        float4 _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Float_1aff61bf689d4108b50d7f187e109723_Out_0_Float, _ToGrid_d759dae6c2594e7d8ec5b45a94779549, _ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4);
        float _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_ToGrid_d759dae6c2594e7d8ec5b45a94779549_New_0_Vector4.xy), 25, _SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float);
        UnityTexture2D _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D = _DistortTex;
        float4 _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4 = _DistortTran;
        float _Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[0];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[1];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[2];
        float _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float = _Property_d8b6f0d404d14b61967f9554a2d38c95_Out_0_Vector4[3];
        float2 _Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_B_3_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_A_4_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676;
        float4 _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676, _ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4);
        float2 _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2 = float2(_Split_b70baf73305643ab846edc32d2bc5ad8_R_1_Float, _Split_b70baf73305643ab846edc32d2bc5ad8_G_2_Float);
        float2 _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2;
        Unity_Subtract_float2((_ToGrid_1cb99c48f85f4abb849f48ac6cfb8676_New_0_Vector4.xy), _Vector2_ae4311c8b82a47dcac6b4dcd6ea3d1ea_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2);
        float2 _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_7421816df61840d6990127f9a0a69be3_Out_0_Vector2, _Subtract_51bf562c710a426fb45ef5a8580be782_Out_2_Vector2, _Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2);
        float2 _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2;
        Unity_Add_float2(_Multiply_57a33da067d14699aa81db6e10fe144f_Out_2_Vector2, float2(0.5, 0.5), _Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2);
        float4 _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.tex, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.samplerstate, _Property_e505dd402f1043788457328d7244e748_Out_0_Texture2D.GetTransformedUV(_Add_b7d28573836441369dd2cc612fc02337_Out_2_Vector2) );
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.r;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_G_5_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.g;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_B_6_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.b;
        float _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_A_7_Float = _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_RGBA_0_Vector4.a;
        float _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float;
        Unity_Multiply_float_float(_SimpleNoise_e2da9df85012402e86ce824dafc19000_Out_2_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float);
        float _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_137cc8036298419aa3006d00e313a5ad_Out_2_Float, 5, _Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float);
        float _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float;
        Unity_Floor_float(_Multiply_d19e15f3d7aa40308fed2dc96051490f_Out_2_Float, _Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float);
        float _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float;
        Unity_Divide_float(_Floor_ab9db3e156e04e779c171c01b3265189_Out_1_Float, 5, _Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float);
        float4 _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_9c2e0d4e88eb4c5889cee5bf334bf26f_Out_0_Vector4, (_Divide_3e8f5c401de44d70a77401c963458e91_Out_2_Float.xxxx), _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4);
        float _Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float = _DistortUVStrength;
        float _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        Unity_Multiply_float_float(_Property_933c1eb955b4439e80d2714a5b5204d4_Out_0_Float, _SampleTexture2D_0f346415e7a349648b76eee77a4f5a57_R_4_Float, _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float);
        Color_0 = _Multiply_430e2a49e12c40e79306e900bffb6244_Out_2_Vector4;
        Distort_1 = _Multiply_d6a8188b07a04083aa0d1658e105447f_Out_2_Float;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
        float x; Hash_Tchou_2_1_float(p, x);
        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
        float2 p = UV * Scale.xy;
        float2 ip = floor(p);
        float2 fp = frac(p);
        float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
        float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
        float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
        float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        struct Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float
        {
        };
        
        void SG_FloatToPolar_d32299cdd298f07439c149bb0b195311_float(float _Value, float _Strength, Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float IN, out float2 OutVector2_1)
        {
        float _Property_d31c4d35f15b4d97be7577a22ceb143f_Out_0_Float = _Value;
        float Constant_a26c58304c444c2dad6d66d54b8cd98d = 3.141593;
        float _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float;
        Unity_Multiply_float_float(Constant_a26c58304c444c2dad6d66d54b8cd98d, 2, _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float);
        float _Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float;
        Unity_Multiply_float_float(_Property_d31c4d35f15b4d97be7577a22ceb143f_Out_0_Float, _Multiply_5263974c54304a74b8845c9a81eb877c_Out_2_Float, _Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float);
        float _Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float;
        Unity_Cosine_float(_Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float, _Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float);
        float _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float;
        Unity_Sine_float(_Multiply_88972d8bb6f742869ae59476632dd727_Out_2_Float, _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float);
        float2 _Vector2_ecd6022db98d475db804ef747cd313f1_Out_0_Vector2 = float2(_Cosine_608ade21e8c04aff953ca9be44aff498_Out_1_Float, _Sine_24849ca664a54d2586d90a17dc3f796d_Out_1_Float);
        float _Property_557076e218104d538d6e410a74d0cd36_Out_0_Float = _Strength;
        float2 _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_ecd6022db98d475db804ef747cd313f1_Out_0_Vector2, (_Property_557076e218104d538d6e410a74d0cd36_Out_0_Float.xx), _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2);
        OutVector2_1 = _Multiply_d6ff033af7b045d4a9a64d3944ebbb93_Out_2_Vector2;
        }
        
        struct Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_UVCaustic_7660615f0780a3a47ab0e632916466ff_float(float _CausticSpeed, float _CausticBlend, float _CausticScale, float _Distort, float _CausticNoiseScale, Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float IN, out float2 New_0)
        {
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_a81b27dd86034a94abb2a30b48b0192c;
        float4 _ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_a81b27dd86034a94abb2a30b48b0192c, _ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4);
        float _Property_ac7db366df844e4990fb5aafebd5921b_Out_0_Float = _CausticScale;
        float4 _Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4;
        Unity_Divide_float4(_ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4, (_Property_ac7db366df844e4990fb5aafebd5921b_Out_0_Float.xxxx), _Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4);
        float _Property_b1507ced237e4e97811b7632476b1641_Out_0_Float = _Distort;
        float4 _Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4;
        Unity_Add_float4(_Divide_8d59a3161d1a46c1baa61662aed3567e_Out_2_Vector4, (_Property_b1507ced237e4e97811b7632476b1641_Out_0_Float.xxxx), _Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4);
        float _Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float = _CausticSpeed;
        float2 _Vector2_4eedbb6ddec7420db830b4c2c1d5ab5e_Out_0_Vector2 = float2(_Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float, _Property_fbc5815c86c14781ad582c30373e09e9_Out_0_Float);
        float2 _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2;
        Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Vector2_4eedbb6ddec7420db830b4c2c1d5ab5e_Out_0_Vector2, _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2);
        float2 _Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_a81b27dd86034a94abb2a30b48b0192c_New_0_Vector4.xy), _Multiply_2ec6fd7b50d14e64b7b0dd6387ab7cd0_Out_2_Vector2, _Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2);
        float _Property_1d12d5d6305243e58faec8b8aee91847_Out_0_Float = _CausticNoiseScale;
        float _GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_e073157fda1e44ffa0c6157e11230567_Out_2_Vector2, _Property_1d12d5d6305243e58faec8b8aee91847_Out_0_Float, _GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float);
        float _Property_d391d04563db4da1bab793dc2c87b6d7_Out_0_Float = _CausticBlend;
        Bindings_FloatToPolar_d32299cdd298f07439c149bb0b195311_float _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1;
        float2 _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2;
        SG_FloatToPolar_d32299cdd298f07439c149bb0b195311_float(_GradientNoise_881b98095a2740a78e53206b89dcb7ba_Out_2_Float, _Property_d391d04563db4da1bab793dc2c87b6d7_Out_0_Float, _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1, _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2);
        float2 _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2;
        Unity_Add_float2((_Add_e517e9b2449f43119f5e21590a290694_Out_2_Vector4.xy), _FloatToPolar_64fe67688e2c4c6fa4b9f8cab75076f1_OutVector2_1_Vector2, _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2);
        New_0 = _Add_45b1c78e72c64cf0b00d77bc966371b3_Out_2_Vector2;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Maximum_float(float A, float B, out float Out)
        {
            Out = max(A, B);
        }
        
        struct Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float
        {
        float3 TimeParameters;
        };
        
        void SG_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float(float4 _CausticBaseColor, UnityTexture2D _CasusitcBaseTex, float _CausitcBaseStrength, float2 _MaskSpeed, float _MaskScale, float _MaskRange, float _MaskStrength, float2 _UV, Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float IN, out float OutVector1_1, out float4 New_2)
        {
        float _Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float = _MaskRange;
        float _Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float;
        Unity_Subtract_float(0.5, _Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float, _Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float);
        float _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float;
        Unity_Add_float(_Property_71639dbf9c7c43f3bb3a2199aa11fabf_Out_0_Float, 0.5, _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float);
        float2 _Property_4bd9fd7272d34a208a97a1bed01732f1_Out_0_Vector2 = _MaskSpeed;
        float2 _Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2;
        Unity_Divide_float2(_Property_4bd9fd7272d34a208a97a1bed01732f1_Out_0_Vector2, float2(10, 10), _Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2);
        float2 _Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Divide_33f67589dc7f49808a17eb0bcaf16af3_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2);
        float2 _Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2 = _UV;
        float2 _Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2;
        Unity_Add_float2(_Multiply_b2b96b09ea0c4e2688e7e5852804c135_Out_2_Vector2, _Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2, _Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2);
        float _Property_03882e3ee2374621b36aa67eb4b6e6a7_Out_0_Float = _MaskScale;
        float _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float;
        Unity_Divide_float(_Property_03882e3ee2374621b36aa67eb4b6e6a7_Out_0_Float, 16, _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float);
        float _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_a819e7aed6d64ab7ad04f2bcd24501f6_Out_2_Vector2, _Divide_3a102d89a14a40629d7c29b8189fb996_Out_2_Float, _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float);
        float _Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float;
        Unity_Smoothstep_float(_Subtract_9da513dfe0ba49b1a51f67c430c28aa4_Out_2_Float, _Add_0317a57c31204bf4bf770bec93284d65_Out_2_Float, _GradientNoise_6fbf0d5c065f46cfa30c209950caff8b_Out_2_Float, _Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float);
        float _Property_606338ee7850446496b7e64baa713052_Out_0_Float = _MaskStrength;
        float _Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float;
        Unity_Multiply_float_float(_Smoothstep_857c26591f9e4260afd784af9db69441_Out_3_Float, _Property_606338ee7850446496b7e64baa713052_Out_0_Float, _Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float);
        float _Property_4c16863732d749b0b2fe45fbd80ab963_Out_0_Float = _CausitcBaseStrength;
        float4 _Property_353355bab0d040bc9968c896e6ae626c_Out_0_Vector4 = _CausticBaseColor;
        UnityTexture2D _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D = _CasusitcBaseTex;
        float4 _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.tex, _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.samplerstate, _Property_a966ece53b674d38bfb5809270034575_Out_0_Texture2D.GetTransformedUV(_Property_5700da1285cb477c9ff502b34ed27976_Out_0_Vector2) );
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_R_4_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.r;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_G_5_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.g;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_B_6_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.b;
        float _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_A_7_Float = _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4.a;
        float4 _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_353355bab0d040bc9968c896e6ae626c_Out_0_Vector4, _SampleTexture2D_e5d0b71f4c72451784ef2fc13df31ef4_RGBA_0_Vector4, _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4);
        float4 _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4;
        Unity_Multiply_float4_float4((_Property_4c16863732d749b0b2fe45fbd80ab963_Out_0_Float.xxxx), _Multiply_cb1f962dbc674ad0b13de821a0581e2f_Out_2_Vector4, _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4);
        float4 _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4;
        Unity_Multiply_float4_float4((_Multiply_66d438b785744f518d6894dcea5ed2de_Out_2_Float.xxxx), _Multiply_b087e9cc9c494002ad72b467cd04124a_Out_2_Vector4, _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4);
        float _Split_28d235cb3e744c15ad540ea4b55baea8_R_1_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[0];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_G_2_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[1];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_B_3_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[2];
        float _Split_28d235cb3e744c15ad540ea4b55baea8_A_4_Float = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4[3];
        float _Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float;
        Unity_Maximum_float(_Split_28d235cb3e744c15ad540ea4b55baea8_R_1_Float, _Split_28d235cb3e744c15ad540ea4b55baea8_G_2_Float, _Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float);
        float _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float;
        Unity_Maximum_float(_Maximum_092669a0e016497cba89acb577c14b9e_Out_2_Float, _Split_28d235cb3e744c15ad540ea4b55baea8_B_3_Float, _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float);
        OutVector1_1 = _Maximum_d37dc39615784178b026048082d341f8_Out_2_Float;
        New_2 = _Multiply_c086f7170ab54e819ba96a87485431c9_Out_2_Vector4;
        }
        
        struct Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(float _SpeedR, float _SpeedL, float _PixelUnit, float _Scale, float _Strenght, Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float IN, out float New_0)
        {
        float _Property_a57dea734ae842c2910db02cb2e57e28_Out_0_Float = _PixelUnit;
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_8bd8de1107214c50b05661f2893264e1;
        float4 _ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), _Property_a57dea734ae842c2910db02cb2e57e28_Out_0_Float, _ToGrid_8bd8de1107214c50b05661f2893264e1, _ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4);
        float2 _Vector2_3bc84328f110402bb37f382ab0c588a0_Out_0_Vector2 = float2(1, 0);
        float _Property_62c2aa2262a24fe3b9d690b4515a955e_Out_0_Float = _SpeedR;
        float _Float_59aac4a4aaf241ec8ad201b3507f57b5_Out_0_Float = _Property_62c2aa2262a24fe3b9d690b4515a955e_Out_0_Float;
        float2 _Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_3bc84328f110402bb37f382ab0c588a0_Out_0_Vector2, (_Float_59aac4a4aaf241ec8ad201b3507f57b5_Out_0_Float.xx), _Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2);
        float2 _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Multiply_d24e0215201f439893777b91f2858b2c_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2);
        float2 _Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4.xy), _Multiply_527ec8f97f5c47e98a2e4c6b6b9f2817_Out_2_Vector2, _Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2);
        float _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float = _Scale;
        float _GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_6aaa9528bacc4ef5867151636bebdfd8_Out_2_Vector2, _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float, _GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float);
        float2 _Vector2_b3e9026a96064661915e65401c5ef998_Out_0_Vector2 = float2(-1, 0);
        float _Property_5db7cfbaab2847daa7d8b4b583b68d89_Out_0_Float = _SpeedL;
        float _Float_7ee06057e8e549cf8720690a5c06f38a_Out_0_Float = _Property_5db7cfbaab2847daa7d8b4b583b68d89_Out_0_Float;
        float2 _Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Vector2_b3e9026a96064661915e65401c5ef998_Out_0_Vector2, (_Float_7ee06057e8e549cf8720690a5c06f38a_Out_0_Float.xx), _Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2);
        float2 _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2;
        Unity_Multiply_float2_float2(_Multiply_0a5a3c3d10844f2a9d50c5e7f21d5759_Out_2_Vector2, (IN.TimeParameters.x.xx), _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2);
        float2 _Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2;
        Unity_Add_float2((_ToGrid_8bd8de1107214c50b05661f2893264e1_New_0_Vector4.xy), _Multiply_5860a946d9e3446b879d3cbeb844fa60_Out_2_Vector2, _Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2);
        float _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float;
        Unity_GradientNoise_Deterministic_float(_Add_92ad53dd436a4aecb80ba223dedb1fb2_Out_2_Vector2, _Property_37f8b6a19f804396a27866949b95ea01_Out_0_Float, _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float);
        float _Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float;
        Unity_Multiply_float_float(_GradientNoise_dd5901f0bc89419b8a0605c257544cd9_Out_2_Float, _GradientNoise_fa7982bb45424ff88e08acb3058702c7_Out_2_Float, _Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float);
        float _Property_c3cd5c2888684348bdb0dcb89b2d3a55_Out_0_Float = _Strenght;
        float _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_1590d408a4634e5889b0f9f408b3e052_Out_2_Float, _Property_c3cd5c2888684348bdb0dcb89b2d3a55_Out_0_Float, _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float);
        New_0 = _Multiply_f1134b40526d4aada0db0b85a3bdf555_Out_2_Float;
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        struct Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float
        {
        float3 WorldSpacePosition;
        float3 TimeParameters;
        };
        
        void SG_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float(float _FloatingMaskScale, float _FloatingSpeed, float _FloatingScale, float _FloatingStep, float _FloatingStrength, float _AreaAlpha, Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float IN, out float New_0)
        {
        float _Property_768d4c11e1d74c038c611c29a7fe7bee_Out_0_Float = _FloatingStep;
        float _Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float = _FloatingSpeed;
        float _Property_12f8969d6c634c5dbece900f136fc702_Out_0_Float = _FloatingScale;
        Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1;
        _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1.WorldSpacePosition = IN.WorldSpacePosition;
        _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1.TimeParameters = IN.TimeParameters;
        float _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float;
        SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(_Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float, _Property_134e4d5e3ee645fb8637978f3d6e61ee_Out_0_Float, 16, _Property_12f8969d6c634c5dbece900f136fc702_Out_0_Float, 2, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float);
        float _Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float;
        Unity_Step_float(_Property_768d4c11e1d74c038c611c29a7fe7bee_Out_0_Float, _EdgeNoise_5b7462769d084fdf9e5d5c04c4d73fb1_New_0_Float, _Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float);
        float _Property_10ae58a5122440539849cc655abb1e9e_Out_0_Float = _AreaAlpha;
        float _Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float;
        Unity_Multiply_float_float(_Step_87e624fd2a5443e9bc1cf17e4dbf2491_Out_2_Float, _Property_10ae58a5122440539849cc655abb1e9e_Out_0_Float, _Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0;
        float4 _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0, _ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4);
        float _Property_425de77259b74a9088e05b137bbe6009_Out_0_Float = _FloatingSpeed;
        float _Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_425de77259b74a9088e05b137bbe6009_Out_0_Float, _Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float);
        float4 _Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4;
        Unity_Add_float4(_ToGrid_6dbb2b1b0480461582b465e3fed6c8d0_New_0_Vector4, (_Multiply_543b14fc61f14455a3efddee87b868ac_Out_2_Float.xxxx), _Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4);
        float _Property_348d99c06d5346dcaf5fc7679b3d10ce_Out_0_Float = _FloatingMaskScale;
        float _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float((_Add_26bb35a65630484b8aebf39cb4e7ef59_Out_2_Vector4.xy), _Property_348d99c06d5346dcaf5fc7679b3d10ce_Out_0_Float, _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float);
        float _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float;
        Unity_Step_float(0.4, _SimpleNoise_649f9be05e8f4b77afe19121f7b882a4_Out_2_Float, _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float);
        float _Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_23f2857d1c034745b56ba986d4662ef4_Out_2_Float, _Step_3dc5ec81443a4d31bd51376d55dfb075_Out_2_Float, _Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float);
        float _Property_55168c628bf74e3b87cd33712139ad59_Out_0_Float = _FloatingStrength;
        float _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float;
        Unity_Multiply_float_float(_Multiply_a2f0b461f1e34d83a22956f16d8f1dd0_Out_2_Float, _Property_55168c628bf74e3b87cd33712139ad59_Out_0_Float, _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float);
        New_0 = _Multiply_0876a228abd54fd89b9b94fad75330d5_Out_2_Float;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
        {
        Hash_Tchou_2_2_float(UV, UV);
        return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);
        for (int y = -1; y <= 1; y++)
        {
        for (int x = -1; x <= 1; x++)
        {
        float2 lattice = float2(x, y);
        float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
        float d = distance(lattice + offset, f);
        if (d < res.x)
        {
        res = float3(d, offset.x, offset.y);
        Out = res.x;
        Cells = res.y;
        }
        }
        }
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        struct Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float
        {
        float3 WorldSpacePosition;
        float2 NDCPosition;
        float3 TimeParameters;
        };
        
        void SG_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float(float _LiquidBottomSpeed, float _LiquidBottomAngleSpeed, float _LiquidBottomNoiseScale, float _LiquidBottomNoiseStrengh, float2 _PixelCell, float _Scale, float _Distort, Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float IN, out float4 OutVector4_1)
        {
        float4 _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
        float _Split_babc1f5e31a04264ac52c77e08f160df_R_1_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[0];
        float _Split_babc1f5e31a04264ac52c77e08f160df_G_2_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[1];
        float _Split_babc1f5e31a04264ac52c77e08f160df_B_3_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[2];
        float _Split_babc1f5e31a04264ac52c77e08f160df_A_4_Float = _ScreenPosition_10da780e24954afd8e697f0d3de72e11_Out_0_Vector4[3];
        float2 _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2 = _PixelCell;
        float _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float = _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2[0];
        float _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float = _Property_fe12897ce89d4f7b8be9dc211a9c2045_Out_0_Vector2[1];
        float _Split_21288f26c09f404a9991b72c6c6549e5_B_3_Float = 0;
        float _Split_21288f26c09f404a9991b72c6c6549e5_A_4_Float = 0;
        float _Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float;
        Unity_Multiply_float_float(_Split_babc1f5e31a04264ac52c77e08f160df_R_1_Float, _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float, _Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float);
        float _Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float;
        Unity_Divide_float(_Multiply_3448acffa5a94b9a9f1ccb59e4cbf456_Out_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_R_1_Float, _Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float);
        float _Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float;
        Unity_Multiply_float_float(_Split_babc1f5e31a04264ac52c77e08f160df_G_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float, _Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float);
        float _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float;
        Unity_Divide_float(_Multiply_cad7c24c1f3f4c6ca409784bfd49f6b4_Out_2_Float, _Split_21288f26c09f404a9991b72c6c6549e5_G_2_Float, _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float);
        float4 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGBA_4_Vector4;
        float3 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGB_5_Vector3;
        float2 _Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2;
        Unity_Combine_float(_Divide_7268d836e88f4d6cba2fc049c6fdfc99_Out_2_Float, _Divide_c6df7a101ca147a08a6356a1606bc716_Out_2_Float, 0, 0, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGBA_4_Vector4, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RGB_5_Vector3, _Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2);
        float _Property_a65d4b108b87421787112feacdc2ad90_Out_0_Float = _LiquidBottomSpeed;
        float _Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_a65d4b108b87421787112feacdc2ad90_Out_0_Float, _Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_025acfae81ee4923b89c42d5e95e3073;
        float4 _ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_025acfae81ee4923b89c42d5e95e3073, _ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4);
        float _Property_632061910c294796a237dd943f583f7a_Out_0_Float = _Distort;
        float4 _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4;
        Unity_Add_float4(_ToGrid_025acfae81ee4923b89c42d5e95e3073_New_0_Vector4, (_Property_632061910c294796a237dd943f583f7a_Out_0_Float.xxxx), _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4);
        float4 _Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4;
        Unity_Add_float4((_Multiply_738108e4ca294490a219d1dc37cd3fdc_Out_2_Float.xxxx), _Add_fcb78f24abf94c8aa3e460019c71351e_Out_2_Vector4, _Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4);
        float _Property_cd3a650be5e74143bdb6ef87564a6459_Out_0_Float = _LiquidBottomAngleSpeed;
        float _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float;
        Unity_Multiply_float_float(_Property_cd3a650be5e74143bdb6ef87564a6459_Out_0_Float, IN.TimeParameters.x, _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float);
        float _Property_dd1d1cc3b97841399706968720371ee0_Out_0_Float = _LiquidBottomNoiseScale;
        float _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float;
        float _Voronoi_56b538e009a446cd850ace505427cf1b_Cells_4_Float;
        Unity_Voronoi_Deterministic_float((_Add_f4ef250651ca49f3830b4438b9b6b9c4_Out_2_Vector4.xy), _Multiply_81385d0c02f645108de2b81b39bc246f_Out_2_Float, _Property_dd1d1cc3b97841399706968720371ee0_Out_0_Float, _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float, _Voronoi_56b538e009a446cd850ace505427cf1b_Cells_4_Float);
        float _Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float;
        Unity_Step_float(0.5, _Voronoi_56b538e009a446cd850ace505427cf1b_Out_3_Float, _Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float);
        float _Property_953201261ff4463b82ab45ac766060fa_Out_0_Float = _LiquidBottomNoiseStrengh;
        float _Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float;
        Unity_Multiply_float_float(_Step_c67533b1508d4b24b1213ab72d1bb156_Out_2_Float, _Property_953201261ff4463b82ab45ac766060fa_Out_0_Float, _Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float);
        float2 _TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2;
        Unity_TilingAndOffset_float(_Combine_ff0eb431b2c64b5ebe68101f5467c237_RG_6_Vector2, float2 (1, 1), (_Multiply_9b32bdd202ca484eb6efdbd7a0acf3ee_Out_2_Float.xx), _TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2);
        float4 _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).tex, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).samplerstate, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_Texture_1_Texture2D).GetTransformedUV(_TilingAndOffset_b74803493e87428dac4824a60060922d_Out_3_Vector2) );
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_R_4_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.r;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_G_5_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.g;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_B_6_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.b;
        float _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_A_7_Float = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4.a;
        OutVector4_1 = _SampleTexture2D_f7d22e430c9144a0825060bb5943ff29_RGBA_0_Vector4;
        }
        
        struct Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float
        {
        half4 uv0;
        };
        
        void SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(UnityTexture2D _MainTex, Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D = _MainTex;
        float4 _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.tex, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.samplerstate, _Property_2c1011e8ee184cfd8d67d1274632a823_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_R_4_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.r;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_G_5_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.g;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.b;
        float _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float = _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_RGBA_0_Vector4.a;
        float _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_B_6_Float, _SampleTexture2D_3d49c7fd2a89430899620e8b6582bdec_A_7_Float, _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float);
        OutVector1_1 = _Multiply_e51c2198df624e08a14d62ef96afc354_Out_2_Float;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        struct Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float
        {
        float3 WorldSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(float4 _UV, float _WaveSpeed, float _WaveLength, float _WaveHeight, float _WaveShakeSpeed, float _WaveDistort, Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float IN, out float2 OutVector2_1)
        {
        float4 _UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4 = IN.uv0;
        float _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float = _WaveDistort;
        float _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float;
        Unity_Step_float(0.2, _Property_30eeb8b4b3274c888507bda5a4fafbd1_Out_0_Float, _Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float);
        float _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float;
        Unity_OneMinus_float(_Step_b48a7a906f9348e2beb0f6b56f6b13fb_Out_2_Float, _OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float);
        float _Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float = _WaveSpeed;
        float _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float;
        Unity_Multiply_float_float(_Property_9c6c3bba26a443eaaf81c7a39a8465f6_Out_0_Float, IN.TimeParameters.x, _Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float);
        Bindings_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float _ToGrid_ece7a048a124490eb96c1c943b36de7e;
        float4 _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4;
        SG_ToGrid_33402011d5dbc6543a413cde2f5ec28a_float((float4(IN.WorldSpacePosition, 1.0)), 16, _ToGrid_ece7a048a124490eb96c1c943b36de7e, _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4);
        float _Split_ae769ff284874f9a8245510995a1f39d_R_1_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[0];
        float _Split_ae769ff284874f9a8245510995a1f39d_G_2_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[1];
        float _Split_ae769ff284874f9a8245510995a1f39d_B_3_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[2];
        float _Split_ae769ff284874f9a8245510995a1f39d_A_4_Float = _ToGrid_ece7a048a124490eb96c1c943b36de7e_New_0_Vector4[3];
        float _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float = _WaveLength;
        float _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float;
        Unity_Multiply_float_float(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Property_339f1a8361ca4819b0220de8919a3772_Out_0_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float);
        float _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float;
        Unity_Add_float(_Multiply_5cd8bd9ad00f423995ae7443d28dca42_Out_2_Float, _Multiply_20b62021d7ea445e9610c199cd1b7b17_Out_2_Float, _Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float);
        float _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float;
        Unity_Sine_float(_Add_900614a19c8744c5b2d0154e129c61e0_Out_2_Float, _Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float);
        float _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float = _WaveShakeSpeed;
        float _Multiply_70a55d008a294763835173cb84629169_Out_2_Float;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_c1bbaffc85664c79a56dc2cbe04a2b1d_Out_0_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float2 _Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2 = float2(_Split_ae769ff284874f9a8245510995a1f39d_R_1_Float, _Multiply_70a55d008a294763835173cb84629169_Out_2_Float);
        float _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float;
        Unity_SimpleNoise_Deterministic_float(_Vector2_8e17afc2f8094b0989945a8750a12d6b_Out_0_Vector2, 10, _SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float);
        float _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float;
        Unity_Subtract_float(_SimpleNoise_e4cb99e499744f2cae53b7a46bc0de78_Out_2_Float, 0.5, _Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float);
        float _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float;
        Unity_Multiply_float_float(_Subtract_1a7c6b0d98f44a209d0ccfc82f66a6f2_Out_2_Float, 3, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float);
        float _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float;
        Unity_Step_float(_Sine_96908d3705d04a7e88f327be84d4b351_Out_1_Float, _Multiply_2af6199ab98a48ad8c08918d9b8c4164_Out_2_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float);
        float _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float;
        Unity_Minimum_float(_OneMinus_55d25e467b8d4d6bb55c20353451aaed_Out_1_Float, _Step_25438d7ecfbb490c9869a72c7253ec73_Out_2_Float, _Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float);
        float _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float = _WaveHeight;
        float _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float;
        Unity_Multiply_float_float(_Minimum_acc8c0f4d3504cdb8b5eaf8a6f58dca8_Out_2_Float, _Property_7532fa7d72c845dc890cfcd946127336_Out_0_Float, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float);
        float4 _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4;
        float3 _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3;
        float2 _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2;
        Unity_Combine_float(0, _Multiply_2541033809824dcbafbc1d41fe22162e_Out_2_Float, 0, 0, _Combine_22e4697756d74b1ba2033256ce218ef7_RGBA_4_Vector4, _Combine_22e4697756d74b1ba2033256ce218ef7_RGB_5_Vector3, _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2);
        float2 _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        Unity_Add_float2((_UV_df97ffaf33f64edb9809b64445b2069c_Out_0_Vector4.xy), _Combine_22e4697756d74b1ba2033256ce218ef7_RG_6_Vector2, _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2);
        OutVector2_1 = _Add_5f1d348966674f1ca85143bd998cadf6_Out_2_Vector2;
        }
        
        struct Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float
        {
        };
        
        void SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(UnityTexture2D _MainTex, float4 _UV, Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float IN, out float OutVector1_1)
        {
        UnityTexture2D _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D = _MainTex;
        float4 _Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4 = _UV;
        float4 _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.tex, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.samplerstate, _Property_640da21b8f044c8f9bc3ba1538332044_Out_0_Texture2D.GetTransformedUV((_Property_8fd28066cfd34587855dcb6f6a72dfd8_Out_0_Vector4.xy)) );
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_R_4_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.r;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.g;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_B_6_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.b;
        float _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float = _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_RGBA_0_Vector4.a;
        float _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        Unity_Multiply_float_float(_SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_G_5_Float, _SampleTexture2D_d59df1cbb4aa41f6bcae306583b7b960_A_7_Float, _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float);
        OutVector1_1 = _Multiply_be2b07f63afc484f8b95320b92857134_Out_2_Float;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4 = _DistortTran;
            UnityTexture2D _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_DistortTex);
            float _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float = _DistortUVStrength;
            float4 _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4 = _DistortColor;
            Bindings_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float _WaterDistort_535e0977d51441b3927c7ec02959bc97;
            _WaterDistort_535e0977d51441b3927c7ec02959bc97.WorldSpacePosition = IN.WorldSpacePosition;
            float4 _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4;
            float _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float;
            SG_WaterDistort_5c76b77f167290945a5e21d4c351e0ce_float(_Property_d248bbeab3a24909ac1b78b2df2de40e_Out_0_Vector4, _Property_32978ef6bd724fff92df74f03bac9c0c_Out_0_Texture2D, _Property_8d9c099bf66c4cf8b4c18c696a3b3be9_Out_0_Float, _Property_2c763cf65c2b40059e4ba332731c772c_Out_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float);
            float4 _Property_c07fc1928f0146aab38ffc698890428e_Out_0_Vector4 = _CausticBaseColor;
            UnityTexture2D _Property_23c78aa0988448d193906bb14bcbd874_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_CausticBaseTex);
            float _Property_0919388d90484df7b47156bdff8991e4_Out_0_Float = _CausticBaseStrength;
            float2 _Property_3df02cd1b2b54257a5f27cbedaa58dc5_Out_0_Vector2 = _MaskSpeed;
            float _Property_ed5a798690254c86b56f751c75170330_Out_0_Float = _MaskScale;
            float _Property_0c3340ffe9b94bc882f54dbc156c47d0_Out_0_Float = _MaskRange;
            float _Property_ce5b21ef3eaf4b5bb7954300c744b09e_Out_0_Float = _MaskStrength;
            float _Property_40b8633fc13d4ee997a5c69df3d82f87_Out_0_Float = _CausticSpeed;
            float _Property_5e6fecce44e247f5a33ac030c2ad9e21_Out_0_Float = _CausitcBlend;
            float _Property_d5308f1b3b514b8da3799eb0f61a694d_Out_0_Float = _CausticScale;
            float _Property_5c2585358f944caa8d34cf98b5694651_Out_0_Float = _CausticNoiseScale;
            Bindings_UVCaustic_7660615f0780a3a47ab0e632916466ff_float _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb;
            _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb.WorldSpacePosition = IN.WorldSpacePosition;
            _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb.TimeParameters = IN.TimeParameters;
            float2 _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2;
            SG_UVCaustic_7660615f0780a3a47ab0e632916466ff_float(_Property_40b8633fc13d4ee997a5c69df3d82f87_Out_0_Float, _Property_5e6fecce44e247f5a33ac030c2ad9e21_Out_0_Float, _Property_d5308f1b3b514b8da3799eb0f61a694d_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _Property_5c2585358f944caa8d34cf98b5694651_Out_0_Float, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2);
            Bindings_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2;
            _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2.TimeParameters = IN.TimeParameters;
            float _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float;
            float4 _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4;
            SG_WaterCaustic_279f7ab7eac53bd4e98150ab2479bfa4_float(_Property_c07fc1928f0146aab38ffc698890428e_Out_0_Vector4, _Property_23c78aa0988448d193906bb14bcbd874_Out_0_Texture2D, _Property_0919388d90484df7b47156bdff8991e4_Out_0_Float, _Property_3df02cd1b2b54257a5f27cbedaa58dc5_Out_0_Vector2, _Property_ed5a798690254c86b56f751c75170330_Out_0_Float, _Property_0c3340ffe9b94bc882f54dbc156c47d0_Out_0_Float, _Property_ce5b21ef3eaf4b5bb7954300c744b09e_Out_0_Float, _UVCaustic_ebe3e0d80a534ea595838f273b1f2eeb_New_0_Vector2, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4);
            float _Property_2470e4696d544eb9bdbd493f89e33be9_Out_0_Float = _FloatingMaskScale;
            float _Property_edc40ab4725a443989cbdc4861756e9b_Out_0_Float = _FloatingSpeed;
            float _Property_27f3ddc40e54460ca0d0c2983d883803_Out_0_Float = _FloatingScale;
            float _Property_f76b3b80b7dd403ca709d91f624d7dcd_Out_0_Float = _FloatingStep;
            float _Property_219c9e59f3f24b30a2c77917c412b0b6_Out_0_Float = _FloatingStrength;
            Bindings_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float _WaterFloating_12c80082bf2c4682a4f593c5690670d5;
            _WaterFloating_12c80082bf2c4682a4f593c5690670d5.WorldSpacePosition = IN.WorldSpacePosition;
            _WaterFloating_12c80082bf2c4682a4f593c5690670d5.TimeParameters = IN.TimeParameters;
            float _WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float;
            SG_WaterFloating_250fe7c839d5c0d4c8b0034a3375edcf_float(_Property_2470e4696d544eb9bdbd493f89e33be9_Out_0_Float, _Property_edc40ab4725a443989cbdc4861756e9b_Out_0_Float, _Property_27f3ddc40e54460ca0d0c2983d883803_Out_0_Float, _Property_f76b3b80b7dd403ca709d91f624d7dcd_Out_0_Float, _Property_219c9e59f3f24b30a2c77917c412b0b6_Out_0_Float, _WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_OutVector1_1_Float, _WaterFloating_12c80082bf2c4682a4f593c5690670d5, _WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float);
            float4 _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4;
            Unity_Add_float4(_WaterCaustic_2efacb592e7c4762b9b9c6042a420be2_New_2_Vector4, (_WaterFloating_12c80082bf2c4682a4f593c5690670d5_New_0_Float.xxxx), _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4);
            float4 _Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4;
            Unity_Add_float4(_WaterDistort_535e0977d51441b3927c7ec02959bc97_Color_0_Vector4, _Add_6cdc20b7a543437aafaf77dcbcd25241_Out_2_Vector4, _Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4);
            float _Property_16a4ee35313f48afacbb28383a2d74ca_Out_0_Float = _BottomSpeed;
            float _Property_bfa36bdf8a3942399424d5b51d83dd06_Out_0_Float = _BottomAngleSpeed;
            float _Property_1a75e1efb86c457087c5f5bd2bab3e11_Out_0_Float = _BottomNoiseScale;
            float _Property_e2d3edae3a34460784d651b854f86f5e_Out_0_Float = _BottomNoiseStrengh;
            float2 _Property_801c129985d747aba3e7481ad87dd0b8_Out_0_Vector2 = _BottomPixelCount;
            float _Property_9db9f5b047a64b2483dd9c946159309b_Out_0_Float = _BottomScale;
            Bindings_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float _CatchLiquidBottom_f94162af491145679d1c11914ad36b53;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.WorldSpacePosition = IN.WorldSpacePosition;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.NDCPosition = IN.NDCPosition;
            _CatchLiquidBottom_f94162af491145679d1c11914ad36b53.TimeParameters = IN.TimeParameters;
            float4 _CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4;
            SG_CatchLiquidBottom_cfa79c485e9acab46a97811ebfc7be40_float(_Property_16a4ee35313f48afacbb28383a2d74ca_Out_0_Float, _Property_bfa36bdf8a3942399424d5b51d83dd06_Out_0_Float, _Property_1a75e1efb86c457087c5f5bd2bab3e11_Out_0_Float, _Property_e2d3edae3a34460784d651b854f86f5e_Out_0_Float, _Property_801c129985d747aba3e7481ad87dd0b8_Out_0_Vector2, _Property_9db9f5b047a64b2483dd9c946159309b_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _CatchLiquidBottom_f94162af491145679d1c11914ad36b53, _CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4);
            float _Property_70d5267ba184430a8fed1cd11db83493_Out_0_Float = _BottomStrengh;
            float4 _Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4;
            Unity_Multiply_float4_float4(_CatchLiquidBottom_f94162af491145679d1c11914ad36b53_OutVector4_1_Vector4, (_Property_70d5267ba184430a8fed1cd11db83493_Out_0_Float.xxxx), _Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4);
            UnityTexture2D _Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            Bindings_RGBCutB_a563cbb182c347949a3c3fae898a2146_float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3;
            _RGBCutB_e130f7613eb140988bc99e39b3f57fb3.uv0 = IN.uv0;
            float _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float;
            SG_RGBCutB_a563cbb182c347949a3c3fae898a2146_float(_Property_0145df2a04b34d7dae74a3d3f08aee12_Out_0_Texture2D, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3, _RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float);
            UnityTexture2D _Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4 = IN.uv0;
            float _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float = _EdgeWaveSpeed;
            float _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float = _EdgeWaveLength;
            float _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float = _EdgeWaveHeight;
            float _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float = _EdgeWaveShakeSpeed;
            Bindings_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.WorldSpacePosition = IN.WorldSpacePosition;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.uv0 = IN.uv0;
            _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8.TimeParameters = IN.TimeParameters;
            float2 _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2;
            SG_UVSineWave_f3b53f26e323d254e81f4d0ba8891831_float(_UV_e0a559ae2392453b9d35649e46d6c63c_Out_0_Vector4, _Property_c750c47770c24b5399971e20c2e0849f_Out_0_Float, _Property_accd1bcc544f42049bfaad20772861bf_Out_0_Float, _Property_3907c96da78b4729b816b3b6282b4950_Out_0_Float, _Property_f980f57007af49409c7806b1801c2ff7_Out_0_Float, _WaterDistort_535e0977d51441b3927c7ec02959bc97_Distort_1_Float, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8, _UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2);
            Bindings_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852;
            float _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float;
            SG_RGBCutG_c0e1febe33cc93f46afc4996555c2ac3_float(_Property_bea6a441512a41f9bfe02c610b591398_Out_0_Texture2D, (float4(_UVSineWave_96cad1fd321045aabe8f42273cdaf5f8_OutVector2_1_Vector2, 0.0, 1.0)), _RGBCutG_62abc9b5809b4b07a9fae459f7e96852, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float);
            float _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float;
            Unity_Step_float(0.1, _RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float);
            float _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float;
            Unity_Subtract_float(_RGBCutB_e130f7613eb140988bc99e39b3f57fb3_OutVector1_1_Float, _Step_204d9944c1bf41dc807b44fc3c4d88d4_Out_2_Float, _Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float);
            float _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float;
            Unity_Clamp_float(_Subtract_5c8d08afa67a41a7bb511056d8378e42_Out_2_Float, 0, 1, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float);
            float4 _Property_e7e806c9a8a64fd59d0a70cd6598a43b_Out_0_Vector4 = _BaseColor;
            float4 _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float.xxxx), _Property_e7e806c9a8a64fd59d0a70cd6598a43b_Out_0_Vector4, _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4);
            float _Property_d3385627294b41949fa94f7d49df2d80_Out_0_Float = _BottomAlpha;
            float4 _Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4;
            Unity_Lerp_float4(_Multiply_d425ad0dd5214ab99908d94f8e2dca59_Out_2_Vector4, _Multiply_f8c6ce79aac04746b5af06f27df36c5b_Out_2_Vector4, (_Property_d3385627294b41949fa94f7d49df2d80_Out_0_Float.xxxx), _Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4);
            float4 _Property_5156b57acc9f49efa11c95a30950d9a2_Out_0_Vector4 = _EdgeWaveColor;
            float4 _Property_81b01f5ed69941dd8601ee9432af1a5f_Out_0_Vector4 = _EdgeWaveHighLightColor;
            float _Property_7a60d176b9994138bfe5bd4b0415dade_Out_0_Float = _EdgeWaveColorNoiseScale;
            Bindings_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7;
            _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7.WorldSpacePosition = IN.WorldSpacePosition;
            _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7.TimeParameters = IN.TimeParameters;
            float _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float;
            SG_EdgeNoise_1143bc248c964e64da9c0f148bfabab1_float(1, 1, 16, _Property_7a60d176b9994138bfe5bd4b0415dade_Out_0_Float, 0.5, _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7, _EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float);
            float _Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float;
            Unity_Multiply_float_float(_EdgeNoise_2b0379252ee4445a8a6a09a8739205c7_New_0_Float, 10, _Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float);
            float _Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float;
            Unity_Floor_float(_Multiply_6023569de59c48fc892d297f1f50864f_Out_2_Float, _Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float);
            float _Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float;
            Unity_Divide_float(_Floor_8840a8adac0b4fe3be8a7fbf82db2f93_Out_1_Float, 5, _Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float);
            float4 _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_81b01f5ed69941dd8601ee9432af1a5f_Out_0_Vector4, (_Divide_f54e626392514ab1a0020242466ad49d_Out_2_Float.xxxx), _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4);
            float4 _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4;
            Unity_Add_float4(_Property_5156b57acc9f49efa11c95a30950d9a2_Out_0_Vector4, _Multiply_776404a21112440faa3db950c2e0cf19_Out_2_Vector4, _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4);
            float4 _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4;
            Unity_Multiply_float4_float4((_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float.xxxx), _Add_9701b8cae6f94d828e3fbc086507f545_Out_2_Vector4, _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4);
            float4 _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4;
            Unity_Add_float4(_Lerp_c54c54bcf9db482d8daf3064f1bc44f9_Out_3_Vector4, _Multiply_e1993ffa4eb146d68c647c0d93177ac9_Out_2_Vector4, _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4);
            float4 _Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4;
            Unity_Add_float4(_Add_f526c2a147834a5c9c24eda06f8e5a4f_Out_2_Vector4, _Add_3c1c3a49b9f64ec586e22a7dcb8255bb_Out_2_Vector4, _Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4);
            float _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            Unity_Add_float(_RGBCutG_62abc9b5809b4b07a9fae459f7e96852_OutVector1_1_Float, _Clamp_d3b19c5f8a3049e7a987acaefa14ee66_Out_3_Float, _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float);
            surface.BaseColor = (_Add_7ab47c269b7b4ac9b0c4ec6411da56dc_Out_2_Vector4.xyz);
            surface.Alpha = _Add_ee95893b183342e9b9ccbbd378d2ac6d_Out_2_Float;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}