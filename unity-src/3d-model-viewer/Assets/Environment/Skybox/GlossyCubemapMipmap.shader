Shader "Custom/GlossyCubemapMipmap"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        [Gamma] _Exposure ("Exposure", Range(0,8)) = 1.0
        _Rotation ("Rotation", Range(0,360)) = 0
        [NoScaleOffset] _Tex ("Cubemap (HDR)", CUBE) = "grey" {}
        _MipLevel ("Mipmap Level", Range(0,7)) = 0
    }
    SubShader
    {
        Tags { "QUEUE"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _Tex;
            float4 _Tint;
            float _Exposure;
            float _Rotation;
            float _MipLevel;
            float4 _Tex_HDR;

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Standard Unity Skybox/Cubemap mapping mit Rotation
                float3 dir = v.vertex.xyz;
                float theta = radians(_Rotation);
                float cosTheta = cos(theta);
                float sinTheta = sin(theta);
                float3 rotatedDir = float3(
                    cosTheta * dir.x - sinTheta * dir.z,
                    dir.y,
                    sinTheta * dir.x + cosTheta * dir.z
                );
                o.texcoord = rotatedDir;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir = normalize(i.texcoord);

                // Cubemap mit explizitem Mipmap-Level abtasten
                fixed4 tex = texCUBElod(_Tex, float4(dir, _MipLevel));

                // HDR Decoding wie im Unity-Original
                float3 c = DecodeHDR(tex, _Tex_HDR);
                c *= _Tint.rgb * _Exposure;
                c *= 4.5947938; // Unity intern: 0.5/0.11

                return fixed4(c, 1.0);
            }

            // Unity HDR Decoding Funktion (wie im Original)
            float3 DecodeHDR(fixed4 data, float4 decodeInstructions)
            {
                float alpha = data.a;
                float3 rgb = data.rgb;
                float decodeExp = decodeInstructions.w;
                float decodeScale = decodeInstructions.x;
                float decodeBias = decodeInstructions.y;
                float decodeMult = decodeInstructions.z;

                float3 res = rgb * (decodeScale * exp2(decodeBias * (alpha - 1.0)));
                return res;
            }
            ENDCG
        }
    }
    Fallback Off
}
