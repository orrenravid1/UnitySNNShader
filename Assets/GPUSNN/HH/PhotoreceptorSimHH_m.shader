Shader"CustomRenderTexture/PhotoreceptorSimHH_m"
{
// TODO:
// 1. Need to normalize and un-normalize, n,m,h, and V by their min and max values
    Properties
    {
        _V_Tex("V_Tex", 2D) = "white" {}
        _V_Min("V_Min", Float) = 45
        _V_Max("V_Max", Float) = -80
        _AlphaMul("Alpha_Mul", Float) = 1
        _BetaMul("Beta_Mul", Float) = 1
        _AlphaExpVoltageOffset("AlphaExpVoltageOffset", Float) = 0
        _BetaExpVoltageOffset("BetaExpVoltageOffset", Float) = 0
        _AlphaExpDenomOffset("AlphaExpDenomOffset", Float) = 0
        _BetaExpDenomOffset("BetaExpDenomOffset", Float) = 0
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
            float _AlphaMul;
            float _BetaMul;
            float _AlphaExpVoltageOffset;
            float _BetaExpVoltageOffset;
            float _AlphaExpDenomOffset;
            float _BetaExpDenomOffset;
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

            float4 alpha_m(float4 V)
            {
                return _AlphaMul * (0.1f * (V + 40.0f)) / (1.0f - exp(-(V + 40.0f + _AlphaExpVoltageOffset) / (10.0f + _AlphaExpDenomOffset)));
            }

            float4 beta_m(float4 V)
            {
                return _BetaMul * 4.0f * exp(-(V + 65.0f + _BetaExpVoltageOffset) / (18.0f + _BetaExpDenomOffset));
            }

            float4 dmdt(float4 m_prev, float4 V_prev) 
            {
                return alpha_m(V_prev) * (1-m_prev) - beta_m(V_prev) * m_prev;
            }

            float4 HH_m(float4 m_prev, float4 V_prev)
            {
                float4 m = m_prev;
                float4 V_in = scale(V_prev, _V_Min, _V_Max);
                m += dmdt(m_prev, V_in) * _DeltaTime;
                return m;
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 m_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 V_prev = tex2D(_V_Tex, IN.localTexcoord.xy);
                return clamp(HH_m(m_prev, V_prev * _Color), 0, 1);
                //return _Color * IN.localTexcoord.xyxy;
            }
            ENDCG
        }
    }
}
