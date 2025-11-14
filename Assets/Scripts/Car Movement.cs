using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Acceleration = 0f;
    public float MaxSpeed = 15f;
    public float MaxReverseSpeed = -5f;
    public float Drag = 0.98f;
    public float SteerAngle = 20f;
    public float Traction = 10f;

    [Header("State")]
    public Vector3 Speed;
    public bool isBoosted = false;

    private InputAction driveAction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.centerOfMass = new Vector3(0f, -0.3f, 0f);
        
        driveAction = InputSystem.actions.FindAction("Drive");
    }

    private void FixedUpdate()
    {
        if (driveAction == null)
            return;

        Vector2 input = driveAction.ReadValue<Vector2>();
        float accelInput = input.y;
        float steerInput = input.x;
        
        Debug.Log(input);

        //check if accelerating
        if (accelInput > 0f && !isBoosted)
        {
            Acceleration = 50f;
        }
        //Brake
        else if (accelInput < 0f)
        {
            Acceleration = -20f;
        }
        else
        {
            Acceleration = 0f;
        }

        // Move forward along car's forward
        Speed += transform.forward * (Acceleration * Time.fixedDeltaTime);

        // Drag
        Speed *= Drag;

        // Speed Limit
        Vector3 localSpeed = transform.InverseTransformDirection(Speed);
        localSpeed.z = Mathf.Clamp(localSpeed.z, MaxReverseSpeed, MaxSpeed);
        Speed = transform.TransformDirection(localSpeed);

        // Traction (pull velocity toward forward direction)
        if (Speed.sqrMagnitude > 0.0001f)
        {
            Vector3 desiredDir = transform.forward;
            Vector3 newDir = Vector3.Lerp(Speed.normalized, desiredDir, Traction * Time.fixedDeltaTime);
            Speed = newDir.normalized * Speed.magnitude;
        }

        // Move Car
        rb.MovePosition(rb.position + Speed * Time.fixedDeltaTime);

        // Steering based on current speed
        float steerAmount = steerInput * Speed.magnitude * SteerAngle * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, steerAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
