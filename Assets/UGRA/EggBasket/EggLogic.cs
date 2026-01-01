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
        if (!other.CompareTag("Basket")) return;

        triggered = true;

        Destroy(this.gameObject, 0.5f);
        Destroy(other.gameObject, 0.5f);

        manager.onEggBasketCompletion(0.5f);
        
    }
}