Shader"CustomRenderTexture/PhotoreceptorSimIzhV"
{
    Properties
    {
        _Tex("InputTex", 2D) = "white" {}
        _U_Tex("U_Tex", 2D) = "white" {}
        _U_Min("U_Min", Float) = -20
        _U_Max("U_Max", Float) = 10
        _V_Min("V_Min", Float) = -65
        _V_Max("V_Max", Float) = 30
        _C("C", Color) = (1, 1, 1, 1)
        _C_Mul("C_Mul", Float) = -65
        _DeltaTime("DeltaTime", Float) = 0.5
        _NeuronTime("NeuronTime", Float) = 0
        _Sensitivity("Sensitivity", Color) = (1, 1, 1, 1)
        _Sensitivity_Mul("Sensitivity_Mul", Float) = 10
        _NoiseMultiplier("NoiseMultiplier", Color) = (0, 0, 0, 0)
        _NoiseMultiplier_Mul("NoiseMultiplier_Mul", Float) = 1
        _NoiseFrequency("NoiseFrequency", Float) = 1
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

            sampler2D   _Tex;
            sampler2D _U_Tex;
            float _U_Min;
            float _U_Max;
            float _V_Min;
            float _V_Max;
            float4 _C;
            float _C_Mul;
            float _DeltaTime;
            float _NeuronTime;
            float4 _Sensitivity;
            float _Sensitivity_Mul;
            float4 _NoiseMultiplier;
            float _NoiseMultiplier_Mul;
            float _NoiseFrequency;

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

            float4 dVdt(float4 V_prev, float4 U_prev, float4 I, float2 uv)
            {
                float4 randNeuronTime = float4(sin(_NoiseFrequency * _NeuronTime), cos(_NoiseFrequency * _NeuronTime), 
                                               tan(_NoiseFrequency * _NeuronTime), sin(_NoiseFrequency * _NeuronTime));
                float4 I_net = I + (rand2(uv + randNeuronTime.xy) * rand4dTo4d(I + randNeuronTime) - 0.5f) * _NoiseMultiplier_Mul * _NoiseMultiplier;
                return 0.04 * V_prev * V_prev + 5 * V_prev + 140 - U_prev + I_net;
            }

            float4 IzhV(float4 V_prev, float4 U_prev, float4 I_in, float2 uv)
            {
                float4 V = V_prev < _V_Max ? V_prev : (_C * _C_Mul);
                V += dVdt(V, U_prev, I_in * _Sensitivity_Mul * _Sensitivity, uv) * _DeltaTime;
                return V;
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 I_in = tex2D(_Tex, IN.localTexcoord.xy);
                float4 U_prev = tex2D(_U_Tex, IN.localTexcoord.xy);
                float4 U_prev_scaled = scale(U_prev, _U_Min, _U_Max);
                float4 V_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 V_prev_scaled = scale(V_prev, _V_Min, _V_Max);
                float4 V_scaled = IzhV(V_prev_scaled, U_prev_scaled, I_in, IN.localTexcoord.xy);
                return scale_normalize(clamp(V_scaled, _V_Min, _V_Max), _V_Min, _V_Max);
                //return _Color * IN.localTexcoord.xyxy;
            }
            ENDCG
        }
    }
}
