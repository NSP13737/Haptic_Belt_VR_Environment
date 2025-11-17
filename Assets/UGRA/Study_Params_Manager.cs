using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Study_Params_Manager : MonoBehaviour
{

    UDP_Manager udp;

    [SerializeField]
    [Range(1,2)]
    float conditionSelection;
    [SerializeField]
    [Range(0, 5)]
    float minActivationDist;
    [SerializeField]
    [Range(0,5)]
    float maxActivationDist;
    [SerializeField]
    [Range(0,50)]
    float minFreqHz;
    [SerializeField]
    [Range(0,50)]
    float maxFreqHz;
    [SerializeField]
    [Range(0,1)]
    float fixedDutyCycle;
    [SerializeField]
    [Range(0,50)]
    float fixedFreqHz;
    [SerializeField]
    [Range(0, 1)]
    float just_detectable_intensity;

    private void Awake()
    {
        udp = GetComponent<UDP_Manager>();
    }

    // Update is called once per frame
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
            just_detectable_intensity
            };

        udp.setStudyParams(studyParamsBuffer);
    }
}
