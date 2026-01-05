using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class headTrackerTools : MonoBehaviour
{
    // Source - https://stackoverflow.com/a
    // Posted by derHugo, modified by community. See post 'Timeline' for change history
    // Retrieved 2025-12-30, License - CC BY-SA 4.0
    // has been modified significantly by Nathan Phillips

    [SerializeField]
    float deadzone = 0.01f; // 10 millimeters
    [SerializeField] private GameObject head;

    // Stores the overall moved distance
    private float totalMovedDistance;
    private int collisionCount;

    // flag to start and stop tracking
    // Could also use a Coroutine if that fits you better
    private bool track;

    // Store position of last frame
    private Vector3 lastPos;

    private UnityEngine.XR.InputDevice device;

    public void BeginTrack()
    {
        // reset total value
        totalMovedDistance = 0;

        // store first position
        device = new UnityEngine.XR.InputDevice();
        device = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);

        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
        {
            position.y = 0; //remove player vertical head component
            lastPos = position;
            Debug.Log($"Tracking started at: {lastPos}");
        }

        // start tracking
        track = true;
    }

    public float EndTrack()
    {
        // stop tracking
        track = false;

        // whatever you want to do with the total distance now
        Debug.Log($"Total moved distance in local space: {totalMovedDistance}", this);
        return totalMovedDistance;
    }

    public void ToggleTrack(bool toTrack)
    {
        if (toTrack)
        {
            BeginTrack();
        }
        else
        {
            EndTrack();
        }
    }

    public void startHeadCollisionTracker()
    {
        collisionCount = 0;
    }
    public int stopHeadCollisionTracker()
    {
        return collisionCount;
    }

    private void incrementCollisionCount(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Boundary") || obj.layer == LayerMask.NameToLayer("RealBoundary"))
        {
            collisionCount++;
        }
    }

    private void Start()
    {
        HeadCollisionEvent broadcaster;
        //create and check components for head collision tracking
        if (head.TryGetComponent<HeadCollisionEvent>(out HeadCollisionEvent headCollisionEvent))
        {
            broadcaster = headCollisionEvent;
        }
        else
        {
            broadcaster = head.AddComponent<HeadCollisionEvent>();
        }
        if (!head.TryGetComponent<Collider>(out Collider _))
        {
            Debug.LogError("The head does not have a collider and will not log any collisions for logging");
        }

        broadcaster.OnHeadCollide += incrementCollisionCount;
        
    }
    private void Update()
    {
        updateTotalMoveDist();
    }

    private void updateTotalMoveDist()
    {
        // If not tracking do nothing
        if (!track) return;

        // get current hmd position
        Vector3 currentPos;
        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
        {
            position.y = 0; //remove vertical player head movement
            currentPos = position;
        }
        else
        {
            currentPos = lastPos;
        }

        // Get distance moved since last frame
        float thisFrameDistance = Vector3.Distance(currentPos, lastPos);

        // sum it up to the total value if change is greater than deadzone
        if (thisFrameDistance > deadzone)
        {
            totalMovedDistance += thisFrameDistance;
            // update the last position
            lastPos = currentPos;
        }
    }

}
