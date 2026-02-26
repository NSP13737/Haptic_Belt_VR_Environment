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

    // --- DIRTY FLAG ---
    // Set to true by default so it sends the initial state on the first frame
    private bool _requiresUpdate = true;

    [Header("Condition")]
    [SerializeField, Range(1, 2)]
    static float conditionSelection = 1.0f;

    // -----------------------------
    // Activation Distance (meters)
    // -----------------------------
    float minActivationDist_sliderMin = 0.0f;
    float minActivationDist_sliderMax = 0.5f;
    [SerializeField, Range(0, 5)]
    static float minActivationDist = 0.0f;

    float maxActivationDist_sliderMin = 0.0f;
    float maxActivationDist_sliderMax = 2.0f;
    [SerializeField, Range(0, 5)]
    static float maxActivationDist = 5.0f;

    // -----------------------------
    // Frequency (Hz)
    // -----------------------------
    float minFreqHz_sliderMin = 0.1f;
    float minFreqHz_sliderMax = 50.0f;
    [SerializeField, Range(0, 50)]
    static float minFreqHz = 0.0f;

    float maxFreqHz_sliderMin = 0.0f;
    float maxFreqHz_sliderMax = 40.0f;
    [SerializeField, Range(0, 50)]
    static float maxFreqHz = 50.0f;

    // -----------------------------
    // Fixed Duty Cycle (0..1)
    // -----------------------------
    float fixedDutyCycle_sliderMin = 0.0f;
    float fixedDutyCycle_sliderMax = 1.0f;
    [SerializeField, Range(0, 1)]
    static float fixedDutyCycle = 0.5f;

    // -----------------------------
    // Fixed Frequency (Hz)
    // -----------------------------
    float fixedFreqHz_sliderMin = 0.0f;
    float fixedFreqHz_sliderMax = 40.0f;
    [SerializeField, Range(0, 50)]
    static float fixedFreqHz = 10.0f;

    // -----------------------------
    // Just Detectable Intensity (0..1)
    // -----------------------------
    float justDetectableIntensity_sliderMin = 0.0f;
    float justDetectableIntensity_sliderMax = 1.0f;
    [SerializeField, Range(0, 1)]
    static float justDetectableIntensity = 0.0f;

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

    // Triggers automatically if you change a value in the Unity Inspector
    private void OnValidate()
    {
        _requiresUpdate = true;
    }

    void Update()
    {
        // Skip the update entirely if nothing has changed
        if (!_requiresUpdate) return;

        studyParamsBuffer[0] = conditionSelection;
        studyParamsBuffer[1] = minActivationDist;
        studyParamsBuffer[2] = maxActivationDist;
        studyParamsBuffer[3] = minFreqHz;
        studyParamsBuffer[4] = maxFreqHz;
        studyParamsBuffer[5] = fixedDutyCycle;
        studyParamsBuffer[6] = fixedFreqHz;
        studyParamsBuffer[7] = justDetectableIntensity;

        udp.setStudyParams(studyParamsBuffer);

        // Reset the flag after sending
        _requiresUpdate = false;
    }

    // =========================
    // Public API
    // =========================

    public void changeCondition()
    {
        conditionSelection = (conditionSelection >= 2.0f) ? 1.0f : 2.0f;

        if (conditionSelection == 1.0f)
        {
            ChangeMinActivationDist(0.35f, true);
            ChangeMaxActivationDist(2.0f, true);
            ChangeMinFreqHz(2.5f, true);
            ChangeMaxFreqHz(15.5f, true);
            ChangeFixedDutyCycle(0.45f, true);
        }
        else if (conditionSelection == 2.0f)
        {
            ChangeMinActivationDist(0.35f, true);
            ChangeMaxActivationDist(2.0f, true);
            ChangeFixedFreqHz(3.0f, true);
        }

        onChangeCondition?.Invoke(conditionSelection);
        _requiresUpdate = true;
    }
    public void changeCondition(float condition)
    {
        conditionSelection = condition;

        if (conditionSelection == 1.0f)
        {
            ChangeMinActivationDist(0.35f, true);
            ChangeMaxActivationDist(2.0f, true);
            ChangeMinFreqHz(2.5f, true);
            ChangeMaxFreqHz(15.5f, true);
            ChangeFixedDutyCycle(0.45f, true);
        }
        else if (conditionSelection == 2.0f)
        {
            ChangeMinActivationDist(0.35f, true);
            ChangeMaxActivationDist(2.0f, true);
            ChangeFixedFreqHz(3.0f, true);
        }

        onChangeCondition?.Invoke(conditionSelection);
        _requiresUpdate = true;
    }

    public void ChangeMinActivationDist(float val, bool useRawValue = false)
    {
        minActivationDist = useRawValue
            ? Mathf.Clamp(val, minActivationDist_sliderMin, minActivationDist_sliderMax)
            : Mathf.Lerp(minActivationDist_sliderMin, minActivationDist_sliderMax, val);

        onChangeMinActivationDist?.Invoke(minActivationDist);
        _requiresUpdate = true;
    }

    public void ChangeMaxActivationDist(float val, bool useRawValue = false)
    {
        maxActivationDist = useRawValue
            ? Mathf.Clamp(val, maxActivationDist_sliderMin, maxActivationDist_sliderMax)
            : Mathf.Lerp(maxActivationDist_sliderMin, maxActivationDist_sliderMax, val);

        onChangeMaxActivationDist?.Invoke(maxActivationDist);
        _requiresUpdate = true;
    }

    public void ChangeMinFreqHz(float val, bool useRawValue = false)
    {
        minFreqHz = useRawValue
            ? Mathf.Clamp(val, minFreqHz_sliderMin, minFreqHz_sliderMax)
            : Mathf.Lerp(minFreqHz_sliderMin, minFreqHz_sliderMax, val);

        onChangeMinFreqHz?.Invoke(minFreqHz);
        _requiresUpdate = true;
    }

    public void ChangeMaxFreqHz(float val, bool useRawValue = false)
    {
        maxFreqHz = useRawValue
            ? Mathf.Clamp(val, maxFreqHz_sliderMin, maxFreqHz_sliderMax)
            : Mathf.Lerp(maxFreqHz_sliderMin, maxFreqHz_sliderMax, val);

        onChangeMaxFreqHz?.Invoke(maxFreqHz);
        _requiresUpdate = true;
    }

    public void ChangeFixedDutyCycle(float val, bool useRawValue = false)
    {
        fixedDutyCycle = useRawValue
            ? Mathf.Clamp(val, fixedDutyCycle_sliderMin, fixedDutyCycle_sliderMax)
            : Mathf.Lerp(fixedDutyCycle_sliderMin, fixedDutyCycle_sliderMax, val);

        onChangeFixedDutyCycle?.Invoke(fixedDutyCycle);
        _requiresUpdate = true;
    }

    public void ChangeFixedFreqHz(float val, bool useRawValue = false)
    {
        fixedFreqHz = useRawValue
            ? Mathf.Clamp(val, fixedFreqHz_sliderMin, fixedFreqHz_sliderMax)
            : Mathf.Lerp(fixedFreqHz_sliderMin, fixedFreqHz_sliderMax, val);

        onChangeFixedFreqHz?.Invoke(fixedFreqHz);
        _requiresUpdate = true;
    }

    public void ChangeJustDetectableIntensity(float normalizedVal)
    {
        justDetectableIntensity = Mathf.Lerp(justDetectableIntensity_sliderMin, justDetectableIntensity_sliderMax, normalizedVal);
        onChangeJustDetectableIntensity?.Invoke(justDetectableIntensity);
        _requiresUpdate = true;
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
        _requiresUpdate = true;
    }
}