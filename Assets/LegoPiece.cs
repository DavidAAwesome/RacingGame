using UnityEngine;

public class LegoPiece : MonoBehaviour
{
    private ParticleSystem smoke;
    private bool collisionActive = false;
    private float activationDelay = 0f;
    private float timer = 0f;

    [Tooltip("Delay (seconds) before collision disables smoke.")]
    public float collisionDelay = 0.5f; // you can adjust in Inspector

    void Start()
    {
        smoke = GetComponentInChildren<ParticleSystem>();
        activationDelay = collisionDelay;
        timer = 0f;
    }

    void Update()
    {
        // Count up until delay passes
        if (!collisionActive)
        {
            timer += Time.deltaTime;
            if (timer >= activationDelay)
            {
                collisionActive = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collisionActive && smoke != null && smoke.isPlaying)
        {
            smoke.Stop();
        }
    }
}