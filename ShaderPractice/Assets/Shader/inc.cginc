    
    #define fract frac 
    float3 palette(float t)
    {
        float3 a = float3(0.5, 0.5, 0.5);
        float3 b = float3(0.5, 0.5, 0.5);
        float3 c = float3(1.0, 1.0, 1.0);
        float3 d = float3(0.263, 0.416, 0.557);

        return a + b * cos(6.28318 * (c * t + d));
    }

void GetGlowColor_float(float2 posx,  out float3 tColor)
{ 
        float2 uv0 = posx;
        float2 pos = posx;
        float3 finalColor = float3(0., 0., 0.);
        for (int i = 0; i < 4; i++)
        {
            pos = fract(pos * 1.5) - 0.5;

            float d = length(pos) * exp(-length(uv0));
            
            float3 col = palette(length(uv0) + i * .4 + _Time.y * .4);

            d = sin(d * 8. + _Time.y) / 8.;
            d = abs(d);

            d = pow(0.008 / d, 1.4);

            finalColor += col * d;
            finalColor = saturate(finalColor);
        }
        tColor = finalColor;
}
