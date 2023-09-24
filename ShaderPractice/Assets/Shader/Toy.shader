Shader "Unlit/Toy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

	CGINCLUDE   
            #include "UnityCG.cginc"
  		#pragma target 3.0   
  		    #pragma glsl   
  		#define vec2 float2
  		#define vec3 float3
  		#define vec4 float4
  		#define mat2 float2x2
  		#define iGlobalTime _Time.y
  		#define iTime _Time.y
  		#define mod fmod
  		#define mix lerp
  		#define atan atan2
  		#define fract frac 
  		#define texture2D tex2D
  		// 屏幕的尺寸
  		#define iResolution _ScreenParams
  		// 屏幕中的坐标，以pixel为单位
  		#define gl_FragCoord ((_iParam.scPos.xy/_iParam.scPos.w)*_ScreenParams.xy) 
  		#define gl_uv ((_iParam.scPos.xy* 2.0 - iResolution.xy) / iResolution.y);

  		#define PI2 6.28318530718
  		#define pi 3.14159265358979
  		#define halfpi (pi * 0.5)
  		#define oneoverpi (1.0 / pi)

            struct v2f
            {
                float4 scPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };
 
vec3 palette(float t)
{
    vec3 a = vec3(0.5, 0.5, 0.5);
    vec3 b = vec3(0.5, 0.5, 0.5);
    vec3 c = vec3(1.0, 1.0, 1.0);
    vec3 d = vec3(0.263, 0.416, 0.557);

    return a + b * cos(6.28318 * (c * t + d));
}
v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
    o.scPos = ComputeScreenPos(o.pos);
                return o;
            }

vec4 frag(v2f _iParam) : COLOR0
            {
    vec2 fragCoord = ((_iParam.scPos.xy / _iParam.scPos.w) * _ScreenParams.xy);
    vec2 pos = (2.0 * fragCoord - iResolution.xy) / min(iResolution.y, iResolution.x);
    //vec2 uv = pos;
    vec2 uv0 = pos;
    vec3 finalColor = vec3(0., 0., 0.);
    
    for (float i = 0.0; i < 4.0; i++)
    {
        pos = fract(pos * 1.5) - 0.5;

        float d = length(pos) * exp(-length(uv0));
         
        vec3 col = palette(length(uv0) + i * .4 + iTime * .4);

        d = sin(d * 8. + iTime) / 8.;
        d = abs(d);

        d = pow(0.008 / d, 1.4);

        finalColor += col * d;
    }
         
    return vec4(finalColor, 1.0);
}
ENDCG

SubShader { 		
        Pass {		
CGPROGRAM
  
            #pragma vertex vert    
            #pragma fragment frag    
            //#pragma fragmentoption ARB_precision_hint_fastest     
  
            ENDCG
        }    
    }     
FallBack Off
}
