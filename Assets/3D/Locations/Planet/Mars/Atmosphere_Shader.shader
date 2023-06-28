// Made with Amplify Shader Editor v1.9.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/Atmosphere_Shader"
{
	Properties
	{
		_Atmosphere_Sharpness("Atmosphere_Sharpness", Range( 0 , 1)) = 0.15
		_Atmosphere_Circle("Atmosphere_Circle", Range( 0 , 1)) = 0.3851372
		_Atmosphere_Color("Atmosphere_Color", Color) = (1,0.3621442,0.0518868,0)
		_Atmosphere_Size("Atmosphere_Size", Range( 0 , 0.1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Front
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Lambert keepalpha noambient nofog vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform float _Atmosphere_Size;
		uniform float4 _Atmosphere_Color;
		uniform float _Atmosphere_Sharpness;
		uniform float _Atmosphere_Circle;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ase_vertexNormal * _Atmosphere_Size );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = i.worldNormal;
			float dotResult3 = dot( ase_worldlightDir , ase_worldNormal );
			float dotResult5 = dot( ase_worldNormal , i.viewDir );
			float temp_output_15_0 = pow( (0.0 + (-dotResult5 - 0.0) * (( 15.0 * _Atmosphere_Sharpness ) - 0.0) / (1.0 - 0.0)) , ( 15.0 * _Atmosphere_Sharpness ) );
			float lerpResult16 = lerp( ( dotResult3 * temp_output_15_0 ) , temp_output_15_0 , _Atmosphere_Circle);
			float clampResult18 = clamp( lerpResult16 , 0.0 , 1.0 );
			o.Emission = ( _Atmosphere_Color * clampResult18 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19100
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;111,-151;Float;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;Amplify/Atmosphere_Shader;False;False;False;False;True;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;Front;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0;True;False;0;True;Overlay;;Overlay;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.DotProductOpNode;5;-2047.977,-64.46194;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-162.0056,-467.1311;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;21;-485.4097,174.244;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-199.4097,115.7441;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;8;-1555.56,-75.64682;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;16;-621.2616,-388.9865;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;18;-387.7217,-398.0622;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-446.0682,-599.7908;Inherit;False;Property;_Atmosphere_Color;Atmosphere_Color;3;0;Create;True;0;0;0;False;0;False;1,0.3621442,0.0518868,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;6;-2288.062,25.8044;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;2;-2293.609,-120.5392;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;7;-1793.116,-98.31949;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2015.695,436.8592;Float;False;Property;_Atmosphere_Sharpness;Atmosphere_Sharpness;1;0;Create;True;0;0;0;False;0;False;0.15;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1793.695,-11.14077;Float;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1792.695,68.85922;Float;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-897.9601,-87.34995;Float;False;Property;_Atmosphere_Circle;Atmosphere_Circle;2;0;Create;True;0;0;0;False;0;False;0.3851372;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-473.7093,362.7441;Float;False;Property;_Atmosphere_Size;Atmosphere_Size;4;0;Create;True;0;0;0;False;0;False;0;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1729.239,211.6851;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1990.683,205.0281;Float;False;Constant;_Atmosphere_Intensity;Atmosphere_Intensity;1;0;Create;True;0;0;0;False;0;False;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;3;-2040.279,-320.7896;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;1;-2301.275,-417.7898;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;15;-1279.311,-76.48369;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1438.659,266.4626;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1093.371,-346.5685;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
WireConnection;0;2;20;0
WireConnection;0;11;23;0
WireConnection;5;0;2;0
WireConnection;5;1;6;0
WireConnection;20;0;19;0
WireConnection;20;1;18;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;8;2;10;0
WireConnection;8;3;9;0
WireConnection;8;4;13;0
WireConnection;16;0;26;0
WireConnection;16;1;15;0
WireConnection;16;2;17;0
WireConnection;18;0;16;0
WireConnection;7;0;5;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;15;0;8;0
WireConnection;15;1;14;0
WireConnection;14;0;11;0
WireConnection;14;1;12;0
WireConnection;26;0;3;0
WireConnection;26;1;15;0
ASEEND*/
//CHKSM=7A7864918221D1ACFDA9A52877CB4A04DF4D7507