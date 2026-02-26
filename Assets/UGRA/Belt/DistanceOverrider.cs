using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DistanceOverrider : MonoBehaviour
{
    [SerializeField] private GameObject flagGO; // flagGO is the GO that when active in the scene, triggers the overriding of realtime distance getting
    [SerializeField] private UDP_Manager udpManager;
    [SerializeField] private Study_Params_Manager paramsManager;
    [SerializeField] private BeltDistanceCaster beltDistanceCaster;

    // --- NEW CHANGE: STATE TRACKING ---
    private bool wasSwitchedToTwo = false;
    // ----------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (flagGO.activeInHierarchy)
        {
            // --- NEW CHANGE: MONITOR AND OVERRIDE ---
            // If the value hits 2.0, record that it happened and force it back to 1.0
            if (udpManager.getParam_Condition() == 2.0f)
            {
                wasSwitchedToTwo = true;
                paramsManager.changeCondition(1.0f);
            }
            // -----------------------------------------

            beltDistanceCaster.disableRealtimeDistance(true);
            float setDist = udpManager.getParam_MaxDistance() * 0.999f;
            float[] fakeDistances = new float[8];
            for (int i = 0; i < fakeDistances.Length; i++)
            {
                fakeDistances[i] = setDist;
            }
            udpManager.setDistances(fakeDistances);
        }
        else
        {
            // --- NEW CHANGE: CONDITIONAL RESTORE ---
            // Only switch back to 2.0 if we detected a 2.0 during the menu's active state
            if (wasSwitchedToTwo)
            {
                paramsManager.changeCondition(2.0f);
                wasSwitchedToTwo = false; // Reset the flag
            }
            // ----------------------------------------

            beltDistanceCaster.disableRealtimeDistance(false);
            this.gameObject.SetActive(false);
        }
    }
}