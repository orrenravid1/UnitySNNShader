using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoReceptorLIFShaderSim : MonoBehaviour
{
    [SerializeField]
    private CustomRenderTexture photoreceptorLIFTexture;
    [SerializeField]
    private Material photoreceptorLIFMaterial;
    [SerializeField]
    private NetworkClock networkClock;

    public void SetV_0Color(Color color)
    {
        photoreceptorLIFMaterial.SetColor("_V_0", color);
    }

    public void SetV_0Mul(float mul)
    {
        photoreceptorLIFMaterial.SetFloat("_V_0_Mul", mul);
    }

    public void SetRColor(Color color)
    {
        photoreceptorLIFMaterial.SetColor("_R", color);
    }

    public void SetRMul(float mul)
    {
        photoreceptorLIFMaterial.SetFloat("_R_Mul", mul);
    }

    public void SetCColor(Color color)
    {
        photoreceptorLIFMaterial.SetColor("_C", color);
    }

    public void SetCMul(float mul)
    {
        photoreceptorLIFMaterial.SetFloat("_C_Mul", mul);
    }

    // Start is called before the first frame update
    void Start()
    {
        photoreceptorLIFTexture.Initialize();
        SetV_0Color(Color.black);
        SetV_0Mul(1.0f);
        SetRColor(Color.white);
        SetRMul(1.0f);
        SetCColor(Color.white);
        SetCMul(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        photoreceptorLIFTexture.Update();
        photoreceptorLIFMaterial.SetFloat("_DeltaTime", networkClock.DeltaTime);
        photoreceptorLIFMaterial.SetFloat("_NeuronTime", networkClock.CurrentTime);
    }
}
