using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Distance_Caster : MonoBehaviour
{

    [SerializeField]
    int maxRayDist;
    [SerializeField]
    int rayCount;

    LayerMask layerMask; // To make sure casts only collide with walls
    UDP_Manager udp;
    


    private void Awake()
    {
        layerMask = LayerMask.GetMask("Boundary");
        udp = GetComponent<UDP_Manager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float[] floatBuffer = new float[8];

        for (int i = 0; i < rayCount; i++)
        {
            // Set up new Ray
            float angle = i * (360f / rayCount); // Get angle for each ray
            Vector3 direction = Quaternion.Euler(0, -angle, 0) * transform.forward; // Turn that angle into a vector with respect to forward facing direction (vector iterate in CCW direction)
            Ray currentRay = new Ray(transform.position, direction);

            // Raycast
            RaycastHit currentHit;
            if (Physics.Raycast(currentRay, out currentHit, maxRayDist, layerMask))
            {
                floatBuffer[i] = currentHit.distance;
                Debug.DrawRay(currentRay.origin, currentRay.direction*currentHit.distance, Color.green);
                //Debug.Log(currentHit.distance);
            }
            else
            {
                floatBuffer[i] = float.MaxValue; // Normally in this case currentRay.distance would return 0, but we want to send a very far distance for the microcontroller
                Debug.DrawRay(currentRay.origin, currentRay.direction * maxRayDist, Color.red);
                //Debug.Log(float.MaxValue);
            }
        }

        udp.setFloats(floatBuffer);

        

      
    }
}
