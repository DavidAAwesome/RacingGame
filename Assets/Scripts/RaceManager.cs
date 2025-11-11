using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;
    public CheckPoint[] checkpoints;
    public int totalLaps = 3; // Total number of
    public int currentLap = 1; // Current lap number
    public int lastCheckpointIndex = -1; // Index of the last checkpoint reached

    public bool raceFinished = false;
    public bool raceStarted = false;

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

