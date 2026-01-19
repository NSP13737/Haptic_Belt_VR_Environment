using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToTMP : MonoBehaviour
{
    [Header("Assign these in Inspector")]
    public Slider slider;
    public TMP_Text valueText;

    [Header("Display Settings")]
    public bool showAsPercent = false;   // if slider is 0..1
    public int decimals = 2;             // 0 = no decimals, 2 = like 0.48

    [Header("Optional prefix/suffix")]
    public string prefix = "";
    public string suffix = "";

    private void Awake()
    {
        if (slider == null)
            slider = GetComponentInChildren<Slider>(true);

        if (slider != null)
            slider.onValueChanged.AddListener(UpdateText);

        // set initial text right away
        UpdateText(slider != null ? slider.value : 0f);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(UpdateText);
    }

    private void UpdateText(float v)
    {
        if (valueText == null) return;

        string format = "F" + Mathf.Max(0, decimals);

        if (showAsPercent)
        {
            valueText.text = $"{prefix}{(v * 100f).ToString(format)}%{suffix}";
        }
        else
        {
            valueText.text = $"{prefix}{v.ToString(format)}{suffix}";
        }
    }
}
