Shader"CustomRenderTexture/PhotoreceptorSimHH"
{
// TODO:
// 1. Need to normalize and un-normalize, n,m,h, and V by their min and max values
// 2. Need to make an independent shader to update n,m,and h
    Properties
    {
        _InputTex("Input_Tex", 2D) = "white" {}
        _n_Tex("n_Tex", 2D) = "white" {}
        _m_Tex("m_Tex", 2D) = "white" {}
        _h_Tex("h_Tex", 2D) = "white"{}
        _V_Min("V_Min", Float) = -80
        _V_Max("V_Max", Float) = 45
        _n("n", Color) = (1, 1, 1, 1)
        _n_Mul("n_Mul", Float) = 1
        _m("m", Color) = (1, 1, 1, 1)
        _m_Mul("m_Mul", Float) = 1
        _h("h", Color) = (1, 1, 1, 1)
        _h_Mul("h_Mul", Float) = 1
        _g_Na("g_Na", Color) = (1, 1, 1, 1)
        _g_Na_Mul("g_Na_Mul", Float) = 120
        _g_K("g_K", Color) = (1, 1, 1, 1)
        _g_K_Mul("g_K_Mul", Float) = 36
        _g_L("g_L", Color) = (1, 1, 1, 1)
        _g_L_Mul("g_L_Mul", Float) = 0.3
        _C("C", Color) = (1, 1, 1, 1)
        _C_Mul("C_Mul", Float) = 1
        _E_Na("E_Na", Color) = (1, 1, 1, 1)
        _E_Na_Mul("E_Na_Mul", Float) = 50
        _E_K("E_K", Color) = (1, 1, 1, 1)
        _E_K_Mul("E_K_Mul", Float) = -77
        _E_L("E_L", Color) = (1, 1, 1, 1)
        _E_L_Mul("E_L_Mul", Float) = -54.387
        _DeltaTime("DeltaTime", Float) = 0.5
        _NeuronTime("NeuronTime", Float) = 0
        _Sensitivity("InputCurrentTint", Color) = (1, 1, 1, 1)
        _InputCurrent_Min("InputCurrent_Min", Float) = 0
        _InputCurrent_Max("InputCurrent_Max", Float) = 1
        _NoiseMultiplier("NoiseMultiplier", Color) = (0, 0, 0, 0)
        _NoiseMultiplier_Min("NoiseMultiplier_Min", Float) = -10
        _NoiseMultiplier_Max("NoiseMultiplier_Max", Float) = 60
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

            sampler2D   _InputTex;
            sampler2D   _n_Tex;
            sampler2D   _m_Tex;
            sampler2D   _h_Tex;
            float _V_Min;
            float _V_Max;
            float4 _n;
            float _n_Mul;
            float4 _m;
            float _m_Mul;
            float4 _h;
            float _h_Mul;
            float4 _g_Na;
            float _g_Na_Mul;
            float4 _g_K;
            float _g_K_Mul;
            float4 _g_L;
            float _g_L_Mul;
            float4 _E_Na;
            float _E_Na_Mul;
            float4 _E_K;
            float _E_K_Mul;
            float4 _E_L;
            float _E_L_Mul;
            float4 _C;
            float _C_Mul;
            float _DeltaTime;
            float _NeuronTime;
            float4 _Sensitivity;
            float _InputCurrent_Min;
            float _InputCurrent_Max;
            float4 _NoiseMultiplier;
            float _NoiseMultiplier_Min;
            float _NoiseMultiplier_Max;
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

            float4 scale_normalize(float4 val, float min, float max)
            {
                return (val - min) / (max - min);
            }

            float4 scale(float4 normVal, float min, float max)
            {
                return normVal * (max - min) + min;
            }

            float4 C_net()
            {
                return _C * _C_Mul;
            }

            float4 n_net(float4 n_in)
            {
                return _n * _n_Mul * n_in;
            }

            float4 m_net(float4 m_in)
            {
                return _m * _m_Mul * m_in;
            }

            float4 h_net(float4 h_in)
            {
                return _h * _h_Mul * h_in;
            }

            float4 g_Na_net()
            {
                return _g_Na * _g_Na_Mul;
            }

            float4 g_K_net()
            {
                return _g_K * _g_K_Mul;
            }

            float4 g_L_net()
            {
                return _g_L * _g_L_Mul;
            }

            float4 E_Na_net()
            {
                return _E_Na * _E_Na_Mul;
            }

            float4 E_K_net()
            {
                return _E_K * _E_K_Mul;
            }

            float4 E_L_net()
            {
                return _E_L * _E_L_Mul;
            }

            float4 I_net(float4 I, float2 uv)
            {

                float4 randNeuronTime = float4(sin(_NoiseFrequency * _NeuronTime), cos(_NoiseFrequency * _NeuronTime), 
                                               tan(_NoiseFrequency * _NeuronTime), sin(_NoiseFrequency * _NeuronTime));
                return I + 
                ((rand2(uv + randNeuronTime.xy) * rand4dTo4d(I + randNeuronTime) - 0.5f) * scale(_NoiseMultiplier, _NoiseMultiplier_Min, _NoiseMultiplier_Max));

                //return I;
            }

            float4 ionicCurrent(float4 V, float4 n, float4 m, float4 h)
            {

                return g_Na_net() * pow(m_net(m), 3) * h_net(h) * (V - E_Na_net()) +
                       g_K_net() * pow(n_net(n), 4) * (V - E_K_net()) + 
                       g_L_net() * (V - E_L_net());

                //return g_K_net() * pow(n_net(n), 4) * (V - E_K_net());
            }

            float4 dVdt(float4 V_prev, float4 I, float4 n, float4 m, float4 h, float2 uv)
            {
                return (I_net(I, uv) - ionicCurrent(V_prev, n, m, h)) / C_net();
            }

            float4 HH(float4 V_prev, float4 I_in, float4 n_in, float4 m_in, float4 h_in, float2 uv)
            {
                float4 V = scale(V_prev, _V_Min, _V_Max);
                V += dVdt(V, scale(I_in, _InputCurrent_Min, _InputCurrent_Max) * _Sensitivity, n_in, m_in, h_in, uv) * _DeltaTime;
                V = scale_normalize(V, _V_Min, _V_Max);
                return V;
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 I_in = tex2D(_InputTex, IN.localTexcoord.xy);
                float4 n_in = tex2D(_n_Tex, IN.localTexcoord.xy);
                float4 m_in = tex2D(_m_Tex, IN.localTexcoord.xy);
                float4 h_in = tex2D(_h_Tex, IN.localTexcoord.xy);
                float4 V_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                return clamp(HH(V_prev, I_in, n_in, m_in, h_in, IN.localTexcoord.xy), 0, 1);
                //return _Color * IN.localTexcoord.xyxy;
                //return I_in;
            }
            ENDCG
        }
    }
}
