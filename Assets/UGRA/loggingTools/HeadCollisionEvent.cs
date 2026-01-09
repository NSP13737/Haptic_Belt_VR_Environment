using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionEvent : MonoBehaviour
{
    public System.Action<GameObject> OnHeadCollide;

    private void OnTriggerEnter(Collider collider)
    {
        OnHeadCollide?.Invoke(collider.gameObject);
    }
}
