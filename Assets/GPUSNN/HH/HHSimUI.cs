using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker;

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
    private Slider simSpeedSlider;
    [SerializeField]
    private Slider gNaMulSlider;
    [SerializeField]
    private Slider gKMulSlider;
    [SerializeField]
    private Slider gLMulSlider;
    [SerializeField]
    private Slider eNaMulSlider;
    [SerializeField]
    private Slider eKMulSlider;
    [SerializeField]
    private Slider eLMulSlider;
    [SerializeField]
    private Slider cMulSlider;

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

        simSpeedSlider.onValueChanged.AddListener(SetTimeStep);

        gNaMulSlider.onValueChanged.AddListener(SetGNaMul);
        eNaMulSlider.onValueChanged.AddListener(SetENaMul);
        gKMulSlider.onValueChanged.AddListener(SetGKMul);
        eKMulSlider.onValueChanged.AddListener(SetEKMul);
        gLMulSlider.onValueChanged.AddListener(SetGLMul);
        eLMulSlider.onValueChanged.AddListener(SetELMul);
        cMulSlider.onValueChanged.AddListener(SetCMul);

        resetParametersButton.onClick.AddListener(ResetParameters);


        SetTimeStep(simSpeedSlider.value);
        SetGNaMul(gNaMulSlider.value);
        SetENaMul(eNaMulSlider.value);
        SetGKMul(gKMulSlider.value);
        SetEKMul(eKMulSlider.value);
        SetGLMul(gLMulSlider.value);
        SetELMul(eLMulSlider.value);
        SetCMul(cMulSlider.value);
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
        gNaMulSlider.value = PhotoReceptorHHShaderSim.gNaDefault;
        eNaMulSlider.value = PhotoReceptorHHShaderSim.ENaDefault;
        gKMulSlider.value = PhotoReceptorHHShaderSim.gKDefault;
        eKMulSlider.value = PhotoReceptorHHShaderSim.EKDefault;
        gLMulSlider.value = PhotoReceptorHHShaderSim.gLDefault;
        eLMulSlider.value = PhotoReceptorHHShaderSim.ELDefault;
        cMulSlider.value = PhotoReceptorHHShaderSim.CDefault;

        colorPickerGNa.CurrentColor = Color.white;
        colorPickerENa.CurrentColor = Color.white;
        colorPickerGK.CurrentColor = Color.white;
        colorPickerEK.CurrentColor = Color.white;
        colorPickerGL.CurrentColor = Color.white;
        colorPickerEL.CurrentColor = Color.white;
        colorPickerC.CurrentColor = Color.white;

        photoReceptorHHShaderSim.ReInitialize();
    }
}
