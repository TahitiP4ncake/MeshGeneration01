Shader "Dorian/S_VertexColor"
{
    Properties
    {
        _Color ("Color",color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 col : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 col : COLOR;
            };

           float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.col = v.col;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 col = i.col * _Color;
                float4 final = float4(col.rgb,1);
                return final;
            }
            ENDCG
        }
    }
}
