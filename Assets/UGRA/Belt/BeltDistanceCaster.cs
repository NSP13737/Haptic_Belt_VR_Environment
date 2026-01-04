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
            LineRenderer raycastVisual = rayInstances[i].GetComponent<LineRenderer>();
            // Set up new Ray
            float angle = i * (360f / rayCount); // Get angle for each ray
            Vector3 direction = Quaternion.Euler(0, -angle, 0) * transform.forward; // Turn that angle into a vector with respect to forward facing direction (vector iterate in CCW direction)
            Ray currentRay = new Ray(transform.position, direction);

            // Raycast
            RaycastHit currentHit;
            if (Physics.Raycast(currentRay, out currentHit, maxRayDist, combinedMask))
            {
                distancesBuffer[i] = currentHit.distance;
                endPoint = currentHit.point;
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

        udp.setDistances(distancesBuffer);
    }
}
