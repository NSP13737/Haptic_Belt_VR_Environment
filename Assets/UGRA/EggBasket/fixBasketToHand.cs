using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class fixBasketToHand : MonoBehaviour
{
    private Grabbable grabbable;
    private bool hasBeenGrabbed = false;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();

        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (hasBeenGrabbed) return;

        if (evt.Type == PointerEventType.Select)
        {
            hasBeenGrabbed = true;

            MonoBehaviour interactor = evt.Data as MonoBehaviour;
            if (interactor != null)
            {
                // 1. Handle Parenting Logic
                if (transform.parent != null)
                {
                    transform.parent.SetParent(interactor.transform, true);
                }
                else
                {
                    Debug.LogWarning("No parent found! Parenting the grabbed object directly.");
                    transform.SetParent(interactor.transform, true);
                }

                // 2. Disable the Mesh Collider on THIS object (the child)
                MeshCollider meshCol = GetComponent<MeshCollider>();
                if (meshCol != null)
                {
                    meshCol.enabled = false;
                }

                // 3. Destroy the Rigidbody on THIS object
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Destroy(rb);
                }
            }

            // 4. Disable the Grabbable component
            grabbable.enabled = false;
        }
    }

    void OnDestroy()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised -= HandlePointerEvent;
        }
    }
}