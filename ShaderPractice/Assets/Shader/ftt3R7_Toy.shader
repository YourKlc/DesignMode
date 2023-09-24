Shader "Unlit/ftt3R7_Toy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _iMouse ("Mouse Pos", Vector) = (100., 100., 0, 0)

    }

	CGINCLUDE    
            #include "UnityCG.cginc"
  		#pragma target 3.0   
  		    #pragma glsl   
  		#define vec2 float2
  		#define vec3 float3
  		#define vec4 float4
  		#define mat2 float2x2
  		#define mat3 float3x3
        //#define vec3(x) (vec3(x,x,x))
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
        sampler2D _MainTex;
        float4 _iMouse;
#define iMouse (_iMouse)
            struct v2f
            {
                float4 scPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };
v2f vert(appdata_base v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.scPos = ComputeScreenPos(o.pos);
    return o;
}
#define NUM_LAYERS 10.

mat2 Rot(float a)
{
    float c = cos(a), s = sin(a);
    return mat2(c, -s, s, c);
}

float Star(vec2 uv, float flare)
{
    float col = 0.;
    float d = length(uv);
    float m = .02 / d;
    
    float rays = max(0., 1. - abs(uv.x * uv.y * 1000.));
    m += rays * flare;
    uv = mul(uv, Rot(3.1415 / 4.));
    rays = max(0., 1. - abs(uv.x * uv.y * 1000.));
    m += rays * .3 * flare;
    
    m *= smoothstep(1., .2, d);

    return m;
}

float Hash21(vec2 p)
{
    p = fract(p * vec2(123.34, 456.21));
    p += dot(p, p + 45.32);
  
    return fract(p.x * p.y);
}

vec3 StarLayer(vec2 uv)
{
    vec3 col = vec3(0., 0., 0.);
    
    vec2 gv = fract(uv) - 0.5;
    vec2 id = floor(uv);
    
    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            vec2 offs = vec2(x, y);

            float n = Hash21(id + offs);
            float size = fract(n * 345.32);
            
            vec2 p = vec2(n, fract(n * 34.));
            
            float star = Star(gv - offs - p + .5, smoothstep(.8, 1., size) * .6);
            
            vec3 hueShift = fract(n * 2345.2 + dot(uv / 420., tex2D(_MainTex, vec2(0.25, 0.)).rg)) * vec3(.2, .3, .9) * 123.2;

            vec3 color = sin(hueShift) * .5 + .5;
            color = color * vec3(1., .25, 1. + size);

            star *= sin(iTime * 3. + n * 6.2831) * .4 + 1.;
            col += star * size * color;
        }
    }
    
    return col;

}

vec2 N(float angle)
{
    return vec2(sin(angle), cos(angle));
}

void mainImage(out vec4 fragColor, in vec2 fragCoord)
{
    vec2 uv = (fragCoord - 0.5 * iResolution.xy) / iResolution.y;
    vec2 M = (iMouse.xy - iResolution.xy * .5) / iResolution.y;
    float t = iTime * .01;
    
    uv.x = abs(uv.x);
    uv.y += tan((5. / 6.) * 3.1415) * .5;

    vec2 n = N((5. / 6.) * 3.1415);
    float d = dot(uv - vec2(.5, 0.), n);
    uv -= n * max(0., d) * 2.;

    // col += smoothstep(.01, .0, abs(d));

    n = N((2. / 3.) * 3.1415);
    float scale = 1.;
    uv.x += 1.5 / 1.25;
    for (int i = 0; i < 5; i++)
    {
        scale *= 1.25;
        uv *= 1.25;
        uv.x -= 1.5;

        uv.x = abs(uv.x);
        uv.x -= 0.5;
        uv -= n * min(0., dot(uv, n)) * 2.;
    }

 
    uv += M * 4.;

    uv = mul(uv, Rot(t));
    vec3 col = vec3(0., 0., 0.);
    
    float layers = 10.;
    
    for (float i = 0.; i < 1.; i += 1. / NUM_LAYERS)
    {
        float depth = fract(i + t);
        float scale = mix(20., .5, depth);
        float fade = depth * smoothstep(1., .9, depth);
        col += StarLayer(uv * scale + i * 453.2) * fade;
    }

    fragColor = vec4(col, 1.0);
}
vec4 frag(v2f _iParam) : COLOR0
{
    vec2 fragCoord = ((_iParam.scPos.xy / _iParam.scPos.w) * _ScreenParams.xy);
    vec2 pos = (2.0 * fragCoord - iResolution.xy) / min(iResolution.y, iResolution.x);
    vec4 fragColor;
    mainImage(fragColor, fragCoord);
    return fragColor;
}

ENDCG

SubShader {	
        Pass {
CGPROGRAM
            #pragma vertex vert    
            #pragma fragment frag    
            #pragma fragmentoption ARB_precision_hint_fastest     
  
            ENDCG
        } 
    }
FallBack Off
}
