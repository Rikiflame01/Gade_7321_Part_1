Shader "Custom/VertexColorShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Float) = 0.1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Outline Pass
        Pass {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite Off
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "UnityCG.cginc"

            float _OutlineThickness;
            fixed4 _OutlineColor;

            struct appdataOutline {
                float4 vertex : POSITION;
            };

            struct v2fOutline {
                float4 pos : SV_POSITION;
            };

            v2fOutline vertOutline(appdataOutline v) {
                v2fOutline o;
                // Correctly transform the vertex position from object space to world space
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 norm = UnityObjectToWorldNormal(v.vertex.xyz);
                // Push the vertex along the normal by the outline thickness
                posWorld.xyz += norm * _OutlineThickness;
                o.pos = UnityWorldToClipPos(posWorld);
                return o;
            }

            fixed4 fragOutline(v2fOutline i) : SV_Target {
                return _OutlineColor;
            }
            ENDCG
        }

        // Main Pass
        Pass {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            Cull Back
            ZWrite On
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}
