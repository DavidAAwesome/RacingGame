using UnityEngine;

public class DisplayOverlay : MonoBehaviour
{
    public bool gameWon;
     public bool gameLost;
    public GameObject Overlay;
    public static GameObject Overlay1_Global;
    public GameObject Overlay2;
    public GameObject Overlay3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Overlay1_Global=Overlay;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!gameObject.name.Contains("Win") && (!gameObject.name.Contains("Lose")) && Input.GetKey(KeyCode.Escape)))
        {
            Overlay.SetActive(true);
        }
        else if (gameWon)
        {
             Overlay2.SetActive(true);
        }
         else if (gameLost)
        {
             Overlay3.SetActive(true);
        }
    }
}
