Shader"CustomRenderTexture/PhotoreceptorSimLIF"
{
    Properties
    {
        _Tex("InputTex", 2D) = "white" {}
        _V_0("V_0", Color) = (1, 1, 1, 1)
        _V_0_Mul("V_0_Mul", Float) = -65
        _V_thresh("V_thresh", Color) = (1, 1, 1, 1)
        _V_thresh_Mul("V_thresh_Mul", Float) = -50
        _R("R", Color) = (1, 1, 1, 1)
        _R_Mul("R_Mul", Float) = 1
        _C("C", Color) = (1, 1, 1, 1)
        _C_Mul("C_Mul", Float) = 1
        _DeltaTime("DeltaTime", Float) = 0.5
        _NeuronTime("NeuronTime", Float) = 0
        _Sensitivity("Sensitivity", Color) = (1, 1, 1, 1)
        _Sensitivity_Mul("Sensitivity_Mul", Float) = 1
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
            float4 _V_0;
            float _V_0_Mul;
            float4 _V_thresh;
            float _V_thresh_Mul;
            float4 _R;
            float _R_Mul;
            float4 _C;
            float _C_Mul;
            float _DeltaTime;
            float _NeuronTime;
            float4 _Sensitivity;
            float _Sensitivity_Mul;
            float4 _NoiseMultiplier;
            float _NoiseMultiplier_Mul;
            float _NoiseFrequency;
            /*
            float4 dIdt(float4 I_in)
            {
                // Update this if adding synapses
                return -I_in;
            }
            */

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

            float4 dVdt(float4 V_prev, float4 I, float2 uv)
            {
                float4 Tau = _R * _R_Mul * _C * _C_Mul;
                float4 randNeuronTime = float4(sin(_NoiseFrequency * _NeuronTime), cos(_NoiseFrequency * _NeuronTime), 
                                               tan(_NoiseFrequency * _NeuronTime), sin(_NoiseFrequency * _NeuronTime));
                return (-(V_prev - _V_0_Mul * _V_0) + 
                        _R_Mul * _R * (I + (rand2(uv + randNeuronTime.xy) * rand4dTo4d(I + randNeuronTime) - 0.5f) * _NoiseMultiplier_Mul * _NoiseMultiplier)) / Tau;
            }

            float4 LIF(float4 V_prev, float4 I_in, float2 uv)
            {
                float4 V = V_prev;
                V += dVdt(V_prev, I_in * _Sensitivity_Mul * _Sensitivity, uv) * _DeltaTime;
                V = V < (_V_thresh_Mul * _V_thresh) ? V : (_V_0 * _V_0_Mul);
                return V;
            }

            float4 Debug(float4 V_prev, float4 I_in, float2 uv)
            {
                return dVdt(V_prev, I_in * _Sensitivity_Mul * _Sensitivity, uv);
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 I_in = tex2D(_Tex, IN.localTexcoord.xy);
                float4 V_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                return LIF(V_prev, I_in, IN.localTexcoord.xy);
                //return _Color * IN.localTexcoord.xyxy;
            }
            ENDCG
        }
    }
}
