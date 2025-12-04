Shader"CustomRenderTexture/PhotoreceptorSimExpSynG"
{
    Properties
    {
        _V_Tex("V_Tex", 2D) = "white" {}
        _V_Min("V_Min", Float) = -80
        _V_Max("V_Max", Float) = 45
        _V_Thresh("V_Thresh", Float) = 30
        _g_Max("g_Max", Float) = 1
        _Delta_g_Max("Delta_g_Max", Float) = 1
        _g_Steepness("g_Steepness", Float) = 10
        _Tau("Tau", Float) = 1
        
        _Color("Color", Color) = (1, 1, 1, 1)
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

            sampler2D   _V_Tex;
            float _V_Min;
            float _V_Max;
            float _V_Thresh;
            float _g_Max;
            float _Delta_g_Max;
            float _g_Steepness;
            float _Tau;
            float _DeltaTime;
            float4 _Color;

            float4 scale_normalize(float4 val, float min, float max)
            {
                return (val - min) / (max - min);
            }

            float4 scale(float4 normVal, float min, float max)
            {
                return normVal * (max - min) + min;
            }

            float4 activationFunc(float4 V) 
            {
                return 1.0f / (1 + exp(-_g_Steepness * (V - _V_Thresh)));
            }

            float4 dgdt(float4 g_prev, float4 V_prev) 
            {
                return -g_prev / _Tau + _Delta_g_Max * (1 - g_prev/_g_Max) * activationFunc(V_prev);                  
            }

            float4 HH_syn_g(float4 g_prev, float4 V_prev)
            {
                float4 g = scale(g_prev, 0, _g_Max);
                float4 V_in = scale(V_prev, _V_Min, _V_Max);
                g += dgdt(g_prev, V_in) * _DeltaTime;
                return g;
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 g_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 V_prev = tex2D(_V_Tex, IN.localTexcoord.xy);
                float4 gUnscaled = clamp(HH_syn_g(g_prev, V_prev * _Color), 0, _g_Max);
                return scale_normalize(gUnscaled, 0, _g_Max);
                //return _Color * IN.localTexcoord.xyxy;
            }
            ENDCG
        }
    }
}
