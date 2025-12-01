using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int checkPointID;
    private RaceManager raceManager;
    [SerializeField] GameObject player;

    private void Awake()
    {
        raceManager = GetComponent<RaceManager>();
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update(){
        RaceManager.Instance.RespawnCar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RaceManager.Instance.CheckpointReach(checkPointID);
        }

        if(other.CompareTag("AI"))
        {
            RaceManager.Instance.AICheckpointReach(checkPointID);
        }
    }
}
