using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartVisualTilt : MonoBehaviour
{
    [Header("Ground Detection")]
    public float raycastLength = 1.5f; // How far down to check for ground
    public LayerMask groundLayer;      // Which layers count as ground

    [Header("Tilt Settings")]
    public float tiltSpeed = 5f;       // How quickly the kart tilts to match slope
    public float uprightStrength = 2f; // How strongly it returns upright when airborne

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Cast a ray downward from the kart's position
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, raycastLength, groundLayer))
        {
            // Get the ground normal
            Vector3 groundNormal = hit.normal;

            // Calculate the target rotation based on ground normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // If no ground detected, slowly return upright
            Quaternion uprightRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, uprightStrength * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the raycast in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }
}