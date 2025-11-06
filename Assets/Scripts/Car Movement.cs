using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class CarMovement : MonoBehaviour
{

    public float Acceleration = 0;
    public float MaxSpeed = 15;
    public float MaxReverseSpeed = -5;
    public float Drag = 0.98f;
    public float SteerAngle = 20;
    public float Traction = 10;
    public Vector3 Speed;

    InputAction driveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        driveAction = InputSystem.actions.FindAction("Drive");
    }

    

    private void FixedUpdate()
    {
        Debug.Log(driveAction.ReadValue<Vector2>());
        //check if accelerating
        if (driveAction.ReadValue<Vector2>().y > 0)
        {
            Acceleration = 50;


        }
        //Brake
        else if (driveAction.ReadValue<Vector2>().y < 0)
        {
            Acceleration = -20;
        }
        else if (driveAction.ReadValue<Vector2>().y == 0)
        {
            Acceleration = 0;
        }

            //turn
            transform.Rotate(Vector3.up * driveAction.ReadValue<Vector2>().x * Speed.magnitude * SteerAngle * Time.deltaTime);
        
        
        //Move forward
        Speed += transform.forward * Acceleration * Time.deltaTime;
        transform.position += Speed * Time.deltaTime;

        // Drag
        Speed *= Drag;

        //Speed Limit (kind of working backwards)
        Speed = ClampMagnitude(Speed, MaxSpeed, MaxReverseSpeed);
        


        //Traction
        Speed = Vector3.Lerp(Speed.normalized, transform.forward, Traction * Time.deltaTime) * Speed.magnitude;
    }

    public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }

}
