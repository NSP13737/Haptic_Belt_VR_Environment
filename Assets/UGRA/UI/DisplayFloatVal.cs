using UnityEngine;
using TMPro;
using System; // Include this to access TextMeshPro components

public class TextMeshUpdater : MonoBehaviour
{
    private TMP_Text textComponent; // Reference to the TextMeshPro component

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    // Method to update text dynamically
    public void UpdateText(float floatVal)
    {
        if (textComponent != null)
        {
            textComponent.text = floatVal.ToString("F3"); //get float val and output first 3 decimals
        }
    }
}