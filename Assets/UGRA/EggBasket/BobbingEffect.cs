using UnityEngine;
using Oculus.Interaction;

public class BobbingEffect : MonoBehaviour
{
    private Grabbable objGrabber;

    [Header("Bobbing Settings")]
    [SerializeField] float bobAmplitude = 0.5f;
    [SerializeField] float bobFrequency = 1f;
    
    private float phaseOffset = 0f;

    [Header("Rotation Settings")]
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        objGrabber = GetComponent<Grabbable>();
    }

    void Update()
    {
        // 1. Handle Bobbing with Phase Offset
        // Formula: y = startY + sin(time * frequency + offset) * amplitude
        float newY = startPosition.y + Mathf.Sin((Time.time * bobFrequency) + phaseOffset) * bobAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 2. Handle Rotation
        transform.Rotate(rotationSpeed * Time.deltaTime);

        //If egg has been grabbed, disable rotation
        if (objGrabber.GrabPoints.Count > 0)
        {
            this.enabled = false;
        }
    }
}