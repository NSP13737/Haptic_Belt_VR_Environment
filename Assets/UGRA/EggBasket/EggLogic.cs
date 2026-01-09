using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EggLogic : MonoBehaviour
{
    private EggBasketManager manager;
    private bool triggered = false;

    public void SetManager(EggBasketManager mgr) => manager = mgr;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("EggCollectionTrigger")) return;

        triggered = true;

        Destroy(this.gameObject, 0.5f);

        manager.onEggCompletion(0.5f);
        
    }
}