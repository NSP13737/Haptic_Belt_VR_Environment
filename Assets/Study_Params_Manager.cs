using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Study_Params_Manager : MonoBehaviour
{

    UDP_Manager udp;

    [SerializeField]
    float numDrivers;
    [SerializeField]
    float conditionSelection;
    [SerializeField]
    float minActivationDist;
    [SerializeField]
    float maxActivationDist;
    [SerializeField]
    float minFreqHz;
    [SerializeField]
    float maxFreqHz;
    [SerializeField]
    float fixedDutyCycle;
    [SerializeField]
    float fixedPeriodMs;

    private void Awake()
    {
        udp = GetComponent<UDP_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        float[] studyParamsBuffer =
            {numDrivers,
            conditionSelection,
            minActivationDist,
            maxActivationDist,
            minFreqHz,
            maxFreqHz,
            fixedDutyCycle,
            fixedPeriodMs};

        udp.setStudyParams(studyParamsBuffer);
    }
}
