using UnityEngine;

[DisallowMultipleComponent]
public class Belt_Anchor_Locomotion : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The transform to follow.")]
    public Transform target;

    [Header("Position Settings")]
    [Tooltip("Fixed world-space Y height to maintain.")]
    public float fixedY = 1.8f;

    [Tooltip("Offset applied on XZ plane relative to the target (world space). Y is ignored.")]
    public Vector3 positionOffset = Vector3.zero;

    [Tooltip("Smoothly interpolate position toward the target.")]
    public bool smoothPosition = false;

    [Tooltip("Lerp speed for position when smoothing is enabled.")]
    [Range(0.1f, 20f)]
    public float positionLerpSpeed = 8f;

    [Header("Rotation Settings (Yaw Only)")]
    [Tooltip("If enabled, this object will match the target's Y rotation (yaw).")]
    public bool matchYaw = true;

    [Tooltip("Additional yaw (in degrees) added after matching the target's yaw.")]
    public float yawOffsetDegrees = 0f;

    [Tooltip("Smoothly slerp rotation toward the target yaw.")]
    public bool smoothRotation = false;

    [Tooltip("Slerp speed for rotation when smoothing is enabled.")]
    [Range(0.1f, 20f)]
    public float rotationLerpSpeed = 10f;

    [Header("Update Timing")]
    [Tooltip("Use LateUpdate to follow after target movement (recommended).")]
    public bool useLateUpdate = true;

    void Update()
    {
        if (!useLateUpdate)
        {
            Follow();
        }
    }

    void LateUpdate()
    {
        if (useLateUpdate)
        {
            Follow();
        }
    }

    private void Follow()
    {
        if (target == null) return;

        // --- Position: inherit XZ from target, keep fixed Y
        Vector3 desired = target.position;
        desired.y = fixedY;

        // Apply XZ offset (ignore offset.y)

        Vector3 planarOffset = target.right * positionOffset.x + target.forward * positionOffset.z;
        desired += new Vector3(planarOffset.x, 0f, planarOffset.z);


        if (smoothPosition)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                1f - Mathf.Exp(-positionLerpSpeed * Time.deltaTime)
            );
        }
        else
        {
            transform.position = desired;
        }

        // --- Rotation: yaw only
        if (matchYaw)
        {
            // Extract target yaw
            float targetYaw = target.eulerAngles.y + yawOffsetDegrees;
            Quaternion desiredRot = Quaternion.Euler(0f, targetYaw, 0f);

            if (smoothRotation)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    desiredRot,
                    1f - Mathf.Exp(-rotationLerpSpeed * Time.deltaTime)
                );
            }
            else
            {
                transform.rotation = desiredRot;
            }
        }
        else
        {
            // Lock roll & pitch even if not matching yaw
            Vector3 e = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, e.y, 0f);
        }
    }

    // Optional: Draw a gizmo to show fixed height and offset
    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = new Color(0.2f, 0.7f, 1f, 0.6f);
        Vector3 gizmoPos = new Vector3(
            target.position.x + positionOffset.x,
            fixedY,
            target.position.z + positionOffset.z
        );
        Gizmos.DrawWireSphere(gizmoPos, 0.15f);
        Gizmos.DrawLine(gizmoPos, new Vector3(gizmoPos.x, target.position.y, gizmoPos.z));
    }
}
