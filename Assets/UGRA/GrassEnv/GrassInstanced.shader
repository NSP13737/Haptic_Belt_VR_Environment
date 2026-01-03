
Shader "Custom/GrassInstanced"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.57, 0.84, 0.32, 1.0)
        _BottomColor ("Bottom Color", Color) = (0.06, 0.37, 0.07, 1.0)
        _WindDistortionMap ("Wind Noise", 2D) = "white" {}
        _WindFrequency ("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
        _WindStrength ("Wind Strength", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        // Consider turning off culling if your blades can flip (negative scales):
        // Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            // Instancing + XR stereo variants
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID       // required for instancing
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float4 color : COLOR;

                UNITY_VERTEX_INPUT_INSTANCE_ID       // pass instance ID to frag
                UNITY_VERTEX_OUTPUT_STEREO           // stereo output payload
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;
            sampler2D _WindDistortionMap;
            float4 _WindFrequency;
            float  _WindStrength;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Per-instance world position for wind sampling
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Sample wind noise (vertex texture fetch needs target 3.0)
                // Use UV.y as "height" factor so the root remains planted
                float windSample = tex2Dlod(
                    _WindDistortionMap,
                    float4(worldPos.xz * _WindFrequency.xy + _Time.y * 0.1, 0, 0)
                ).r;

                float sway = windSample * _WindStrength * v.uv.y;

                // Apply sway in local space (x-axis bend)
                float4 vtx = v.vertex;
                vtx.x += sway;

                // Stereo-correct object->clip transform
                o.pos = UnityObjectToClipPos(vtx);
                o.uv  = v.uv;

                // Vertical gradient (bottom to top)
                o.color = lerp(_BottomColor, _TopColor, v.uv.y);

                // Ensure per-eye index is set for SPI
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                return i.color;
            }
            ENDCG
        }
    }
}
