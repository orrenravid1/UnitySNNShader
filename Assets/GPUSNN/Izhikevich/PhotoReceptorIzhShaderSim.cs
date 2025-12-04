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
