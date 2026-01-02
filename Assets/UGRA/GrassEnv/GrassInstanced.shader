Shader "Custom/GrassInstanced"
{
    Properties
    {
        _TopColor("Top Color", Color) = (0.57, 0.84, 0.32, 1.0)
        _BottomColor("Bottom Color", Color) = (0.06, 0.37, 0.07, 1.0)
        _WindDistortionMap("Wind Noise", 2D) = "white" {}
        _WindFrequency("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
        _WindStrength("Wind Strength", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing // This is the magic line
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Required for instancing
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;
            sampler2D _WindDistortionMap;
            float4 _WindFrequency;
            float _WindStrength;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                // 1. Get world position for wind sampling
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                // 2. Simple Wind Animation
                // We use the vertex color or UV.y to make sure the root (0) doesn't move
                float windSample = tex2Dlod(_WindDistortionMap, float4(worldPos.xz * _WindFrequency.xy + _Time.y * 0.1, 0, 0)).r;
                float sway = windSample * _WindStrength * v.uv.y;
                v.vertex.x += sway;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // 3. Color Gradients (Bottom to Top)
                o.color = lerp(_BottomColor, _TopColor, v.uv.y);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                return i.color;
            }
            ENDCG
        }
    }
}