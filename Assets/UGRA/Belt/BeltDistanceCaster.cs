using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeltDistanceCaster : MonoBehaviour
{

    [SerializeField]
    float maxRayDist;
    [SerializeField]
    GameObject rayPrefab;
    
    int rayCount = 8;

    UDP_Manager udp;

    GameObject[] rayInstances = new GameObject[8];


    private float[] distancesBuffer = new float[8];


    private int combinedMask;

    private bool getRealtimeDistances = true;

    private void Awake()
    {
        combinedMask = (1 << LayerMask.NameToLayer("Boundary"))
                     | (1 << LayerMask.NameToLayer("realBoundary"));
        udp = GetComponent<UDP_Manager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rayCount; i++)
        {
            if ((i == 3) || (i == 4) || (i == 5))
            {
                continue;
            }
            GameObject rayInstance = Instantiate(rayPrefab, transform);
            rayInstances[i] = rayInstance;
        }

    }

    public void disableRealtimeDistance(bool disable)
    {
        getRealtimeDistances = !disable;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (getRealtimeDistances)
        {
            getAndSendDistances();
        }
        else { return; }
        
    }

    private void getAndSendDistances()
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint;
        for (int i = 0; i < rayCount; i++)
        {
            if ((i == 3) || (i == 4) || (i == 5))
            {
                continue;
            }
            LineRenderer raycastVisual = rayInstances[i].GetComponent<LineRenderer>();
            // Set up new Ray
            float angle = i * (360f / rayCount); // Get angle for each ray
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward; // Turn that angle into a vector with respect to forward facing direction (vector iterate in CCW direction)
            Ray currentRay = new Ray(transform.position, direction);

            // Raycast
            RaycastHit currentHit;
            if (Physics.Raycast(currentRay, out currentHit, maxRayDist, combinedMask))
            {
                distancesBuffer[i] = currentHit.distance;

                // Switch between these to visualize distance of the rays
                //endPoint = currentHit.point;
                endPoint = startPoint + (direction * maxRayDist);

                //Debug.DrawRay(currentRay.origin, currentRay.direction*currentHit.distance, Color.green);
                //Debug.Log(currentHit.distance);
            }
            else
            {
                distancesBuffer[i] = float.MaxValue; // Normally in this case currentRay.distance would return 0, but we want to send a very far distance for the microcontroller
                endPoint = startPoint + (direction * maxRayDist);
                //Debug.DrawRay(currentRay.origin, currentRay.direction * maxRayDist, Color.red);
                //Debug.Log(float.MaxValue);
            }
            raycastVisual.SetPosition(0, startPoint);
            raycastVisual.SetPosition(1, endPoint);
        }

        //Manually cutting out distances from back-left and back-right motors
        distancesBuffer[3] = float.MaxValue; //back left
        distancesBuffer[4] = float.MaxValue; // back
        distancesBuffer[5] = float.MaxValue; // back right

        //// Adding a two motor shift to array so that we can shift the belt (battery is now mounted on right side of body instead of back)
        //Array.Reverse(distancesBuffer);          // { 7, 6, 5, 4, 3, 2, 1, 0 }
        //Array.Reverse(distancesBuffer, 0, 6);    // { 2, 3, 4, 5, 6, 7, 1, 0 }
        //Array.Reverse(distancesBuffer, 6, distancesBuffer.Length - 6); // { 2, 3, 4, 5, 6, 7, 0, 1 }

        ////Further shifting values so that index 7 goes to 5, index 5 goes to 6, and index 6 goes to 7
        //var temp = distancesBuffer[7];
        //distancesBuffer[7] = distancesBuffer[6]; // 6 goes to 7
        //distancesBuffer[6] = distancesBuffer[5]; // 5 goes to 6
        //distancesBuffer[5] = temp;               // 7 goes to 5


        udp.setDistances(distancesBuffer);
    }
}