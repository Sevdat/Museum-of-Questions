Shader "Custom/RenderFromGPU"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            StructuredBuffer<float3> Vertices;

            struct appdata
            {
                uint vertexID : SV_VertexID;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;

                // Get vertex position from the buffer
                float3 vertex = Vertices[v.vertexID];
                o.pos = UnityObjectToClipPos(float4(vertex, 1.0));

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(1, 1, 1, 1); // White color
            }
            ENDCG
        }
    }
}