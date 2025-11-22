using UnityEngine;

[RequireComponent(typeof(KartPhysics))]
public class AIController : MonoBehaviour
{
    public WaypointPath path;
    public int currentWaypointIndex = 0;

    [Header("Steering")]
    public float waypointReachDistance = 5f;
    public float lookAheadDistance = 10f;

    [Header("Speed Control")]
    public float maxSpeed = 18f;
    public float cornerSlowdownFactor = 0.5f;  // how much to slow in sharp turns

    [Header("Obstacle Avoidance")]
    public float avoidRayDistance = 5f;
    public float avoidSteerStrength = 0.8f;

    private KartPhysics kart;
    private Rigidbody rb;

    private void Awake()
    {
        kart = GetComponent<KartPhysics>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (path == null || path.Count == 0) return;

        Transform currentWp = path.GetWaypoint(currentWaypointIndex);
        Vector3 toWp = currentWp.position - transform.position;

        // Advance to next waypoint when close enough
        if (toWp.magnitude < waypointReachDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % path.Count;
            currentWp = path.GetWaypoint(currentWaypointIndex);
            toWp = currentWp.position - transform.position;
        }

        // Look-ahead point for smoother steering
        Vector3 targetPos = currentWp.position;
        Vector3 dirToWp = toWp.normalized;
        targetPos = transform.position + dirToWp * lookAheadDistance;

        // Convert target to local space to get steering direction
        Vector3 localTarget = transform.InverseTransformPoint(targetPos);
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);

        // Speed control: slow down if the turn is sharp
        float angleToTarget = Vector3.Angle(transform.forward, toWp);
        float desiredSpeed = Mathf.Lerp(maxSpeed * cornerSlowdownFactor, maxSpeed, Mathf.InverseLerp(180f, 0f, angleToTarget));

        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float accel = Mathf.Clamp((desiredSpeed - currentSpeed) / maxSpeed, -1f, 1f);

        // Obstacle avoidance with a simple raycast
        steer = ApplyAvoidance(steer);

        // Write inputs to kart
        kart.steerInput = steer;
        kart.accelInput = accel;
    }

    private float ApplyAvoidance(float currentSteer)
    {
        RaycastHit hit;

        // Straight ahead
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, avoidRayDistance))
        {
            // Steer away from the obstacle
            Vector3 localHit = transform.InverseTransformPoint(hit.point);
            float avoidDir = localHit.x > 0 ? -1f : 1f;
            currentSteer += avoidDir * avoidSteerStrength;
        }

        return Mathf.Clamp(currentSteer, -1f, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize avoidance ray
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f,
                        transform.position + Vector3.up * 0.5f + transform.forward * avoidRayDistance);
    }
}
