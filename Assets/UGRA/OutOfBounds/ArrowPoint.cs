using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPoint: MonoBehaviour
{

    [SerializeField]
    private Transform centerTarget;

    void Update()
    {
        Vector3 direction = centerTarget.position - transform.position;
        direction.y = 0; //do not change  "pitch"  of arrow
        this.transform.rotation = Quaternion.LookRotation(direction);
    }
}
