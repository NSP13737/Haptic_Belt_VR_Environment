using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FadeHandler : MonoBehaviour
{
    [SerializeField]
    private FadeEffect FadeEffect;

    [SerializeField]
    private bool reverseLogic = false;

    [SerializeField]
    string layerNameForMask = "realBoundary";

    [SerializeField]
    private Transform parentTransform;

    private LayerMask layerMask;

    void Awake()
    {
        layerMask = LayerMask.GetMask(layerNameForMask);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = parentTransform.position;
        origin.y = 0; //set so that player height doesn't matter
        float fadeDistance = 0.001f; //this is very small so that you effectively have to be inside the wall, since we are already accounting for safety w/ safety bounds
        Collider[] hitColliders = Physics.OverlapSphere(origin, fadeDistance, layerMask);

        if (hitColliders.Length > 0 )
        {
            FadeEffect.Fade(reverseLogic ? false : true);
        }
        else
        {
            FadeEffect.Fade(reverseLogic ? true : false);
        }

        //Debug.Log(hitColliders);
    }

}
