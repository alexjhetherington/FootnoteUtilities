// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PortalCard"
{
	Properties
	{
		_AnglePower("Angle Power", Range( 1 , 200)) = 1
		_Colour("Colour", Color) = (1,0,0,0)
		_FarDepth("Far Depth", Range( 0 , 1000)) = 1
		_FadeMaxDistance("Fade Max Distance", Range( 0 , 500)) = 1
		_FadeCloseDistance("Fade Close Distance", Range( 0 , 20)) = 0
		_AlphaTexture("Alpha Texture", 2D) = "white" {}
		_EdgeGradient1("Edge Gradient 1", Range( 2 , 15)) = 15
		_EdgeGradient2("Edge Gradient 2", Range( 2 , 15)) = 15
		_AngleMinOpacity("Angle Min Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			float3 worldNormal;
			float4 screenPos;
			float eyeDepth;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Colour;
		uniform sampler2D _AlphaTexture;
		uniform float4 _AlphaTexture_ST;
		uniform float _AnglePower;
		uniform float _AngleMinOpacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FarDepth;
		uniform float _FadeCloseDistance;
		uniform float _FadeMaxDistance;
		uniform float _EdgeGradient1;
		uniform float _EdgeGradient2;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_AlphaTexture = i.uv_texcoord * _AlphaTexture_ST.xy + _AlphaTexture_ST.zw;
			float4 tex2DNode38 = tex2D( _AlphaTexture, uv_AlphaTexture );
			o.Emission = min( _Colour , tex2DNode38 ).rgb;
			float3 ase_worldNormal = i.worldNormal;
			float dotResult11 = dot( i.viewDir , ase_worldNormal );
			float clampResult8 = clamp( max( ( ( abs( dotResult11 ) + -0.2 ) * _AnglePower ) , _AngleMinOpacity ) , 0.0 , 1.0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth23 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth23 = abs( ( screenDepth23 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FarDepth ) );
			float clampResult37 = clamp( distanceDepth23 , 0.0 , 1.0 );
			float clampResult31 = clamp( ( ( i.eyeDepth - _FadeCloseDistance ) / _FadeMaxDistance ) , 0.0 , 1.0 );
			float2 appendResult85 = (float2(_EdgeGradient1 , _EdgeGradient2));
			float2 clampResult80 = clamp( ( ( i.uv_texcoord * appendResult85 ) * ( appendResult85 * ( 1.0 - i.uv_texcoord ) ) ) , float2( 0,0 ) , float2( 1,1 ) );
			float2 break81 = clampResult80;
			o.Alpha = ( tex2DNode38.a * clampResult8 * clampResult37 * clampResult31 * i.vertexColor.a * ( break81.x * break81.y ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
152;130;1452;899;3711.329;261.6603;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;83;-1456.68,795.851;Inherit;False;Property;_EdgeGradient1;Edge Gradient 1;6;0;Create;True;0;0;0;False;0;False;15;0;2;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;-1834.949,803.3378;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;84;-1462.15,871.282;Inherit;False;Property;_EdgeGradient2;Edge Gradient 2;7;0;Create;True;0;0;0;False;0;False;15;2;2;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;9;-3321.448,-174.2074;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;10;-3294.148,-20.8082;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;85;-1089.263,836.0384;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;70;-1140.218,968.1065;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;11;-3110.848,-153.4073;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-812.2179,940.1065;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-788.2179,727.1065;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;12;-2964.648,-154.4076;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3074.161,-51.27795;Inherit;False;Constant;_AngleBase;Angle Base;2;0;Create;True;0;0;0;False;0;False;-0.2;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-492.2178,882.1065;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-3337.645,878.6204;Inherit;False;Property;_FadeCloseDistance;Fade Close Distance;4;0;Create;True;0;0;0;False;0;False;0;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-3070.248,38.99249;Inherit;False;Property;_AnglePower;Angle Power;0;0;Create;True;0;0;0;False;0;False;1;1;1;200;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-2831.162,-168.2779;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SurfaceDepthNode;28;-3339.583,774.316;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-2738.846,60.90118;Inherit;False;Property;_AngleMinOpacity;Angle Min Opacity;8;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-3197.831,962.1136;Inherit;False;Property;_FadeMaxDistance;Fade Max Distance;3;0;Create;True;0;0;0;False;0;False;1;1;0;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-3281.938,398.0315;Inherit;False;Property;_FarDepth;Far Depth;2;0;Create;True;0;0;0;False;0;False;1;0;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;80;-254.8577,858.9281;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2739.162,-44.27794;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;-3004.645,817.6204;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;86;-2423.846,-45.09882;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;81;43.61108,839.4183;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleDivideOpNode;32;-2813.905,885.1447;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;39;-1493.946,-363.2204;Inherit;True;Property;_AlphaTexture;Alpha Texture;5;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DepthFade;23;-2960.486,395.7527;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;31;-2643.053,866.1465;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-994.6697,-396.4245;Inherit;False;Property;_Colour;Colour;1;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;37;-2616.456,362.0817;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;40;-848.4831,233.538;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;195.7112,823.8184;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;-2292.241,-43.51835;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-1262.827,-195.9785;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-456.7423,154.0246;Inherit;False;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;44;-739.5916,-221.5534;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;0,0;Float;False;True;-1;3;ASEMaterialInspector;0;0;Unlit;PortalCard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;85;0;83;0
WireConnection;85;1;84;0
WireConnection;70;0;64;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;72;0;85;0
WireConnection;72;1;70;0
WireConnection;74;0;64;0
WireConnection;74;1;85;0
WireConnection;12;0;11;0
WireConnection;76;0;74;0
WireConnection;76;1;72;0
WireConnection;19;0;12;0
WireConnection;19;1;18;0
WireConnection;80;0;76;0
WireConnection;20;0;19;0
WireConnection;20;1;13;0
WireConnection;35;0;28;0
WireConnection;35;1;33;0
WireConnection;86;0;20;0
WireConnection;86;1;87;0
WireConnection;81;0;80;0
WireConnection;32;0;35;0
WireConnection;32;1;29;0
WireConnection;23;0;27;0
WireConnection;31;0;32;0
WireConnection;37;0;23;0
WireConnection;82;0;81;0
WireConnection;82;1;81;1
WireConnection;8;0;86;0
WireConnection;38;0;39;0
WireConnection;36;0;38;4
WireConnection;36;1;8;0
WireConnection;36;2;37;0
WireConnection;36;3;31;0
WireConnection;36;4;40;4
WireConnection;36;5;82;0
WireConnection;44;0;2;0
WireConnection;44;1;38;0
WireConnection;1;2;44;0
WireConnection;1;9;36;0
ASEEND*/
//CHKSM=45FAC04065508AFD003A9073B33454B4449DB7F4