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
            float setDist = udpManager.getParam_MaxDistance();
            float[] fakeDistances = new float[8];
            for (int i = 0; i < fakeDistances.Length; i++)
            {
                fakeDistances[i] = setDist;
            }
            udpManager.setDistances(fakeDistances); //updating this every frame isn't effecient, but it works
        }
        else
        {
            beltDistanceCaster.disableRealtimeDistance(false); //calling this every frame is pretty bad, but it works
        }
    }
}
