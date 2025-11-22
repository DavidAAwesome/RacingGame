using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    public float motorForce = 30f;
    public float maxSpeed = 20f;          // forward speed
    public float maxReverseSpeed = 8f;    // backwards speed
    public float turnSpeed = 80f;
    public float drag = 0.98f;

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

        // This keeps the kart from tipping / flipping physically
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Lower center of mass helps stability even more
        rb.centerOfMass = new Vector3(0f, -0.3f, 0f);
    }

    private void FixedUpdate()
    {
        // Speed along the kart's forward
        forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        HandleAcceleration();
        HandleSteering();

        // Simple drag
        rb.linearVelocity *= drag;
    }

    private void HandleAcceleration()
    {
        if (Mathf.Abs(accelInput) <= 0.01f)
            return;

        bool acceleratingForward = accelInput > 0f;

        if (acceleratingForward)
        {
            // Limit forward speed
            if (forwardSpeed < maxSpeed)
            {
                rb.AddForce(transform.forward * (accelInput * motorForce), ForceMode.Acceleration);
            }
        }
        else
        {
            // Limit reverse speed
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

        // Scale steering by speed so you don’t spin on the spot
        float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / maxSpeed);

        float turnAmount = steer * turnSpeed * speedFactor * Time.fixedDeltaTime;
        Quaternion turnRot = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRot);
    }
}
