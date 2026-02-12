Shader "Custom/BuildingObj_Wall"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _VisualHeight ("Visual Height (Stretch)", Float) = 5.0

        _BaseY ("Base Y (Local Ground)", Float) = 0.0
        _SpriteHeight ("Sprite Height (Local)", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "DisableBatching"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode"="Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float2 lightUV : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _VisualHeight;
                float _BaseY;
                float _SpriteHeight;
            CBUFFER_END

            sampler2D _MainTex;

            TEXTURE2D(_ShapeLightTexture0);
            SAMPLER(sampler_ShapeLightTexture0);

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                float3 originalPos = input.positionOS.xyz;

                // ---- 计算该顶点在墙体高度中的比例 t ----
                float height = max(_SpriteHeight, 1e-6);
                float t = (originalPos.y - _BaseY) / height;

                // 限制在 0~1，防止底部以下或顶部以上的点乱飘
                t = saturate(t);

                // ---- 拉伸：墙根不动，墙顶加满 ----
                float3 stretchedPos = originalPos;
                stretchedPos.y += t * _VisualHeight;

                output.positionCS = TransformObjectToHClip(stretchedPos);

                // ---- 光照采样：永远用墙根位置 ----
                float3 groundPos = originalPos;
                groundPos.y = _BaseY;

                float4 groundCS = TransformObjectToHClip(groundPos);
                float4 groundSP = ComputeScreenPos(groundCS);
                output.lightUV = groundSP.xy / (groundSP.w + 1e-6);

                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, input.uv) * input.color;

                half4 lightColor = SAMPLE_TEXTURE2D(
                    _ShapeLightTexture0,
                    sampler_ShapeLightTexture0,
                    input.lightUV
                );

                return half4(texColor.rgb * lightColor.rgb, texColor.a);
            }

            ENDHLSL
        }
    }}
