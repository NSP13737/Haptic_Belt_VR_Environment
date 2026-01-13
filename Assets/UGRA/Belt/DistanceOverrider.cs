using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class DistanceOverrider : MonoBehaviour
{

    [SerializeField] private GameObject flagGO; //flagGO is the GO that when active in the scene, triggers the overriding of realtime distance getting
    [SerializeField] private UDP_Manager udpManager;
    [SerializeField] private BeltDistanceCaster beltDistanceCaster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flagGO.activeInHierarchy)
        {
            beltDistanceCaster.disableRealtimeDistance(true);
            float setDist = udpManager.getParam_MaxDistance()*0.999f; //making 99% of max val bc at max val user will never feel it
            float[] fakeDistances = new float[8];
            for (int i = 0; i < fakeDistances.Length; i++)
            {
                fakeDistances[i] = setDist;
            }
            udpManager.setDistances(fakeDistances); //updating this every frame isn't effecient, but it works
            Debug.LogError($"Distances; {fakeDistances}");
        }
        else // Once the intensity UI has been disabled, reenable realtime distance sensing and disable this checker so that it isn't running in the BG
        {
            beltDistanceCaster.disableRealtimeDistance(false);
            this.gameObject.SetActive(false);
        }
    }
}
