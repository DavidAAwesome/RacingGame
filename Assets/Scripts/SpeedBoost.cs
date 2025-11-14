using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float BoostAmount = 100f;
    public float BoostDuration = 5f;
    [SerializeField] CarMovement carMovement;
    [SerializeField]GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carMovement = GameObject.FindWithTag("Player").GetComponent<CarMovement>();
        player = GameObject.FindWithTag("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Debug.Log("Speed Boost Activated!");
            carMovement.isBoosted = true;
            carMovement.Acceleration += BoostAmount * 2; //Increase acceleration significantly during boost
            carMovement.MaxSpeed += BoostAmount; //Increase max speed during boost
            carMovement.Speed += player.transform.forward * BoostAmount; 
            Invoke("ResetSpeed", BoostDuration);
            gameObject.SetActive(false);
            Invoke("ReactivateBoost", BoostDuration + 3f); // Reactivate after boost duration + cooldown time
        }
    }

    private void ResetSpeed() //Reset the Speed after boost duration
    {
        carMovement.Acceleration -= BoostAmount;
        carMovement.MaxSpeed -= BoostAmount;
        carMovement.isBoosted = false;
        // Debug.Log("Speed Boost Ended!");
    }

    private void ReactivateBoost() //Reactivate the boost pickup after cooldown
    {
        gameObject.SetActive(true);
        // Debug.Log("Speed Boost Ready Again!");
    }
}

    




