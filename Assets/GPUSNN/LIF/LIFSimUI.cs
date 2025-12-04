using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker;

public class LIFSimUI : MonoBehaviour
{
    [SerializeField]
    private PhotoReceptorLIFShaderSim photoReceptorLIFShaderSim;
    [SerializeField]
    private NetworkClock networkClock;
    
    [SerializeField]
    private ColorPicker colorPickerV_0;
    [SerializeField]
    private ColorPicker colorPickerR;
    [SerializeField]
    private ColorPicker colorPickerC;

    [SerializeField]
    private Slider simSpeedSlider;
    [SerializeField]
    private Slider v_0MulSlider;
    [SerializeField]
    private Slider rMulSlider;
    [SerializeField]
    private Slider cMulSlider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorPickerV_0.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorLIFShaderSim.SetV_0Color(color);
        });
        colorPickerR.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorLIFShaderSim.SetRColor(color);
        });
        colorPickerC.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorLIFShaderSim.SetCColor(color);
        });

        simSpeedSlider.onValueChanged.AddListener(SetTimeStep);
        v_0MulSlider.onValueChanged.AddListener(SetV_0Mul);
        rMulSlider.onValueChanged.AddListener(SetRMul);
        cMulSlider.onValueChanged.AddListener(SetCMul);

        SetTimeStep(simSpeedSlider.value);
        SetV_0Mul(v_0MulSlider.value);
        SetRMul(rMulSlider.value);
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

    private void SetV_0Mul(float mul)
    {
        photoReceptorLIFShaderSim.SetV_0Mul(mul);
    }

    private void SetRMul(float mul)
    {
        photoReceptorLIFShaderSim.SetRMul(mul);
    }

    private void SetCMul(float mul) {
        photoReceptorLIFShaderSim.SetCMul(mul);
    }
}
