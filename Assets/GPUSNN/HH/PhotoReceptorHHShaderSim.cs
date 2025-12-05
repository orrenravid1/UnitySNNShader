using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using NaughtyAttributes;

public class PhotoReceptorHHShaderSim : MonoBehaviour
{
    [SerializeField]
    private CustomRenderTexture HHModelTexture;
    [SerializeField]
    private CustomRenderTexture HHModelNTexture;
    [SerializeField]
    private CustomRenderTexture HHModelMTexture;
    [SerializeField]
    private CustomRenderTexture HHModelHTexture;
    [SerializeField]
    private Material HHModelMaterial;
    [SerializeField]
    private Material HHModelNMaterial;
    [SerializeField]
    private Material HHModelMMaterial;
    [SerializeField]
    private Material HHModelHMaterial;
    [SerializeField]
    private NetworkClock networkClock;

    [SerializeField]
    private RenderTexture nIntermediate;
    [SerializeField]
    private RenderTexture mIntermediate;
    [SerializeField]
    private RenderTexture hIntermediate;
    [SerializeField]
    private RenderTexture VIntermediate;

    private CommandBuffer cmdBuffer;

    public const float gNaDefault = 120.0f;
    public const float gKDefault = 36.0f;
    public const float gLDefault = 0.3f;
    public const float ENaDefault = 50.0f;
    public const float EKDefault = -77.0f;
    public const float ELDefault = -54.387f;
    public const float CDefault = 1.0f;

    public void SetGNaColor(Color color)
    {
        HHModelMaterial.SetColor("_g_Na", color);
    }

    public void SetGKColor(Color color)
    {
        HHModelMaterial.SetColor("_g_K", color);
    }

    public void SetGLColor(Color color)
    {
        HHModelMaterial.SetColor("_g_L", color);
    }

    public void SetENaColor(Color color)
    {
        HHModelMaterial.SetColor("_E_Na", color);
    }

    public void SetEKColor(Color color)
    {
        HHModelMaterial.SetColor("_E_K", color);
    }

    public void SetELColor(Color color)
    {
        HHModelMaterial.SetColor("_E_L", color);
    }

    public void SetCColor(Color color)
    {
        HHModelMaterial.SetColor("_C", color);
    }

    public void SetGNaMul(float mul)
    {
        HHModelMaterial.SetFloat("_g_Na_Mul", mul);
    }

    public void SetGKMul(float mul)
    {
        HHModelMaterial.SetFloat("_g_K_Mul", mul);
    }

    public void SetGLMul(float mul)
    {
        HHModelMaterial.SetFloat("_g_L_Mul", mul);
    }

    public void SetENaMul(float mul)
    {
        HHModelMaterial.SetFloat("_E_Na_Mul", mul);
    }

    public void SetEKMul(float mul)
    {
        HHModelMaterial.SetFloat("_E_K_Mul", mul);
    }

    public void SetELMul(float mul)
    {
        HHModelMaterial.SetFloat("_E_L_Mul", mul);
    }

    public void SetCMul(float mul)
    {
        HHModelMaterial.SetFloat("_C_Mul", mul);
    }

    [Button]
    public void ReInitialize()
    {
        networkClock.ResetTime();
        InitializeTextures();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeTextures();
        cmdBuffer = new CommandBuffer();
        cmdBuffer.name = "HH Blit Commands";

        ResetDefaultMuls();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterialTimes();
        UpdateTextures();
    }

    void OnDestroy()
    {
        // Release the command buffer when done
        cmdBuffer.Release();
    }

    void InitializeTextures()
    {
        HHModelTexture.Initialize();
        HHModelNTexture.Initialize();
        HHModelMTexture.Initialize();
        HHModelHTexture.Initialize();
    }

    void UpdateTextures()
    {
        cmdBuffer.Clear();
        cmdBuffer.Blit(HHModelNTexture, nIntermediate);
        cmdBuffer.Blit(HHModelMTexture, mIntermediate);
        cmdBuffer.Blit(HHModelHTexture, hIntermediate);
        cmdBuffer.Blit(HHModelTexture, VIntermediate);
        Graphics.ExecuteCommandBuffer(cmdBuffer);

        HHModelTexture.Update();
        HHModelNTexture.Update();
        HHModelMTexture.Update();
        HHModelHTexture.Update();
    }

    void UpdateMaterialTime(Material m)
    {
        m.SetFloat("_DeltaTime", networkClock.DeltaTime);
    }

    void UpdateMaterialTimes()
    {
        UpdateMaterialTime(HHModelMaterial);
        HHModelMaterial.SetFloat("_NeuronTime", networkClock.CurrentTime);
        UpdateMaterialTime(HHModelHMaterial);
        UpdateMaterialTime(HHModelMMaterial);
        UpdateMaterialTime(HHModelNMaterial);
    }

    void ResetDefaultMuls()
    {
        SetGNaMul(gNaDefault);
        SetGKMul(gKDefault);
        SetGLMul(gLDefault);
        SetENaMul(ENaDefault);
        SetEKMul(EKDefault);
        SetELMul(ELDefault);
        SetCMul(CDefault);
    }
}
