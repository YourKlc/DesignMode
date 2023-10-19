Shader"Unlit/XsXXDn_Toy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _iMouse ("Mouse Pos", Vector) = (100., 100., 0, 0)
        _iFrame ("Frame", int) = 1
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
    int _iFrame;
    // HG_SDF 

    const float PI  = 3.14159265359;
    const float PHI = 1.61803398875;
    mat2 inverse(mat2 origin)
    {
        mat2 res;
        float p = 1./(origin[0][0] * origin[1][1] - origin[0][1] * origin[1][0]);
        res[0][0] = + origin[1][1];
        res[0][1] = - origin[0][1];
        res[1][0] = - origin[1][0];
        res[1][1] = + origin[0][0];
        return res * p;
    }

    #define PI2 6.28318530718
    #define pi 3.14159265358979
    #define halfpi (pi * 0.5)
    #define oneoverpi (1.0 / pi)
    float4 _iMouse;
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
    
    #define t iTime
    #define r iResolution.xy

    void mainImage( out vec4 fragColor, in vec2 fragCoord,vec2 pos)
    {
        vec3 c;
        float l,z=t;
        for(int i=0;i<3;i++) 
        {
            vec2 p= (fragCoord) / r.xy;

            vec2 uv=fragCoord;  
            p=p-0.5;
            p.x*=r.x/r.y;
            z+=.07;
            l=length(p);
            uv+=p/l*(sin(z)+1.)*abs(sin(l*9.-z-z));
            c[i]=.01/length(mod(uv,1.)-.5);
        }
        fragColor=vec4(c/l,t);
    }

    vec4 frag(v2f _iParam) : SV_Target
    {
        vec2 fragCoord = gl_FragCoord;
        vec2 pos = (2.0 * fragCoord - iResolution.xy) / min(iResolution.y, iResolution.x);
        vec4 fragColor;
        //vec2 uv = _iParam.scPos.xy/_iParam.scPos.w;
        mainImage(fragColor, fragCoord, pos);
        return fragColor;
    }

    ENDCG 

    SubShader 
    {	
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert    
            #pragma fragment frag    
            #pragma fragmentoption ARB_precision_hint_nicest     
            
            ENDCG
        }
    }
    FallBack Off
}

