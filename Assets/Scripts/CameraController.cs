using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;         // The kart
    public Rigidbody targetRb;       // Rigidbody on the kart (for look-ahead, optional)

    [Header("Follow Settings")]
    public Vector3 followOffset = new Vector3(0f, 4f, -8f); // Relative to kart's local space
    public float positionSmoothTime = 0.15f;

    [Header("Rotation Settings")]
    public float rotationSmoothSpeed = 10f;
    public float lookAheadDistance = 4f;   // How far ahead camera looks based on velocity
    public float minLookAheadSpeed = 1f;   // Minimum speed before look-ahead kicks in

    private Vector3 _currentVelocity;

    void Reset()
    {
        // Auto-fill if script is added in editor
        if (!target && transform.parent != null)
            target = transform.parent;

        if (target && !targetRb)
            targetRb = target.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (!target) return;

        // --- POSITION ---
        // Desired position is target position + target's rotation * offset
        Vector3 desiredPosition = target.TransformPoint(followOffset);

        // Smoothly move camera to desired position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref _currentVelocity,
            positionSmoothTime
        );

        // --- ROTATION / LOOK DIRECTION ---
        Vector3 lookTargetPos = target.position;

        // If we have a Rigidbody and are moving, look slightly ahead
        if (targetRb && targetRb.linearVelocity.magnitude > minLookAheadSpeed)
        {
            Vector3 velDir = targetRb.linearVelocity.normalized;
            lookTargetPos += velDir * lookAheadDistance;
        }

        // Direction from camera to look target
        Vector3 direction = (lookTargetPos - transform.position).normalized;
        if (direction.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Smoothly rotate towards target
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
