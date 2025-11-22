using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;             // Player kart transform
    public Rigidbody targetRigidbody;    // Player kart rigidbody (optional but recommended)

    [Header("Offset")]
    public Vector3 localOffset = new Vector3(0f, 3f, -6f); // relative to kart
    public float speedBackOffset = 0.1f; // how much to push camera back based on speed
    public float maxBackOffset = 4f;     // clamp for speed-based push back

    [Header("Smoothing")]
    public float positionSmoothTime = 0.12f;
    public float rotationSmoothSpeed = 10f;

    [Header("Camera Collision")]
    public float collisionRadius = 0.3f;
    public float minDistance = 1.5f;     // minimum distance from kart
    public LayerMask collisionMask = ~0; // what the camera should collide with

    private Vector3 currentVelocity;

    private void Reset()
    {
        // Try to auto-assign target if not set
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            target = player.transform;
            targetRigidbody = player.GetComponent<Rigidbody>();
        }
    }

    private void LateUpdate()
{
    if (!target) return;

    if (targetRigidbody == null)
        targetRigidbody = target.GetComponent<Rigidbody>();

    // --- Desired offset based on speed ---
    float speed = targetRigidbody ? targetRigidbody.linearVelocity.magnitude : 0f;
    float extraBack = Mathf.Clamp(speed * speedBackOffset, 0f, maxBackOffset);

    Vector3 dynamicLocalOffset = localOffset + new Vector3(0f, 0f, -extraBack);

    // Offset in world space (relative to kart orientation)
    Vector3 desiredWorldOffset = target.TransformDirection(dynamicLocalOffset);
    Vector3 desiredPos = target.position + desiredWorldOffset;

    // --- Collision handling (avoid walls / track geometry) ---
    Vector3 focusPoint = target.position + Vector3.up * 1.0f; // ray origin a bit above kart
    Vector3 toCamera = desiredPos - focusPoint;
    float dist = toCamera.magnitude;
    Vector3 dir = toCamera.normalized;

    if (dist < minDistance)
    {
        // Don’t ever get too close
        desiredPos = focusPoint + dir * minDistance;
    }

    if (Physics.SphereCast(focusPoint, collisionRadius, dir, out RaycastHit hit, dist, collisionMask, QueryTriggerInteraction.Ignore))
    {
        // Move camera to the hit point, slightly out from the surface
        desiredPos = hit.point + hit.normal * 0.1f;
    }

    // --- Smooth position ---
    transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, positionSmoothTime);

    // --- Smooth rotation (forward-biased when reversing) ---
    Vector3 lookTarget;

    if (targetRigidbody != null && targetRigidbody.linearVelocity.sqrMagnitude > 0.25f)
    {
        Vector3 vel = targetRigidbody.linearVelocity;
        float forwardDot = Vector3.Dot(vel.normalized, target.forward);

        if (forwardDot > 0.2f)
        {
            // Moving mostly forward → look along velocity
            lookTarget = target.position + vel.normalized * 4f;
        }
        else
        {
            // Moving mostly sideways or backwards → keep looking forward
            lookTarget = target.position + target.forward * 4f;
        }
    }
    else
    {
        // Almost stopped → look forward
        lookTarget = target.position + target.forward * 4f;
    }

    Vector3 lookDir = (lookTarget - transform.position).normalized;

    // Flatten the vertical tilt a bit so it doesn’t look down too hard
    lookDir.y *= 0.6f;
    lookDir = lookDir.normalized;

    if (lookDir.sqrMagnitude < 0.0001f)
        lookDir = (target.position - transform.position).normalized;

    Quaternion desiredRot = Quaternion.LookRotation(lookDir, Vector3.up);
    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationSmoothSpeed * Time.deltaTime);
}

}

