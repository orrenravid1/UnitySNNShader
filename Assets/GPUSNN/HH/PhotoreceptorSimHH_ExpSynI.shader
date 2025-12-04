Shader"CustomRenderTexture/PhotoreceptorSimExpSynI"
{
    Properties
    {
        _V_Tex("V_Tex", 2D) = "white" {}
        _V_Min("V_Min", Float) = -80
        _V_Max("V_Max", Float) = 45
        _g_Tex("g_Tex", 2D) = "black" {}
        _g_Dims("g_Dims", Vector) = (1, 1, 1, 1)
        _g_Max("g_Max", Float) = 1
        _E_rev_Tex("E_rev_Tex", 2D) = "white" {}
        _E_rev_Dims("E_rev_Dims", Vector) = (1, 1, 1, 1)
        _E_rev_Min("E_rev_Min", Float) = -100
        _E_rev_Max("E_rev_Max", Float) = 100
        _Weight_Tex("Weight_Tex", 2D) = "white" {}
        _Weight_Max("Weight_Max", Float) = 1

        _I_Min("_I_Min", Float) = 1
        _I_Max("_I_Max", Float) = 1

        _Color("Color", Color) = (1, 1, 1, 1)
        _DeltaTime("DeltaTime", Float) = 0.5
        _NeuronTime("NeuronTime", Float) = 0
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
            float4 _V_Tex_TexelSize;
            float _V_Min;
            float _V_Max;
            sampler2D _g_Tex;
            //float4 _g_Tex_TexelSize;
            float4 _g_Dims;
            float _g_Max;
            sampler2D _E_rev_Tex;  
            //float4 _E_rev_Tex_TexelSize;
            float4 _E_rev_Dims;
            float _E_rev_Min;
            float _E_rev_Max;
            sampler2D _Weight_Tex;
            float _Weight_Max;
            float _I_Min;
            float _I_Max;

            float _DeltaTime;
            float _NeuronTime;
            float4 _Color;

            float4 scale_normalize(float4 val, float min, float max)
            {
                return (val - min) / (max - min);
            }

            float4 scale(float4 normVal, float min, float max)
            {
                return normVal * (max - min) + min;
            }

            float4 I_syn_agg(float2 uv)
            {
                // Note: I think texel size might be broken. Try to manually set the texel sizes and see what happens.
                float epsilon = 1e-5;
                //float2 filterDims = _E_rev_Tex_TexelSize.zw;
                float2 filterDims = _E_rev_Dims.zw;
                float filterWidth = filterDims.x;
                float filterHeight = filterDims.y;
                //float2 dFilter = _E_rev_Tex_TexelSize.xy;
                float2 dFilter = _E_rev_Dims.xy;
                //float2 dg = _g_Tex_TexelSize.xy;
                float2 dg = _g_Dims.xy;
                float2 offset = uv - floor(filterDims / 2) * dg;
                float4 Isum = 0;
                float4 weightSum = 0;
                float totalPixels = filterWidth * filterHeight;
                
                for (float i = 0; i < filterWidth; i++)
                {
                    for (float j = 0; j < filterHeight; j++) 
                    {
                        float2 filterUV = float2(dFilter.x*i, dFilter.y*j) + epsilon;
                        float2 gUV = offset + float2(dg.x*i, dg.y*j) + epsilon;
                        filterUV = clamp(filterUV, 0.0, 1.0);
                        gUV = clamp(gUV, 0.0, 1.0);
                        float4 E_rev = scale(tex2D(_E_rev_Tex, filterUV),
                                                  _E_rev_Min, _E_rev_Max);
                        float4 weight = scale(tex2D(_Weight_Tex, filterUV), 0, _Weight_Max);
                        float4 g = scale(tex2D(_g_Tex, gUV), 0, _g_Max);
                        float4 V = scale(tex2D(_V_Tex, gUV), _V_Min, _V_Max);
                        //Isum += float4(gUV.x, gUV.y, 0, 0);
                        Isum += weight * g * (V - E_rev);
                        weightSum += weight;
                    }
                }
                /*
                float4 E_rev = scale(tex2D(_E_rev_Tex, float2(0.5f, 0.5f)),
                                                  _E_rev_Min, _E_rev_Max);
                float4 g = scale(tex2D(_g_Tex, uv), 0, _g_Max);
                float4 V = scale(tex2D(_V_Tex, uv), _V_Min, _V_Max);
                Isum = g * (V - E_rev);
                */
                
                return Isum / weightSum;
                /*
                totalPixels = 1;
                float2 offset2 = float2(sin(_NeuronTime / 10), cos(_NeuronTime / 10));
                float4 E_rev = scale(tex2D(_E_rev_Tex, uv + dFilter*offset2),
                                              _E_rev_Min, _E_rev_Max);
                float4 g = scale(tex2D(_g_Tex, uv), 0, _g_Max);
                float4 V = scale(tex2D(_V_Tex, uv), _V_Min, _V_Max);
                Isum += g * (V - E_rev);
                return Isum / totalPixels;
                */
                /*
                float4 res = tex2D(_E_rev_Tex, dFilter * float2(_Tmp_i, _Tmp_j));
                return res;
                */
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                // Note need to make sure current can be negative on the HH side so negative current is possible
    
                //float4 I_prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 IUnscaled = I_syn_agg(IN.localTexcoord.xy);
                float Imin = _g_Max * (_V_Min - _E_rev_Max);
                float Imax = _g_Max * (_V_Max - _E_rev_Min);
                IUnscaled = clamp(IUnscaled, Imin, Imax);
                return _Color * scale_normalize(IUnscaled, Imin, Imax);
                //return _Color * IN.localTexcoord.xyxy;
            }
            ENDCG
        }
    }
}
