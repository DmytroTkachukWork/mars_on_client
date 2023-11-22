// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ParticlesSoftShader" {
	Properties{
		_MainTex("Particle Texture", 2D) = "white" {}
	  _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	  _MaxDistance("Max Distance", Range(0.01,300.0)) = 100.0
		_Color("Color", Color) = (1,1,1,1)
    _FadeDistance("Fade Start/End", vector) = (0.5,1,1,1)
	}

	Category{
	Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "ForceNoShadowCasting" = "True" }
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off
	Lighting Off 
  ZTest Less

	SubShader{
	  Pass{
      ZClip False

	    CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_particles
      #pragma multi_compile_fog

      #include "UnityCG.cginc"


	    struct appdata_t {
	    	float4 vertex : POSITION;
	    	float3 normal : NORMAL;
	    	float2 texcoord : TEXCOORD0;
        fixed4 color : COLOR;
        
	    };

	    struct v2f {
	    	float4 vertex : SV_POSITION;
	    	float2 texcoord : TEXCOORD0;
	    	float4 posWorld : TEXCOORD1;
	    	float3 normalDir : TEXCOORD2;
	    	float4 projPos : TEXCOORD3;
	    	float depth : TEXCOORD4;
        UNITY_FOG_COORDS(5)
        fixed4 color : COLOR;
	    };

	    sampler2D _MainTex;
	    float4 _MainTex_ST;
	    fixed4 _Color;
      float2 _FadeDistance;
	    sampler2D_float _CameraDepthTexture;
	    float _InvFade, _MaxDistance;

	    v2f vert(appdata_t v)
	    {
	    	v2f o;
	    	o.vertex = UnityObjectToClipPos(v.vertex);
	    	o.projPos = ComputeScreenPos(o.vertex);
	    	COMPUTE_EYEDEPTH(o.projPos.z);
	    	o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);

	    	float4x4 modelMatrix = unity_ObjectToWorld;
	    	float3x3 modelMatrixInverse = unity_WorldToObject;

	    	o.posWorld = mul(modelMatrix, v.vertex);
	    	o.normalDir = normalize(mul(v.normal, modelMatrixInverse));


	    	half3 viewDirW = _WorldSpaceCameraPos - o.posWorld;
	    	half viewDist = length(viewDirW);
	    	o.depth = viewDist;

        o.color = v.color * _Color;

	    	return o;
	    }


	    fixed4 frag(v2f i) : SV_Target
	    {
	    	float fade = 1;
	    	float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
	    	float partZ = i.projPos.z;
	    	fade = saturate(_InvFade * (sceneZ - partZ));

        float d = i.depth;
	    	//float cfade = (d - _FadeDistance.x) / (_FadeDistance.y - _FadeDistance.x);
	    	//cfade = saturate(cfade);
	    	//fade *= cfade;
        fade *=saturate( 1 - (d - _MaxDistance) );

	    	half4 col = tex2D(_MainTex, i.texcoord);

	    	col.a *= fade;
	    	col *= i.color;
        UNITY_APPLY_FOG(i.fogCoord, col);
	    	return col;
	    }
	    	ENDCG
	    }
	  }
	}
	}

