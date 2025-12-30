using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPoint: MonoBehaviour
{

    [SerializeField]
    private Transform centerTarget;

    void Update()
    {
        this.transform.LookAt(centerTarget);
    }
}
