using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PhotoReceptorHHSynShaderSim : MonoBehaviour
{
    [SerializeField]
    private CustomRenderTexture synGTexture;
    [SerializeField]
    private CustomRenderTexture synITexture;
    [SerializeField]
    private Material synGMaterial;
    [SerializeField]
    private Material synIMaterial;
    [SerializeField]
    private NetworkClock networkClock;

    [SerializeField]
    private RenderTexture synGIntermediate;
    [SerializeField]
    private RenderTexture synIIntermediate;
    [SerializeField]
    private Texture E_revTexture;

    private CommandBuffer cmdBuffer;

    // Start is called before the first frame update
    void Start()
    {
        cmdBuffer = new CommandBuffer();
        cmdBuffer.name = "HH Syn Blit Commands";

        InitializeTextures();
        synIMaterial.SetVector("_g_Dims", new Vector4(1.0f / synGIntermediate.width, 
                               1.0f / synGIntermediate.height,
                               synGIntermediate.width, synGIntermediate.height));
        synIMaterial.SetVector("_E_rev_Dims", new Vector4(1.0f / E_revTexture.width,
                                1.0f / E_revTexture.height,
                                E_revTexture.width, E_revTexture.height));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterialTimes();
        UpdateTextures();
    }

    void InitializeTextures()
    {
        synGTexture.Initialize();
        synITexture.Initialize();
    }

    void UpdateTextures()
    {
        cmdBuffer.Clear();
        cmdBuffer.Blit(synGTexture, synGIntermediate);
        cmdBuffer.Blit(synITexture, synIIntermediate);
        Graphics.ExecuteCommandBuffer(cmdBuffer);

        synGTexture.Update();
        synITexture.Update();
    }

    void OnDestroy()
    {
        // Release the command buffer when done
        cmdBuffer.Release();
    }

    void UpdateMaterialTime(Material m)
    {
        m.SetFloat("_DeltaTime", networkClock.DeltaTime);
    }

    void UpdateMaterialTimes()
    {
        UpdateMaterialTime(synGMaterial);
        UpdateMaterialTime(synIMaterial);
        synIMaterial.SetFloat("_NeuronTime", networkClock.CurrentTime);
    }
}
