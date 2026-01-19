
using Meta.WitAi.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

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
    float minActivationDist_sliderMax = 0.5f;
    [SerializeField, Range(0, 5)]
    float minActivationDist = 0.0f;

    float maxActivationDist_sliderMin = 0.0f;
    float maxActivationDist_sliderMax = 2.0f;
    [SerializeField, Range(0, 5)]
    float maxActivationDist = 5.0f;

    // -----------------------------
    // Frequency (Hz)
    // -----------------------------
    float minFreqHz_sliderMin = 0.1f;
    float minFreqHz_sliderMax = 50.0f;
    [SerializeField, Range(0, 50)]
    float minFreqHz = 0.0f;

    float maxFreqHz_sliderMin = 0.0f;
    float maxFreqHz_sliderMax = 40.0f;
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
    float fixedFreqHz_sliderMax = 40.0f;
    [SerializeField, Range(0, 50)]
    float fixedFreqHz = 10.0f;

    // -----------------------------
    // Just Detectable Intensity (0..1)
    // -----------------------------
    float justDetectableIntensity_sliderMin = 0.0f;
    float justDetectableIntensity_sliderMax = 1.0f;
    [SerializeField, Range(0, 1)]
    float justDetectableIntensity = 0.2f;

    // -----------------------------
    // Events
    // -----------------------------
    [Header("Events")]
    [SerializeField] private FloatEvent onChangeCondition;
    [SerializeField] private FloatEvent onChangeMinActivationDist;
    [SerializeField] private FloatEvent onChangeMaxActivationDist;
    [SerializeField] private FloatEvent onChangeMinFreqHz;
    [SerializeField] private FloatEvent onChangeMaxFreqHz;
    [SerializeField] private FloatEvent onChangeFixedDutyCycle;
    [SerializeField] private FloatEvent onChangeFixedFreqHz;
    [SerializeField] private FloatEvent onChangeJustDetectableIntensity;

    static float[] studyParamsBuffer = new float[8];

    private void Awake()
    {
        udp = GetComponent<UDP_Manager>();
    }

    void Update()
    {
        studyParamsBuffer[0] = conditionSelection;
        studyParamsBuffer[1] = minActivationDist;
        studyParamsBuffer[2] = maxActivationDist;
        studyParamsBuffer[3] = minFreqHz;
        studyParamsBuffer[4] = maxFreqHz;
        studyParamsBuffer[5] = fixedDutyCycle;
        studyParamsBuffer[6] = fixedFreqHz;
        studyParamsBuffer[7] = justDetectableIntensity;

        udp.setStudyParams(studyParamsBuffer);
    }

    // =========================
    // Public API (Normalized Input)
    // =========================

    public void changeCondition()
    {
        conditionSelection = (conditionSelection >= 2.0f) ? 1.0f : 2.0f;
        onChangeCondition?.Invoke(conditionSelection);
    }

    public void ChangeMinActivationDist(float normalizedVal)
    {
        minActivationDist = Mathf.Lerp(minActivationDist_sliderMin, minActivationDist_sliderMax, normalizedVal);
        onChangeMinActivationDist?.Invoke(minActivationDist);
    }

    public void ChangeMaxActivationDist(float normalizedVal)
    {
        maxActivationDist = Mathf.Lerp(maxActivationDist_sliderMin, maxActivationDist_sliderMax, normalizedVal);
        onChangeMaxActivationDist?.Invoke(maxActivationDist);
    }

    public void ChangeMinFreqHz(float normalizedVal)
    {
        minFreqHz = Mathf.Lerp(minFreqHz_sliderMin, minFreqHz_sliderMax, normalizedVal);
        onChangeMinFreqHz?.Invoke(minFreqHz);
    }

    public void ChangeMaxFreqHz(float normalizedVal)
    {
        maxFreqHz = Mathf.Lerp(maxFreqHz_sliderMin, maxFreqHz_sliderMax, normalizedVal);
        onChangeMaxFreqHz?.Invoke(maxFreqHz);
    }

    public void ChangeFixedDutyCycle(float normalizedVal)
    {
        fixedDutyCycle = Mathf.Lerp(fixedDutyCycle_sliderMin, fixedDutyCycle_sliderMax, normalizedVal);
        onChangeFixedDutyCycle?.Invoke(fixedDutyCycle);
    }

    public void ChangeFixedFreqHz(float normalizedVal)
    {
        fixedFreqHz = Mathf.Lerp(fixedFreqHz_sliderMin, fixedFreqHz_sliderMax, normalizedVal);
        onChangeFixedFreqHz?.Invoke(fixedFreqHz);
    }

    public void ChangeJustDetectableIntensity(float normalizedVal)
    {
        justDetectableIntensity = Mathf.Lerp(justDetectableIntensity_sliderMin, justDetectableIntensity_sliderMax, normalizedVal);
        onChangeJustDetectableIntensity?.Invoke(justDetectableIntensity);
    }
    public void DeltaChangeJustDetectableIntensity(float instensityDelta)
    {
        float tmp = udp.getParam_JustNoticableIntensity() + instensityDelta;

        //clamp tmp to normalized range
        if (tmp < 0)
        {
            tmp = 0f;
        }
        else if (tmp > 1)
        {
            tmp = 1;
        }
        
        justDetectableIntensity = Mathf.Lerp(justDetectableIntensity_sliderMin, justDetectableIntensity_sliderMax, tmp);
        onChangeJustDetectableIntensity?.Invoke(justDetectableIntensity);
    }
}
