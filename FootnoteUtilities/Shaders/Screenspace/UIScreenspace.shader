﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/ScreenSpace"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

		_ScreenTex("Screen tex", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

		_Scaling("Scaling", Float) = 1
		_ScrollSpeed("Scroll speed", Vector) = (0,0,0,0)
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
     
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
 
        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
         
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };
         
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
 
            bool _UseClipRect;
            float4 _ClipRect;
 
            bool _UseAlphaClip;


			float _Scaling;

 
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
 
                OUT.texcoord = IN.texcoord;
             
                #ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
                #endif
             
                OUT.color = IN.color * _Color;
                return OUT;
            }
 
            sampler2D _MainTex;
			sampler2D _ScreenTex;
			float4 _ScrollSpeed;
 
            fixed4 frag(v2f IN) : SV_Target
            {
				fixed2 coord = IN.texcoord;

				float4 vert = IN.vertex;

                half4 color = (tex2D(_MainTex, coord) + _TextureSampleAdd) * IN.color;

				
				vert.y += _Time.w * _ScrollSpeed.y;
				vert.x += _Time.w * _ScrollSpeed.x;



				color *= tex2D(_ScreenTex, vert.xy * _Scaling);
 
                if (_UseClipRect)
                    color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
             

                return color;
            }
        ENDCG
        }
    }
}
 