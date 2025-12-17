
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Study_Params_Manager : MonoBehaviour
{
    UDP_Manager udp;

    [Header("Condition")]
    [SerializeField, Range(1, 2)]
    float conditionSelection = 1.0f;

    // -----------------------------
    // Activation Distance (meters)
    // -----------------------------
    float minActivationDist_sliderMin = 0.0f;
    float minActivationDist_sliderMax = 5.0f;
    [SerializeField, Range(0, 5)]
    float minActivationDist = 0.0f;

    float maxActivationDist_sliderMin = 0.0f;
    float maxActivationDist_sliderMax = 5.0f;
    [SerializeField, Range(0, 5)]
    float maxActivationDist = 5.0f;

    // -----------------------------
    // Frequency (Hz)
    // -----------------------------
    float minFreqHz_sliderMin = 0.0f;
    float minFreqHz_sliderMax = 50.0f;
    [SerializeField, Range(0, 50)]
    float minFreqHz = 0.0f;

    float maxFreqHz_sliderMin = 0.0f;
    float maxFreqHz_sliderMax = 50.0f;
    [SerializeField, Range(0, 50)]
    float maxFreqHz = 50.0f;

    // -----------------------------
    // Fixed Duty Cycle (0..1)
    // -----------------------------
    float fixedDutyCycle_sliderMin = 0.0f;
    float fixedDutyCycle_sliderMax = 1.0f;
    [SerializeField, Range(0, 1)]
    float fixedDutyCycle = 0.5f;

    // -----------------------------
    // Fixed Frequency (Hz)
    // -----------------------------
    float fixedFreqHz_sliderMin = 0.0f;
    float fixedFreqHz_sliderMax = 50.0f;
    [SerializeField, Range(0, 50)]
    float fixedFreqHz = 10.0f;

    // -----------------------------
    // Just Detectable Intensity (0..1)
    // -----------------------------
    float justDetectableIntensity_sliderMin = 0.0f;
    float justDetectableIntensity_sliderMax = 1.0f;
    [SerializeField, Range(0, 1)]
    float justDetectableIntensity = 0.2f;

    private void Awake()
    {
        udp = GetComponent<UDP_Manager>();
    }

    void Update()
    {
        float[] studyParamsBuffer =
        {
            conditionSelection,
            minActivationDist,
            maxActivationDist,
            minFreqHz,
            maxFreqHz,
            fixedDutyCycle,
            fixedFreqHz,
            justDetectableIntensity
        };

        udp.setStudyParams(studyParamsBuffer);
    }

    // =========================
    // Public API (Normalized 0..1)
    // =========================

    // Toggle condition between 1 and 2
    public void changeCondition()
    {
        conditionSelection = (conditionSelection >= 2.0f) ? 1.0f : 2.0f;
    }

    // Min Activation Distance (normalized 0..1)
    public void ChangeMinActivationDist(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        minActivationDist = Mathf.Lerp(minActivationDist_sliderMin, minActivationDist_sliderMax, t);

        // Enforce min <= max
        if (minActivationDist > maxActivationDist)
            minActivationDist = maxActivationDist;
    }

    // Max Activation Distance (normalized 0..1)
    public void ChangeMaxActivationDist(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        maxActivationDist = Mathf.Lerp(maxActivationDist_sliderMin, maxActivationDist_sliderMax, t);

        // Enforce max >= min
        if (maxActivationDist < minActivationDist)
            maxActivationDist = minActivationDist;
    }

    // Min Frequency Hz (normalized 0..1)
    public void ChangeMinFreqHz(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        minFreqHz = Mathf.Lerp(minFreqHz_sliderMin, minFreqHz_sliderMax, t);

        // Enforce min <= max
        if (minFreqHz > maxFreqHz)
            minFreqHz = maxFreqHz;
    }

    // Max Frequency Hz (normalized 0..1)
    public void ChangeMaxFreqHz(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        maxFreqHz = Mathf.Lerp(maxFreqHz_sliderMin, maxFreqHz_sliderMax, t);

        // Enforce max >= min
        if (maxFreqHz < minFreqHz)
            maxFreqHz = minFreqHz;
    }

    // Fixed Duty Cycle (normalized 0..1)
    public void ChangeFixedDutyCycle(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        fixedDutyCycle = Mathf.Lerp(fixedDutyCycle_sliderMin, fixedDutyCycle_sliderMax, t);
    }

    // Fixed Frequency Hz (normalized 0..1)
    public void ChangeFixedFreqHz(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        fixedFreqHz = Mathf.Lerp(fixedFreqHz_sliderMin, fixedFreqHz_sliderMax, t);
    }

    // Just Detectable Intensity (normalized 0..1)
    public void ChangeJustDetectableIntensity(float normalizedVal)
    {
        float t = Mathf.Clamp01(normalizedVal);
        justDetectableIntensity = Mathf.Lerp(justDetectableIntensity_sliderMin, justDetectableIntensity_sliderMax, t);
    }
}
