Shader "Dorian/S_TexturePan"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color",color) = (1,1,1,1)
        
        _Movement("Movement", vector) = (0,0,0,0)
        _NoiseAlphaEffect("NoiseAlphaEffect", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 col : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 col : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _Movement;
            float _NoiseAlphaEffect;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv +=  frac(_Time.y *_Movement.xy);
                o.col = v.col;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                float alpha = saturate(i.col.a - (col.r*2-1)*_NoiseAlphaEffect);
                clip(alpha-.5);

                float4 final = float4(i.col.rgb ,1);
                
                return final;
            }
            ENDCG
        }
    }
}
