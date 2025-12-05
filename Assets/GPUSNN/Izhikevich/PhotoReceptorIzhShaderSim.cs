using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using NaughtyAttributes;

public class PhotoReceptorIzhShaderSim : MonoBehaviour
{
    [SerializeField]
    private CustomRenderTexture IzhModelVTexture;
    [SerializeField]
    private CustomRenderTexture IzhModelUTexture;
    [SerializeField]
    private Material IzhModelVMaterial;
    [SerializeField]
    private Material IzhModelUMaterial;
    [SerializeField]
    private NetworkClock networkClock;

    [SerializeField]
    private RenderTexture vIntermediate;
    [SerializeField]
    private RenderTexture uIntermediate;

    private CommandBuffer cmdBuffer;

    public void SetAColor(Color color)
    {
        IzhModelUMaterial.SetColor("_A", color);
    }

    public void SetBColor(Color color)
    {
        IzhModelUMaterial.SetColor("_B", color);
    }

    public void SetCColor(Color color)
    {
        IzhModelVMaterial.SetColor("_C", color);
    }

    public void SetDColor(Color color)
    {
        IzhModelUMaterial.SetColor("_D", color);
    }

    public void SetAMul(float mul)
    {
        IzhModelUMaterial.SetFloat("_A_Mul", mul);
    }

    public void SetBMul(float mul)
    {
        IzhModelUMaterial.SetFloat("_B_Mul", mul);
    }

    public void SetCMul(float mul)
    {
        IzhModelVMaterial.SetFloat("_C_Mul", mul);
    }

    public void SetDMul(float mul)
    {
        IzhModelUMaterial.SetFloat("_D_Mul", mul);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeTextures();
        cmdBuffer = new CommandBuffer();
        cmdBuffer.name = "Izh Blit Commands";
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
        IzhModelVTexture.Initialize();
        IzhModelUTexture.Initialize();
    }

    void UpdateTextures()
    {
        cmdBuffer.Clear();
        cmdBuffer.Blit(IzhModelVTexture, vIntermediate);
        cmdBuffer.Blit(IzhModelUTexture, uIntermediate);
        Graphics.ExecuteCommandBuffer(cmdBuffer);

        IzhModelVTexture.Update();
        IzhModelUTexture.Update();
    }

    void UpdateMaterialTime(Material m)
    {
        m.SetFloat("_DeltaTime", networkClock.DeltaTime);
    }

    void UpdateMaterialTimes()
    {
        UpdateMaterialTime(IzhModelVMaterial);
        IzhModelVMaterial.SetFloat("_NeuronTime", networkClock.CurrentTime);
        UpdateMaterialTime(IzhModelUMaterial);
    }

    [Button]
    public void ReInitialize()
    {
        networkClock.ResetTime();
        InitializeTextures();
    }
}
