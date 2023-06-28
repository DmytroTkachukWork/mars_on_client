// Made with Amplify Shader Editor v1.9.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/PlanetShaderAmplify"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Clouds("Clouds", 2D) = "black" {}
		_CloudsColor("CloudsColor", Color) = (1,1,1,0)
		_Normal("Normal", 2D) = "bump" {}
		_NormalPower("NormalPower", Float) = 1
		_CloudsSpeed("CloudsSpeed", Vector) = (0,0,0,0)
		[NoScaleOffset]_SunsetRamp("SunsetRamp", 2D) = "white" {}
		_Atmosphere_Intensity("Atmosphere_Intensity", Range( 0 , 10)) = 1
		_Atmosphere_Power("Atmosphere_Power", Range( 0 , 10)) = 1
		_Atmosphere_Color("Atmosphere_Color", Color) = (1,0.5582122,0.3820755,0)
		_Ambiant_Light_Colour("Ambiant_Light_Colour", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float _NormalPower;
		uniform sampler2D _SunsetRamp;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _CloudsColor;
		uniform sampler2D _Clouds;
		uniform float2 _CloudsSpeed;
		uniform float4 _Atmosphere_Color;
		uniform float _Atmosphere_Intensity;
		uniform float _Atmosphere_Power;
		uniform float4 _Ambiant_Light_Colour;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalPower );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult23 = dot( ase_worldlightDir , ase_normWorldNormal );
			float2 temp_cast_0 = (dotResult23).xx;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 color2 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float2 panner15 = ( _Time.y * ( _CloudsSpeed / 100.0 ) + i.uv_texcoord);
			float4 lerpResult39 = lerp( ( tex2D( _Albedo, uv_Albedo ) * color2 ) , _CloudsColor , ( _CloudsColor.a * tex2D( _Clouds, panner15 ).a ));
			o.Albedo = ( tex2D( _SunsetRamp, temp_cast_0 ) * lerpResult39 ).rgb;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV27 = dot( ase_normWorldNormal, ase_worldViewDir );
			float fresnelNode27 = ( 0.0 + _Atmosphere_Intensity * pow( 1.0 - fresnelNdotV27, _Atmosphere_Power ) );
			float smoothstepResult30 = smoothstep( 0.0 , 5.0 , fresnelNode27);
			float4 clampResult45 = clamp( ( _Atmosphere_Color * ( dotResult23 * smoothstepResult30 ) ) , float4( 0,0,0,0 ) , float4( 0,0,0,0 ) );
			float4 lerpResult49 = lerp( clampResult45 , ( clampResult45 + ( ( 1.0 - ( -0.5 + dotResult23 ) ) * lerpResult39 ) ) , ( 0.5 * _Ambiant_Light_Colour ));
			float4 clampResult48 = clamp( lerpResult49 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Emission = clampResult48.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19100
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Amplify/PlanetShaderAmplify;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SamplerNode;13;-332.2284,495.6087;Inherit;True;Property;_Normal;Normal;3;0;Create;True;0;0;0;False;0;False;-1;4334c7e094e4fa24ea0931246c6eda83;4334c7e094e4fa24ea0931246c6eda83;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-550.7737,544.3442;Inherit;False;Property;_NormalPower;NormalPower;4;0;Create;True;0;0;0;False;0;False;1;-0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1671.989,-1788.332;Inherit;True;Property;_SunsetRamp;SunsetRamp;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;a5e277725a48cb9459beb19111128046;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-887.4189,-1582.701;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;34;-1203.318,-1806.3;Inherit;False;Property;_Atmosphere_Color;Atmosphere_Color;9;0;Create;True;0;0;0;False;0;False;1,0.5582122,0.3820755,0;0.8679245,0.4960715,0.3479885,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1093.387,-1575.311;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;30;-1271.928,-1495.285;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1479.684,-1443.466;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1478.05,-1354.471;Inherit;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;27;-1781.343,-1550.985;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;45;-604.8215,-1545.956;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-469.4162,-1.15751;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;48;3.849323,-1079.951;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;49;-208.1384,-1077.51;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-483.7975,-1049.593;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;-2243.792,3.693349;Inherit;True;Property;_Clouds;Clouds;1;0;Create;True;0;0;0;False;0;False;-1;09fba879ef4c7cb4f9532b5224f7cb51;09fba879ef4c7cb4f9532b5224f7cb51;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-2163.818,-184.2529;Inherit;False;Property;_CloudsColor;CloudsColor;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.8392157,0.352941,0.1764704,0.7058824;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;39;-1634.238,-199.5128;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1848.997,-95.27047;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-388.3063,-828.8656;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;50;-743.8238,-809.7189;Inherit;False;Property;_Ambiant_Light_Colour;Ambiant_Light_Colour;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-794.1431,-892.6376;Inherit;False;Constant;_Ambiant_Color_Max;Ambiant_Color_Max;12;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-1631.984,-1023.068;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1390.56,-1022.737;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2099.414,-1031.877;Inherit;False;Constant;_Ambient_Mask_Size;Ambient_Mask_Size;10;0;Create;True;0;0;0;False;0;False;-0.5;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1837.414,-1023.877;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;22;-2255.05,-1897.017;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;23;-1992.496,-1768.884;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;21;-2234.274,-1719.069;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;28;-2229.429,-1482.092;Inherit;False;Property;_Atmosphere_Intensity;Atmosphere_Intensity;7;0;Create;True;0;0;0;False;0;False;1;6;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2233.429,-1402.092;Inherit;False;Property;_Atmosphere_Power;Atmosphere_Power;8;0;Create;True;0;0;0;False;0;False;1;5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;15;-2467.197,31.53677;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;20;-2770.167,49.72739;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;18;-2976.289,19.6668;Inherit;False;Property;_CloudsSpeed;CloudsSpeed;5;0;Create;True;0;0;0;False;0;False;0,0;0.1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;19;-2964.431,148.1272;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;17;-2660.435,157.1016;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-2767.605,-91.70003;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-2075.972,-308.4246;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-2431.663,-465.524;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;0;False;0;False;-1;36c71cf5ee8fbe744a5233c126a58954;36c71cf5ee8fbe744a5233c126a58954;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-2379.664,-254.524;Inherit;False;Constant;_AlbedoMult;AlbedoMult;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;0;0;26;0
WireConnection;0;1;13;0
WireConnection;0;2;48;0
WireConnection;13;5;14;0
WireConnection;24;1;23;0
WireConnection;35;0;34;0
WireConnection;35;1;33;0
WireConnection;33;0;23;0
WireConnection;33;1;30;0
WireConnection;30;0;27;0
WireConnection;30;1;31;0
WireConnection;30;2;32;0
WireConnection;27;0;21;0
WireConnection;27;2;28;0
WireConnection;27;3;29;0
WireConnection;45;0;35;0
WireConnection;26;0;24;0
WireConnection;26;1;39;0
WireConnection;48;0;49;0
WireConnection;49;0;45;0
WireConnection;49;1;47;0
WireConnection;49;2;52;0
WireConnection;47;0;45;0
WireConnection;47;1;44;0
WireConnection;4;1;15;0
WireConnection;39;0;3;0
WireConnection;39;1;7;0
WireConnection;39;2;6;0
WireConnection;6;0;7;4
WireConnection;6;1;4;4
WireConnection;52;0;51;0
WireConnection;52;1;50;0
WireConnection;43;0;41;0
WireConnection;44;0;43;0
WireConnection;44;1;39;0
WireConnection;41;0;42;0
WireConnection;41;1;23;0
WireConnection;23;0;22;0
WireConnection;23;1;21;0
WireConnection;15;0;16;0
WireConnection;15;2;20;0
WireConnection;15;1;17;0
WireConnection;20;0;18;0
WireConnection;20;1;19;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
ASEEND*/
//CHKSM=24C7775FCCFCAD6CB383CC3396D4A1406E2ADDAA