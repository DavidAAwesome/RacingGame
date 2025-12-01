using UnityEngine;
using UnityEngine.UI;
public class RaceManager : MonoBehaviour
{
    int respawnCoolDown = 0;
    public static RaceManager Instance;
    public DisplayOverlay displayOverlay;
    public CheckPoint[] checkpoints;
    public int totalLaps = 3; // Total number of
    public int currentLap = 1; // Current lap number
    public int lastCheckpointIndex = -1; // Index of the last checkpoint reached
    public int AiCurrentLap = 1;// AI Current lap number
    public int AiLastCheckpointIndex = -1;//  Player Index of the last checkpoint reached

    public bool raceFinished = false;
    public bool raceStarted = false;
    public bool playerWin = false;
    public bool aiWin = false;



    void Update(){
   
}
public void RespawnCar(){
     if(deadzone.respawn ==true){
        GameObject.FindWithTag("Player").transform.position = GameObject.Find(lastCheckpointIndex+"").transform.position;
        GameObject.FindWithTag("Player").GetComponent<MeshRenderer>().enabled=false;
   

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

        displayOverlay = GameObject.Find("DisplayCanvas").GetComponent<DisplayOverlay>();
    }

    public void CheckpointReach(int checkPointID)
    {
        if (!raceStarted && checkPointID != 0 || raceFinished)
        {
            return;
        }
        int expectedNext = lastCheckpointIndex + 1 ;
        Debug.Log("Expected Next Checkpoint: " + expectedNext);

        if (checkPointID == expectedNext)
        {
            UpdateCheckpoint(checkPointID);
            Debug.Log("Checkpoint Reached: " + checkPointID);
        }
        else if(respawnCoolDown==1){
            GameObject.Find("Main Camera").transform.position=new Vector3(GameObject.Find("Main Camera").transform.position.x,3.587955f,GameObject.Find("Main Camera").transform.position.z);
            GameObject.Find("Main Camera").GetComponent<CameraController>().enabled=true;
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

        if (currentLap == 1)
        {
            GameObject.Find("L1").GetComponent<Image>().enabled = true;
            GameObject.Find("L2").GetComponent<Image>().enabled = false;
            GameObject.Find("L3").GetComponent<Image>().enabled = false;
            Debug.Log("Run 1");
        }
        if (currentLap == 2)
        {
            GameObject.Find("L1").GetComponent<Image>().enabled = false;
            GameObject.Find("L2").GetComponent<Image>().enabled = true;
            GameObject.Find("L3").GetComponent<Image>().enabled = false;
        }
        if (currentLap == 3)
        {
            GameObject.Find("L1").GetComponent<Image>().enabled = false;
            GameObject.Find("L2").GetComponent<Image>().enabled = false;
            GameObject.Find("L3").GetComponent<Image>().enabled = true;
        }

        lastCheckpointIndex = checkPointID;
    }


    public void AICheckpointReach(int checkPointID)
    {
        if (!raceStarted && checkPointID != 0 || raceFinished)
        {
            return;
        }
        int expectedNext = (AiLastCheckpointIndex + 1) % checkpoints.Length;

        if (checkPointID == expectedNext)
        {
            AiUpdateCheckpoint(checkPointID);
        }
        else if (respawnCoolDown == 1)
        {
               GameObject.FindWithTag("Player").GetComponent<MeshRenderer>().enabled=true;

            respawnCoolDown = 0;
        }
        else
        {
            Debug.Log("Wrong Checkpoint");
        }
    }

    public void AiUpdateCheckpoint(int checkPointID)
    {

        if (checkPointID == 0)
        {
            if (raceStarted == false)
            {
                OnStartRace();

            }
            else if (raceFinished == false && AiLastCheckpointIndex == checkpoints.Length - 1)
            {
                AILapFinish();
                Debug.Log("Lap Finished!");
            }
        }

        AiLastCheckpointIndex = checkPointID;
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
        private void AILapFinish()
        {
            AiCurrentLap++;
            if (AiCurrentLap > totalLaps)
        {
            OnFinishRace();
        }
            else
             {
                AiLastCheckpointIndex = -1; // Reset checkpoint index for new lap
                Debug.Log("Starting Lap " + currentLap);
             }
   
        }


    public void Postioning()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        Transform ai = GameObject.FindWithTag("AI").transform;
        Vector3 distance = player.position - ai.position;
        float distanceDot = Vector3.Dot(distance, player.forward);

        if (lastCheckpointIndex > AiLastCheckpointIndex)
        {
           // Debug.Log("Position 1");
        }
        else if (lastCheckpointIndex == AiLastCheckpointIndex && distanceDot > 0)
        {
           // Debug.Log("Position 1");
        }
        else if (lastCheckpointIndex == AiLastCheckpointIndex && distanceDot < 0)
        {
           // Debug.Log("Position 2");
        }
        else if (currentLap > AiCurrentLap)
        {
           // Debug.Log("Position 1");
        }
        else if (AiCurrentLap > currentLap)
        {
            //Debug.Log("Position 2");
        }
        else
        {
           // Debug.Log("Position 2");
        }

    }

    private void WinLose()
    {
        if (raceFinished && AiCurrentLap > currentLap)
        {
            aiWin = true;
            playerWin = false;
        }
        else if (raceFinished && currentLap > AiCurrentLap)
        {
            playerWin = true;
            aiWin = false;
        }

        if (playerWin)
        {
            Debug.Log("Player Wins!");
            displayOverlay.gameWon = true;

        }
        else if (aiWin)
        {
            Debug.Log("AI Wins!");
            displayOverlay.gameLost = true;
        }
    }

    private void FixedUpdate()
    {
        Postioning();
        WinLose();
        
    }


}



