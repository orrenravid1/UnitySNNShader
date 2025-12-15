using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker;
using TMPro;

public class HHSimUI : MonoBehaviour
{
    [SerializeField]
    private PhotoReceptorHHShaderSim photoReceptorHHShaderSim;
    [SerializeField]
    private NetworkClock networkClock;

    [SerializeField]
    private ColorPicker colorPickerGNa;
    [SerializeField]
    private ColorPicker colorPickerENa;
    [SerializeField]
    private ColorPicker colorPickerGK;
    [SerializeField]
    private ColorPicker colorPickerEK;
    [SerializeField]
    private ColorPicker colorPickerGL;
    [SerializeField]
    private ColorPicker colorPickerEL;
    [SerializeField]
    private ColorPicker colorPickerC;

    [SerializeField]
    private CustomSlider simSpeedSlider;
    [SerializeField]
    private CustomSlider gNaMulSlider;
    [SerializeField]
    private CustomSlider gKMulSlider;
    [SerializeField]
    private CustomSlider gLMulSlider;
    [SerializeField]
    private CustomSlider eNaMulSlider;
    [SerializeField]
    private CustomSlider eKMulSlider;
    [SerializeField]
    private CustomSlider eLMulSlider;
    [SerializeField]
    private CustomSlider cMulSlider;

    [SerializeField]
    private Button resetParametersButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorPickerGNa.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetGNaColor(color);
        });
        colorPickerENa.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetENaColor(color);
        });
        colorPickerGK.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetGKColor(color);
        });
        colorPickerEK.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetEKColor(color);
        });
        colorPickerGL.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetGLColor(color);
        });
        colorPickerEL.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetELColor(color);
        });
        colorPickerC.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorHHShaderSim.SetCColor(color);
        });

        simSpeedSlider.onValueChanged += SetTimeStep;

        gNaMulSlider.onValueChanged += SetGNaMul;
        eNaMulSlider.onValueChanged += SetENaMul;
        gKMulSlider.onValueChanged += SetGKMul;
        eKMulSlider.onValueChanged += SetEKMul;
        gLMulSlider.onValueChanged += SetGLMul;
        eLMulSlider.onValueChanged += SetELMul;
        cMulSlider.onValueChanged += SetCMul;

        resetParametersButton.onClick.AddListener(ResetParameters);

        SetTimeStep(simSpeedSlider.Value);
        SetGNaMul(gNaMulSlider.Value);
        SetENaMul(eNaMulSlider.Value);
        SetGKMul(gKMulSlider.Value);
        SetEKMul(eKMulSlider.Value);
        SetGLMul(gLMulSlider.Value);
        SetELMul(eLMulSlider.Value);
        SetCMul(cMulSlider.Value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetTimeStep(float timeStep)
    {
        networkClock.TimeStep = timeStep;
    }

    private void SetGNaMul(float mul)
    {
        photoReceptorHHShaderSim.SetGNaMul(mul);
    }

    private void SetENaMul(float mul)
    {
        photoReceptorHHShaderSim.SetENaMul(mul);
    }

    private void SetGKMul(float mul)
    {
        photoReceptorHHShaderSim.SetGKMul(mul);
    }

    private void SetEKMul(float mul)
    {
        photoReceptorHHShaderSim.SetEKMul(mul);
    }

    private void SetGLMul(float mul)
    {
        photoReceptorHHShaderSim.SetGLMul(mul);
    }

    private void SetELMul(float mul)
    {
        photoReceptorHHShaderSim.SetELMul(mul);
    }

    private void SetCMul(float mul)
    {
        photoReceptorHHShaderSim.SetCMul(mul);
    }

    private void ResetParameters()
    {
        gNaMulSlider.Value = PhotoReceptorHHShaderSim.gNaDefault;
        eNaMulSlider.Value = PhotoReceptorHHShaderSim.ENaDefault;
        gKMulSlider.Value = PhotoReceptorHHShaderSim.gKDefault;
        eKMulSlider.Value = PhotoReceptorHHShaderSim.EKDefault;
        gLMulSlider.Value = PhotoReceptorHHShaderSim.gLDefault;
        eLMulSlider.Value = PhotoReceptorHHShaderSim.ELDefault;
        cMulSlider.Value = PhotoReceptorHHShaderSim.CDefault;

        colorPickerGNa.AssignColor(Color.white);
        colorPickerENa.AssignColor(Color.white);
        colorPickerGK.AssignColor(Color.white);
        colorPickerEK.AssignColor(Color.white);
        colorPickerGL.AssignColor(Color.white);
        colorPickerEL.AssignColor(Color.white);
        colorPickerC.AssignColor(Color.white);

        photoReceptorHHShaderSim.ResetDefaultMuls();
        photoReceptorHHShaderSim.ResetDefaultColors();

        photoReceptorHHShaderSim.ReInitialize();
    }
}
