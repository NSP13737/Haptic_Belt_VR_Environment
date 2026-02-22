using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;

public class EggLogic : MonoBehaviour
{
    private EggBasketManager manager;
    private Grabbable objGrabber;
    private bool triggered = false;

    public void SetManager(EggBasketManager mgr) => manager = mgr;

    void Start()
    {
        objGrabber = GetComponent<Grabbable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("EggCollectionTrigger")) return;
        if (objGrabber.GrabPoints.Count == 0) return;

        triggered = true;

        Destroy(this.gameObject, 0.1f);

        manager.onEggCompletion(0.1f);
        
    }
}