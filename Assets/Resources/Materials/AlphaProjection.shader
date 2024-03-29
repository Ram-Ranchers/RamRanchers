Shader "Custom/AlphaProjection"
{
    Properties
    {
        _FullTexture("Full Texture", 2D) = "white" {}
        _SemiTexture("Semi Texture", 2D) = "white" {}
        _SemiOpacity("Semi Opacity", float) = 0.5
        _Colour("Colour", Color) = (0, 0, 0, 0)
    }
    Subshader
    {
        Tags{"Queue"="Transparent+100"}
        Pass 
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 
            ZTest Equal 
            
            CGPROGRAM
            #pragma exclude_renderers d3d11 gles
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4x4 unity_Projector;
            sampler2D _FullTexture;
            sampler2D _SemiTexture;
            fixed4 _Colour;
            float _SemiOpacity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul(unity_Projector, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 UV = i.uv;

                float aFull = tex2Dproj(_FullTexture, UV).a;
                float aSemi = tex2Dproj(_SemiTexture, UV).a;

                float a = aFull + _SemiOpacity * aSemi;

                _Colour.a = max(0, _Colour.a - a);
                return _Colour;
            }
            ENDCG
        }
    }
}
