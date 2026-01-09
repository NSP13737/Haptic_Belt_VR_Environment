using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadTrackerTools : MonoBehaviour
{
    // Source - https://stackoverflow.com/a
    // Posted by derHugo, modified by community. See post 'Timeline' for change history
    // Retrieved 2025-12-30, License - CC BY-SA 4.0
    // has been modified significantly by Nathan Phillips

    [SerializeField]
    float movementTrackingDeadzone = 0.01f; // 10 millimeters
    [Tooltip("This can be anything on the head, but CenterEyeAnchor probably is best?")]
    [SerializeField] private GameObject head;
    [Tooltip("This tells us how long we should wait before checking for collisions again (like i-frames)")]
    [SerializeField] private float collisionBufferTime;

    // Stores the overall moved distance
    private float totalMovedDistance;
    private int collisionCount;

    // flag to start and stop tracking
    // Could also use a Coroutine if that fits you better
    private bool track = false;

    // Store position of last frame
    private Vector3 lastPos;

    private UnityEngine.XR.InputDevice device;

    public void BeginDistanceTracking()
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
        }
        else
        {
            Debug.LogError("Could not get device position to begin head distance tracking", this);
        }

            // start tracking
            track = true;
    }

    public float EndDistanceTracking()
    {
        // stop tracking
        track = false;

        // whatever you want to do with the total distance now
        //Debug.Log($"Total moved distance in local space: {totalMovedDistance}", this);
        return totalMovedDistance;
    }

    public void ToggleDistanceTracking(bool toTrack)
    {
        if (toTrack)
        {
            BeginDistanceTracking();
        }
        else
        {
            EndDistanceTracking();
        }
    }

    public void StartHeadCollisionTracker()
    {
        collisionCount = 0;
    }
    public int StopHeadCollisionTracker()
    {
        return collisionCount;
    }

    private float lastCollisionTime;
    private void IncrementCollisionCount(GameObject obj)
    {

        if (obj.layer == LayerMask.NameToLayer("Boundary") || obj.layer == LayerMask.NameToLayer("realBoundary"))
        {
            if ((Time.time - lastCollisionTime) >= collisionBufferTime)
            {
                collisionCount++;
                lastCollisionTime = Time.time;
            }
            
            
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
        if (!head.TryGetComponent<Rigidbody>(out Rigidbody _))
        {
            Debug.LogError("The head does not have a rigid body component (required for collision count tracking)");
        }

        broadcaster.OnHeadCollide += IncrementCollisionCount;
        
    }
    private void Update()
    {
        // If not tracking do nothing
        if (!track) return;
        updateTotalMoveDist();
    }

    private void updateTotalMoveDist()
    {

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

        // sum it up to the total value if change is greater than movementTrackingDeadzone
        if (thisFrameDistance > movementTrackingDeadzone)
        {
            totalMovedDistance += thisFrameDistance;
            // update the last position
            lastPos = currentPos;
        }
    }

}
