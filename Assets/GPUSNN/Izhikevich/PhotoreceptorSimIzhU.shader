Shader"CustomRenderTexture/PhotoreceptorSimIzhU"
{
    Properties
    {
        _V_Tex("V_Tex", 2D) = "white" {}
        _U_Min("U_Min", Float) = -20
        _U_Max("U_Max", Float) = 10
        _V_Min("V_Min", Float) = -65
        _V_Max("V_Max", Float) = 30
        _A("A", Color) = (1, 1, 1, 1)
        _A_Mul("A_Mul", Float) = 0.02
        _B("B", Color) = (1, 1, 1, 1)
        _B_Mul("B_Mul", Float) = 0.2
        _D("D", Color) = (1, 1, 1, 1)
        _D_Mul("D_Mul", Float) = 8
        _DeltaTime("DeltaTime", Float) = 0.5
    }

    SubShader
    {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"

            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            sampler2D _V_Tex;
            float _U_Min;
            float _U_Max;
            float _V_Min;
            float _V_Max;
            float4 _A;
            float _A_Mul;
            float4 _B;
            float _B_Mul;
            float4 _D;
            float _D_Mul;
            float _DeltaTime;

            float rand1(float n)  { return frac(sin(n) * 43758.5453123); }
            float rand2(float2 n) { return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453); }
            float rand4dTo1d(float4 value, float4 dotDir = float4(12.9898, 78.233, 37.719, 17.4265))
            {
	            float4 smallValue = sin(value);
	            float random = dot(smallValue, dotDir);
	            random = frac(sin(random) * 143758.5453);
	            return random;
            }
            float4 rand4dTo4d(float4 value){
	            return float4(
		            rand4dTo1d(value, float4(12.989, 78.233, 37.719, -12.15)),
		            rand4dTo1d(value, float4(39.346, 11.135, 83.155, -11.44)),
		            rand4dTo1d(value, float4(73.156, 52.235, 09.151, 62.463)),
		            rand4dTo1d(value, float4(-12.15, 12.235, 41.151, -1.135))
	            );
            }
            
            
            float gnoise(float2 n) 
            {
	            const float2 d = float2(0.0, 1.0);
    	        float2  b = floor(n), 
                f = smoothstep(d.xx, d.yy, frac(n));

                //float2 f = frac(n);
	            //f = f*f*(3.0-2.0*f);

                float x = lerp(rand2(b), rand2(b + d.yx), f.x),
                 y = lerp(rand2(b + d.xy), rand2(b + d.yy), f.x);

	            return lerp( x, y, f.y );
            }

            float4 scale_normalize(float4 val, float min, float max)
            {
                return (val - min) / (max - min);
            }

            float4 scale(float4 normVal, float min, float max)
            {
                return normVal * (max - min) + min;
            }

            float4 dUdt(float4 V_prev, float4 U_prev)
            {
                return _A * _A_Mul * (_B * _B_Mul * V_prev - U_prev);
            }

            float4 IzhU(float4 V_prev, float4 U_prev)
            {
                float4 U = V_prev < _V_Max ? U_prev : U_prev + _D * _D_Mul;
                U += dUdt(V_prev, U) * _DeltaTime;
                return U;
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 U_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 U_prev_scaled = scale(U_prev, _U_Min, _U_Max);
                float4 V_prev = tex2D(_V_Tex, IN.localTexcoord.xy);
                float4 V_prev_scaled = scale(V_prev, _V_Min, _V_Max);
                float4 U_scaled = IzhU(V_prev_scaled, U_prev_scaled);
                return scale_normalize(clamp(U_scaled, _U_Min, _U_Max), _U_Min, _U_Max);
                //return _Color * IN.localTexcoord.xyxy;
            }
            ENDCG
        }
    }
}
