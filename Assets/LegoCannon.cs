using UnityEngine;

public class LegoCannon : MonoBehaviour
{
    [Header("Cannon Settings")]
    [Tooltip("Point from which the LEGO pieces will be launched.")]
    public Transform launchPoint;

    [Tooltip("Time between shots (seconds).")]
    public float fireInterval = 1f;

    [Tooltip("Lifetime of each LEGO piece before destruction (seconds).")]
    public float legoLifetime = 5f;

    [Header("LEGO Prefabs & Speeds")]
    [Tooltip("First LEGO prefab to launch.")]
    public GameObject legoPrefab1;
    [Tooltip("Launch force for first LEGO piece.")]
    public float launchForce1 = 1500f;

    [Tooltip("Second LEGO prefab to launch.")]
    public GameObject legoPrefab2;
    [Tooltip("Launch force for second LEGO piece.")]
    public float launchForce2 = 2000f;

    [Tooltip("Third LEGO prefab to launch.")]
    public GameObject legoPrefab3;
    [Tooltip("Launch force for third LEGO piece.")]
    public float launchForce3 = 2500f;

    private float timer = 0f;
    private int currentIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireInterval)
        {
            FireNext();
            timer = 0f;
        }
    }

    void FireNext()
    {
        GameObject prefabToLaunch = null;
        float forceToApply = 0f;

        // Cycle through 3 prefabs
        switch (currentIndex)
        {
            case 0:
                prefabToLaunch = legoPrefab1;
                forceToApply = launchForce1;
                break;
            case 1:
                prefabToLaunch = legoPrefab2;
                forceToApply = launchForce2;
                break;
            case 2:
                prefabToLaunch = legoPrefab3;
                forceToApply = launchForce3;
                break;
        }

        if (prefabToLaunch != null && launchPoint != null)
        {
            GameObject legoInstance = Instantiate(prefabToLaunch, launchPoint.position, launchPoint.rotation);

            Rigidbody rb = legoInstance.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = legoInstance.AddComponent<Rigidbody>();
            }

            rb.AddForce(launchPoint.forward * forceToApply);

            Destroy(legoInstance, legoLifetime);
        }

        // Move to next prefab (loop back after 3)
        currentIndex = (currentIndex + 1) % 3;
    }
}