
Shader "Inside/Special/TextureBlur"
{ 
  Properties
  {
    _MainTex   ( "Texture"   , 2D            ) = "white" {}
    _BlurValue ( "Blur Power", Range( 0, 5 ) ) = 1.0

    [KeywordEnum( GAUSSIAN, FAST, MEDIUM, DETAIL )] _Blur           ( "Blur Mode"  ,   float ) = 0.0
    [Toggle( _BLUR_DOUBLE )]                        _BlurDouble     ( "Blur Double",   float ) = 0.0
    [Toggle( UNITY_UI_GRAYSCALE )]                  _UseUIGrayscale ( "Use Grayscale" ,float ) = 0.0
  }

  SubShader
  {
    Tags { "RenderType" = "Opaque" }
    ZWrite Off
    Cull Off

    Pass
    {
      Name "Blur"
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #pragma shader_feature_local __ _BLUR_GAUSSIAN _BLUR_FAST _BLUR_MEDIUM _BLUR_DETAIL
      #pragma shader_feature_local __ _BLUR_DOUBLE
      #pragma shader_feature_local __ UNITY_UI_GRAYSCALE

      struct appdata
      {
        float4 vertex : POSITION;
        half2  uv     : TEXCOORD0;
      };

      struct v2f
      {
        float4 vertex : SV_POSITION;
        half2  uv     : TEXCOORD0;
      };

      inline fixed3 gaussianBlur( sampler2D tex, half2 texcood, half2 blur, half move_px, half multiplier )
      {
        return tex2D( tex, half2( texcood.x - move_px * blur.x, texcood.y - move_px * blur.y ) ) * multiplier;
      }
      
      // * Medium: Sample texture with 4x4 kernel.
      fixed4 tex2DBlurringGaussian( sampler2D tex, half2 texcood, half2 blur )
      {
        const half gaussian[5] = { 0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162 };
        fixed3 color = fixed3( 0.0, 0.0, 0.0 );
      
        color += gaussianBlur( tex, texcood, blur, -4, gaussian[4] );
        color += gaussianBlur( tex, texcood, blur, -3, gaussian[3] );
        color += gaussianBlur( tex, texcood, blur, -2, gaussian[2] );
        color += gaussianBlur( tex, texcood, blur, -1, gaussian[1] );
      
        color += gaussianBlur( tex, texcood, blur,  0, gaussian[0] );
      
        color += gaussianBlur( tex, texcood, blur,  1, gaussian[1] );
        color += gaussianBlur( tex, texcood, blur,  2, gaussian[2] );
        color += gaussianBlur( tex, texcood, blur,  3, gaussian[3] );
        color += gaussianBlur( tex, texcood, blur,  4, gaussian[4] );
      
        return fixed4( color.rgb, 1.0 );
      }
      
      // Sample texture with blurring.
      // * Fast: Sample texture with 3x3 kernel.
      // * Medium: Sample texture with 5x5 kernel.
      // * Detail: Sample texture with 7x7 kernel.
      fixed4 tex2DBlurring( sampler2D tex, half2 texcood, half2 blur, half4 mask )
      {
        #if _BLUR_FAST && _BLUR_DOUBLE
        const int KERNEL_SIZE = 5;
        const float KERNEL_[KERNEL_SIZE] = { 0.2486, 0.7046, 1.0, 0.7046, 0.2486 };
        #elif _BLUR_MEDIUM && _BLUR_DOUBLE
        const int KERNEL_SIZE = 9;
        const float KERNEL_[KERNEL_SIZE] = { 0.0438, 0.1719, 0.4566, 0.8204, 1.0, 0.8204, 0.4566, 0.1719, 0.0438 };
        #elif _BLUR_DETAIL && _BLUR_DOUBLE
        const int KERNEL_SIZE = 13;
        const float KERNEL_[KERNEL_SIZE] = { 0.0438, 0.1138, 0.2486, 0.4566, 0.7046, 0.9141, 1.0, 0.9141, 0.7046, 0.4566, 0.2486, 0.1138, 0.0438 };
        #elif _BLUR_FAST
        const int KERNEL_SIZE = 3;
        const float KERNEL_[KERNEL_SIZE] = { 0.4566, 1.0, 0.4566 };
        #elif _BLUR_MEDIUM
        const int KERNEL_SIZE = 5;
        const float KERNEL_[KERNEL_SIZE] = { 0.2486, 0.7046, 1.0, 0.7046, 0.2486 };
        #elif _BLUR_DETAIL
        const int KERNEL_SIZE = 7;
        const float KERNEL_[KERNEL_SIZE] = { 0.1719, 0.4566, 0.8204, 1.0, 0.8204, 0.4566, 0.1719 };
        #else
        const int KERNEL_SIZE = 1;
        const float KERNEL_[KERNEL_SIZE] = { 1.0 };
        #endif
      
        float4 color = float4( 0.0, 0.0, 0.0, 0.0 );
        float2 shift = float2( 0.0, 0.0 );
        float  sum   = 0;
      
        for ( int x = 0; x < KERNEL_SIZE; x++ )
        {
          shift.x = blur.x * (float( x ) - KERNEL_SIZE / 2);
          for ( int y = 0; y < KERNEL_SIZE; y++ )
          {
            shift.y = blur.y * (float( y ) - KERNEL_SIZE / 2);
            float2 uv = texcood + shift;
            float weight = KERNEL_[x] * KERNEL_[y];
            sum += weight;
            #if _BLUR_DOUBLE
              fixed masked = min( mask.x <= uv.x, uv.x <= mask.z ) * min( mask.y <= uv.y, uv.y <= mask.w );
              color += lerp( fixed4( 0.5, 0.5, 0.5, 0.0 ), tex2D( tex, uv ), masked ) * weight;
            #else
              color += tex2D( tex, uv ) * weight;
            #endif
          }
        }
      
        return color / sum;
      }
      
      // Sample texture with blurring.
      // * Fast: Sample texture with 3x3 kernel.
      // * Medium: Sample texture with 5x5 kernel.
      // * Detail: Sample texture with 7x7 kernel.
      fixed4 tex2DBlurring( sampler2D tex, half2 texcood, half2 blur )
      {
        #if _BLUR_GAUSSIAN
          return tex2DBlurringGaussian( tex, texcood, blur );
        #else
          return tex2DBlurring( tex, texcood, blur, half4( 0.0, 0.0, 1.0, 1.0 ) );
        #endif
      }
      
      // Sample texture with blurring.
      // * Fast: Sample texture with 3x1 kernel.
      // * Medium: Sample texture with 5x1 kernel.
      // * Detail: Sample texture with 7x1 kernel.
      fixed4 tex2DBlurring1D( sampler2D tex, half2 uv, half2 blur )
      {
        #if _BLUR_FAST
          const int KERNEL_SIZE = 3;
        #elif _BLUR_MEDIUM
          const int KERNEL_SIZE = 5;
        #elif _BLUR_DETAIL
          const int KERNEL_SIZE = 7;
        #else
          const int KERNEL_SIZE = 1;
        #endif
      
        float4 color   = float4( 0.0, 0.0, 0.0, 0.0 );
        half2  texcood = float2( 0.0, 0.0 );
        float  sum     = 0;
        float  weight  = 0;
      
        for ( int i = -KERNEL_SIZE / 2; i <= KERNEL_SIZE / 2; i++ )
        {
          texcood = uv;
          texcood.x += blur.x * i;
          texcood.y += blur.y * i;
          weight = 1.0 / (abs( i ) + 2);
          color += tex2D( tex, texcood ) * weight;
          sum += weight;
        }
      
        return color / sum;
      }

      sampler2D _MainTex;
      half4 _MainTex_TexelSize;
      half _BlurValue;

      v2f vert( appdata v )
      {
        v2f o;
        o.vertex = UnityObjectToClipPos( v.vertex );
        o.uv     = v.uv;

        return o;
      }

      fixed4 frag( v2f i ) : COLOR
      {
        fixed3 color = tex2DBlurring( _MainTex, i.uv, _BlurValue * _MainTex_TexelSize.xy * 2.0 );
        #ifdef UNITY_UI_GRAYSCALE
          color = dot( color, float3( 0.3, 0.59, 0.11 ) ); //fast grayscale
        #endif

        return fixed4( color, 1.0f );
      }
      ENDCG
    }
  }
}
