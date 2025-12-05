using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker;

public class IzhSimUI : MonoBehaviour
{
    [SerializeField]
    private PhotoReceptorIzhShaderSim photoReceptorIzhShaderSim;
    [SerializeField]
    private NetworkClock networkClock;
    
    [SerializeField]
    private ColorPicker colorPickerA;
    [SerializeField]
    private ColorPicker colorPickerB;
    [SerializeField]
    private ColorPicker colorPickerC;
    [SerializeField]
    private ColorPicker colorPickerD;

    [SerializeField]
    private Slider simSpeedSlider;
    [SerializeField]
    private Slider aMulSlider;
    [SerializeField]
    private Slider bMulSlider;
    [SerializeField]
    private Slider cMulSlider;
    [SerializeField]
    private Slider dMulSlider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorPickerA.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorIzhShaderSim.SetAColor(color);
        });
        colorPickerB.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorIzhShaderSim.SetBColor(color);
        });
        colorPickerC.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorIzhShaderSim.SetCColor(color);
        });
        colorPickerD.ColorChanged.AddListener((Color color) =>
        {
            photoReceptorIzhShaderSim.SetDColor(color);
        });

        simSpeedSlider.onValueChanged.AddListener(SetTimeStep);
        aMulSlider.onValueChanged.AddListener(SetAMul);
        bMulSlider.onValueChanged.AddListener(SetBMul);
        cMulSlider.onValueChanged.AddListener(SetCMul);
        dMulSlider.onValueChanged.AddListener(SetDMul);

        SetTimeStep(simSpeedSlider.value);
        SetAMul(aMulSlider.value);
        SetBMul(bMulSlider.value);
        SetCMul(cMulSlider.value);
        SetDMul(dMulSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTimeStep(float timeStep)
    {
        networkClock.TimeStep = timeStep;
    }

    private void SetAMul(float mul)
    {
        photoReceptorIzhShaderSim.SetAMul(mul);
    }

    private void SetBMul(float mul)
    {
        photoReceptorIzhShaderSim.SetBMul(mul);
    }

    private void SetCMul(float mul) {
        photoReceptorIzhShaderSim.SetCMul(mul);
    }

    private void SetDMul(float mul)
    {
        photoReceptorIzhShaderSim.SetDMul(mul);
    }
}
