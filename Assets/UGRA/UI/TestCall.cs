using UnityEngine;

public class TestCall : MonoBehaviour
{
    // A reference to the Renderer component of this GameObject.
    private Renderer cubeRenderer;

    void Start()
    {
        // Get the Renderer component when the scene starts.
        cubeRenderer = GetComponent<Renderer>();
    }

    // The simple function to call.
    // 'public' is important so it can be called from outside this script.
    public void ChangeToRed()
    {
        // Check if the renderer exists before trying to use it.
        if (cubeRenderer != null)
        {
            // Change the main color of the material to bright red.
            cubeRenderer.material.color = Color.red;

            // Optional: Log a message to the console to confirm the call
            Debug.Log("Function 'ChangeToRed' was successfully called! Color is now red.");
        }
    }

    public void ChangeToBlue()
    {
        // Check if the renderer exists before trying to use it.
        if (cubeRenderer != null)
        {
            // Change the main color of the material to bright red.
            cubeRenderer.material.color = Color.blue;

            // Optional: Log a message to the console to confirm the call
            Debug.Log("Function 'ChangeToBlue' was successfully called! Color is now blue.");
        }
    }

    public void TestSlider(float sliderVal)
    {
        Debug.Log(sliderVal);
    }
}