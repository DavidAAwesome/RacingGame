using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(KartPhysics))]
public class PlayerKartController : MonoBehaviour
{
    private KartPhysics kart;
    private InputAction driveAction;

    private void Awake()
    {
        kart = GetComponent<KartPhysics>();

        // Uses the same global Input System setup as your CarMovement
        driveAction = InputSystem.actions.FindAction("Drive");

        if (driveAction == null)
        {
            Debug.LogError("Drive action not found. Make sure you have an action named 'Drive' in your Input Actions asset.");
        }
    }

    private void OnEnable()
    {
        driveAction?.Enable();
    }

    private void OnDisable()
    {
        driveAction?.Disable();
    }

    private void Update()
    {
        if (driveAction == null)
            return;

        Vector2 input = driveAction.ReadValue<Vector2>();

        float accelInput = input.y; // up/down on stick or W/S
        float steerInput = input.x; // left/right

        // Optionally clamp so weird devices don't give >1
        accelInput = Mathf.Clamp(accelInput, -1f, 1f);
        steerInput = Mathf.Clamp(steerInput, -1f, 1f);

        kart.accelInput = accelInput;
        kart.steerInput = steerInput;

        if (transform.position.y <= -4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

