using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButtonsOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject MainButton;
    [SerializeField] private GameObject ConfirmationButton;
    [SerializeField] private string targetTag = "MainCamera";

    // Start is called before the first frame update
    void Start()
    {
        MainButton.SetActive(false);
        ConfirmationButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.tag, other.gameObject);
        // Check if the object that entered has the correct tag
        if (other.CompareTag(targetTag))
        {
            MainButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            MainButton.SetActive(false);
            ConfirmationButton.SetActive(false);
        }
    }

}
