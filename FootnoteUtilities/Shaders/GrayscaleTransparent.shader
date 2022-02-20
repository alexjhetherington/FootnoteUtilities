Shader "GrayscaleTransparent" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1,1,1,1)
  }
  SubShader {
    GrabPass { "_BackgroundTexture" }
  
    Pass {
      Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
      ZWrite Off
      ZTest Off
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"
      sampler2D _BackgroundTexture;
      sampler2D _MainTex;
      fixed4 _MainTex_ST;
      struct v2f {
        fixed4 vertex : SV_POSITION;
        fixed4 grabUV : TEXCOORD1;
      };
      struct appdata {
        fixed4 vertex : POSITION;
      };
      v2f vert (appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.grabUV = ComputeGrabScreenPos(o.vertex);
        return o;
      }
      fixed4 frag(v2f i) : SV_Target {
        fixed4 bgc = tex2Dproj(_BackgroundTexture, i.grabUV);
        return (bgc.r + bgc.g + bgc.b) / 3;
      }
      ENDCG
    }
  }
  FallBack Off
}