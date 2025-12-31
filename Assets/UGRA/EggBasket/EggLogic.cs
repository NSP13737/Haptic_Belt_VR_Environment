using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EggLogic : MonoBehaviour
{
    public UnityEvent eggCollect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
        if (other.CompareTag("Basket"))
        {
            Destroy(this.gameObject, 0.5f);
            Destroy(other.gameObject, 0.5f);
        }
        eggCollect.Invoke();
    }
}
