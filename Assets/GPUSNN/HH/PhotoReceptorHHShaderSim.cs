using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using NaughtyAttributes;

public enum HHModelComponentChoice
{
    n,
    m,
    h,
    V
}

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

    [SerializeField]
    private RenderTexture CameraRenderTex;
    [SerializeField]
    private Material DisplayMaterial;

    [SerializeField, OnValueChanged(nameof(UpdateDisplayMaterial))]
    private HHModelComponentChoice ComponentToShow = HHModelComponentChoice.V;

    [SerializeField, OnValueChanged(nameof(SwitchN))]
    bool SwitchNToCamera;
    [SerializeField, OnValueChanged(nameof(SwitchM))]
    bool SwitchMToCamera;
    [SerializeField, OnValueChanged(nameof(SwitchH))]
    bool SwitchHToCamera;

    private CommandBuffer cmdBuffer;

    // Start is called before the first frame update
    void Start()
    {
        InitializeTextures();
        cmdBuffer = new CommandBuffer();
        cmdBuffer.name = "HH Blit Commands";
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

    private void UpdateDisplayMaterial()
    {
        switch (ComponentToShow)
        {
            case HHModelComponentChoice.V:
                DisplayMaterial.SetTexture("_MainTex", VIntermediate);
                break;
            case HHModelComponentChoice.n:
                DisplayMaterial.SetTexture("_MainTex", nIntermediate);
                break;
            case HHModelComponentChoice.m:
                DisplayMaterial.SetTexture("_MainTex", mIntermediate);
                break;
            case HHModelComponentChoice.h:
                DisplayMaterial.SetTexture("_MainTex", hIntermediate);
                break;
        }
    }

    private void SwitchN()
    {
        if (SwitchNToCamera)
        {
            HHModelNMaterial.SetTexture("_V_Tex", CameraRenderTex);
        }
        else
        {
            HHModelNMaterial.SetTexture("_V_Tex", VIntermediate);
        }
    }

    private void SwitchM()
    {
        if (SwitchMToCamera)
        {
            HHModelMMaterial.SetTexture("_V_Tex", CameraRenderTex);
        }
        else
        {
            HHModelMMaterial.SetTexture("_V_Tex", VIntermediate);
        }
    }

    private void SwitchH()
    {
        if (SwitchHToCamera)
        {
            HHModelHMaterial.SetTexture("_V_Tex", CameraRenderTex);
        }
        else
        {
            HHModelHMaterial.SetTexture("_V_Tex", VIntermediate);
        }
    }

    [Button]
    private void ReInitialize()
    {
        networkClock.ResetTime();
        InitializeTextures();
    }
}
