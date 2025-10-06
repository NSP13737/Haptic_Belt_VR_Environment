using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Distance_Caster : MonoBehaviour
{
    [SerializeField]
    Transform parentTransform;
    [SerializeField]
    int maxRayDist;
    [SerializeField]
    int rayCount;

    LayerMask layerMask; // To make sure casts only collide with walls
    


    private void Awake()
    {
        layerMask = LayerMask.GetMask("Boundary");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < rayCount; i++)
        {
            // Set up new Ray
            float angle = i * (360f / rayCount); // Get angle for each ray
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward; // Turn that angle into a vector
            Ray currentRay = new Ray(transform.position, direction);

            // Raycast
            RaycastHit currentHit;
            if (Physics.Raycast(currentRay, out currentHit, maxRayDist, layerMask))
            {
                Debug.DrawRay(currentRay.origin, currentRay.direction*currentHit.distance, Color.yellow);
                Debug.Log(currentHit.distance);
            }
            else
            {
                Debug.DrawRay(currentRay.origin, currentRay.direction * maxRayDist, Color.green);
                Debug.Log(currentHit.distance);
            }
        }

      
    }
}
