using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    public float motorForce = 30f;
    public float maxSpeed = 20f;          // forward speed
    public float maxReverseSpeed = 8f;    // backwards speed
    public float turnSpeed = 10f;
    public float drag = 0.98f;

    [Header("Friction & Stability")]
    public float sideFriction = 8f;       // how hard we kill sideways sliding
    public float angularFriction = 6f;    // how hard we kill spinning (yaw)

    [Header("Behavior")]
    public bool flipSteeringWhenReversing = true;

    [Header("Debug")]
    [Range(-1f, 1f)] public float accelInput;   // set by PlayerKartController
    [Range(-1f, 1f)] public float steerInput;   // set by PlayerKartController
    public float forwardSpeed;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // 🔒 Prevent tipping/rolling
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Heavier + lower COM = more stable
        rb.mass = 150f;                  // tweak if needed
        rb.angularDamping = 5f;             // damping for any leftover spin
        rb.centerOfMass = new Vector3(0f, -0.3f, 0f);
    }

    private void FixedUpdate()
    {
         if(!Input.GetKey("a")&&!Input.GetKey("d")){
    rb.constraints = RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY;
    }
        // Signed speed in forward direction
        forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        HandleAcceleration();
        HandleSteering();

        ApplySideFriction();
        ApplyAngularFriction();

        // Simple drag for overall slowing
        rb.linearVelocity *= drag;
    }

    private void HandleAcceleration()
    {
        if (Mathf.Abs(accelInput) <= 0.01f)
            return;

        bool acceleratingForward = accelInput > 0f;

        if (acceleratingForward)
        {
            if (forwardSpeed < maxSpeed)
            {
                rb.AddForce(transform.forward * (accelInput * motorForce), ForceMode.Acceleration);
            }
        }
        else
        {
            if (forwardSpeed > -maxReverseSpeed)
            {
                rb.AddForce(transform.forward * (accelInput * motorForce), ForceMode.Acceleration);
            }
        }
    }

    private void HandleSteering()
    {
        float steer = steerInput;

        // Flip steering when reversing so controls feel natural
        if (flipSteeringWhenReversing && forwardSpeed < -0.1f)
        {
            steer = -steer;
        }

        // Scale steering by speed so you don’t spin at low speeds
        float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / maxSpeed);

        float turnAmount = steer * turnSpeed * speedFactor * Time.fixedDeltaTime;
        Quaternion turnRot = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRot);
    }

    private void ApplySideFriction()
    {
        // Kill sideways sliding in local space
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);

        // Lerp X (sideways) toward 0
        localVel.x = Mathf.Lerp(localVel.x, 0f, sideFriction * Time.fixedDeltaTime);

        rb.linearVelocity = transform.TransformDirection(localVel);
    }

    private void ApplyAngularFriction()
    {
        // Kill unwanted spinning around Y
        Vector3 angVel = rb.angularVelocity;
        angVel.y = Mathf.Lerp(angVel.y, 0f, angularFriction * Time.fixedDeltaTime);
        rb.angularVelocity = angVel;
    }
}
