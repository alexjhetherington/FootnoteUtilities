Shader "Custom/ScreenspaceObject"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _PatternTex("Pattern", 2D) = "white" {}
        _Scaling("Scaling", Float) = 0.5
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Unlit fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _PatternTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _Scaling;

        fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;

            c.rgb = s.Albedo;
            c.a = s.Alpha;

            //atten = step(.1,atten) * .5;
            c.rgb *= atten;

            return c * _LightColor0;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float2 coords = IN.screenPos.xy / IN.screenPos.w;
            coords.y *= _ScreenParams.y / _ScreenParams.x;

            fixed4 patternColor = tex2D(_PatternTex, coords * _Scaling);

            c *= patternColor;

            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
