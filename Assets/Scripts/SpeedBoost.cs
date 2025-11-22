using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float BoostMotorForce = 50f;       // how much extra motor force during boost
    public float BoostMaxSpeedIncrease = 20f; // how much faster the kart can go
    public float BoostDuration = 5f;         // how long the boost lasts
    public float ReactivateDelay = 3f;       // cooldown after boost ends

    [Header("Runtime")]
    [SerializeField] private KartPhysics kartPhysics;
    [SerializeField] private GameObject player;

    private float originalMotorForce;
    private float originalMaxSpeed;
    private bool isBoostActive = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            kartPhysics = player.GetComponent<KartPhysics>();
        }

        if (kartPhysics == null)
        {
            Debug.LogError("SpeedBoost: Could not find KartPhysics on Player. Make sure your player has a KartPhysics component.");
            enabled = false;
            return;
        }

        // Cache original values
        originalMotorForce = kartPhysics.motorForce;
        originalMaxSpeed = kartPhysics.maxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBoostActive) return; // prevent stacking
        if (other.gameObject != player) return;

        ActivateBoost();
    }

    private void ActivateBoost()
    {
        isBoostActive = true;

        // Increase motor force and max speed
        kartPhysics.motorForce = originalMotorForce + BoostMotorForce;
        kartPhysics.maxSpeed = originalMaxSpeed + BoostMaxSpeedIncrease;

        // Give an instant forward push
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity += player.transform.forward * BoostMaxSpeedIncrease;
        }

        // Disable pickup and schedule reset/reactivation
        gameObject.SetActive(false);
        Invoke(nameof(ResetSpeed), BoostDuration);
        Invoke(nameof(ReactivateBoost), BoostDuration + ReactivateDelay);
    }

    private void ResetSpeed()
    {
        // Restore original values
        kartPhysics.motorForce = originalMotorForce;
        kartPhysics.maxSpeed = originalMaxSpeed;
        isBoostActive = false;
    }

    private void ReactivateBoost()
    {
        gameObject.SetActive(true);
    }
}

