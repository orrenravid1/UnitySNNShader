using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CustomSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private SliderEventAdapter sliderEventAdapter;
    [SerializeField]
    private TMP_Text minText;
    [SerializeField]
    private TMP_Text maxText;
    [SerializeField]
    private bool valueIsMaxValue = true;

    public event Action onPointerDown;
    public event Action onPointerUp;
    public event Action<float> onValueChanged;

    private bool isPointerDown = false;

    public float MinValue
    {
        get => slider.minValue;
        set => SetMinValue(value);
    }

    public float MaxValue
    {
        get => slider.maxValue;
        set => SetMaxValue(value);
    }

    public float Value
    {
        get => slider.value;
        set => SetValue(value);
    }

    private void SetMinValue(float f)
    {
        slider.minValue = f;
        ApplyMinValueText();
    }

    private void SetMaxValue(float f)
    {
        slider.maxValue = f;
        ApplyMaxValueText();
    }

    private void SetValue(float f)
    {
        slider.value = f;
        ApplyValueText();
    }

    private void ApplyMinValueText()
    {
        minText.text = slider.minValue.ToString("F2");
    }

    private void ApplyMaxValueText()
    {
        if (!valueIsMaxValue)
            maxText.text = slider.maxValue.ToString("F2");
    }

    private void ApplyValueText()
    {
        if (valueIsMaxValue)
            maxText.text = slider.value.ToString("F2");
    }

    private void Awake()
    {
        slider.onValueChanged.AddListener((float value) =>
        {
            onValueChanged?.Invoke(value);
        });
        sliderEventAdapter.onPointerDown += OnEventAdapterPointerEnter;
        sliderEventAdapter.onPointerUp += OnEventAdapterPointerExit;
        ApplyMinValueText();
        ApplyMaxValueText();
        ApplyValueText();
    }

    private void Update()
    {
        if (isPointerDown)
        {
            ApplyValueText();
        }
    }

    private void OnEventAdapterPointerEnter()
    {
        isPointerDown = true;
        onPointerDown?.Invoke();
    }

    private void OnEventAdapterPointerExit()
    {
        isPointerDown = false;
        onPointerUp?.Invoke();
    }
}
