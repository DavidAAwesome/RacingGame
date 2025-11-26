using UnityEngine;
using UnityEngine.UI;
public class RaceManager : MonoBehaviour
{
    int respawnCoolDown = 0;
    public static RaceManager Instance;
    public CheckPoint[] checkpoints;
    public int totalLaps = 3; // Total number of
    public int currentLap = 1; // Current lap number
    public int lastCheckpointIndex = -1; // Index of the last checkpoint reached

    public bool raceFinished = false;
    public bool raceStarted = false;

void Update(){
   
}
public void RespawnCar(){
     if(deadzone.respawn ==true){
       GameObject.FindWithTag("Player").transform.position = GameObject.Find(lastCheckpointIndex+"").transform.position;
respawnCoolDown=1;



        deadzone.respawn=false;
    }
}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckpointReach(int checkPointID)
    {
        if (!raceStarted && checkPointID != 0 || raceFinished)
        {
            return;
        }
        int expectedNext = (lastCheckpointIndex + 1) % checkpoints.Length;

        if (checkPointID == expectedNext)
        {
            UpdateCheckpoint(checkPointID);
        }
        else if(respawnCoolDown==1){
            respawnCoolDown=0;
        }
        else
        {
            Debug.Log("Wrong Checkpoint");
        }
    }

    public void UpdateCheckpoint(int checkPointID)
    {
       
        if (checkPointID == 0)
        {
            if (raceStarted == false)
            {
                OnStartRace();

            }
            else if (raceFinished == false && lastCheckpointIndex == checkpoints.Length - 1)
            {
                OnLapFinish();
                Debug.Log("Lap Finished!");
            }
        }

        lastCheckpointIndex = checkPointID;
    }

    private void OnStartRace()
    {
        raceStarted = true;
        raceFinished = false;
        Debug.Log("Race Started!");
    }

    private void OnFinishRace()
    {
        raceFinished = true;
        raceStarted = false;
        Debug.Log("Race Finished!");

    }


    private void OnLapFinish()
    {
        currentLap++;
        if(currentLap==1){
            GameObject.Find("L1").GetComponent<Image>().enabled=true;
             GameObject.Find("L2").GetComponent<Image>().enabled=false;
              GameObject.Find("L3").GetComponent<Image>().enabled=false;
        }
        if(currentLap==2){
            GameObject.Find("L1").GetComponent<Image>().enabled=false;
             GameObject.Find("L2").GetComponent<Image>().enabled=true;
              GameObject.Find("L3").GetComponent<Image>().enabled=false;
        }
         if(currentLap==3){
            GameObject.Find("L1").GetComponent<Image>().enabled=false;
             GameObject.Find("L2").GetComponent<Image>().enabled=false;
              GameObject.Find("L3").GetComponent<Image>().enabled=true;
        }
        if (currentLap > totalLaps)
        {
            OnFinishRace();
        }
        else
        {
            lastCheckpointIndex = -1; // Reset checkpoint index for new lap
            Debug.Log("Starting Lap " + currentLap);
        }

    }

}

