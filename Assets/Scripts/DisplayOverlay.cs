using UnityEngine;

public class DisplayOverlay : MonoBehaviour
{
    public bool gameWon;
     public bool gameLost;
    public GameObject Overlay;
    public static GameObject Overlay1_Global;
    public GameObject win;
    public GameObject lose;
    public GameObject Ready;
     public GameObject Set;
      public GameObject Go;
      float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Overlay1_Global=Overlay;
    }

    // Update is called once per frame
    void Update()
    {
        ReadySetGo_Start();
time+=Time.deltaTime;

        if ((!gameObject.name.Contains("Win") && (!gameObject.name.Contains("Lose")) && Input.GetKey(KeyCode.Escape)))
        {
            Overlay.SetActive(true);
        }
        else if (gameWon)
        {
             win.SetActive(true);
        }
         else if (gameLost)
        {
            lose.SetActive(true);
        }
    }
    void ReadySetGo_Start(){
        Ready_Func();
        if(time>2){
            Set_Func();
        }
         if(time>4){
            Go_Func();
        }
         if(time>6){
            Go.SetActive(false);
        }
    }
    void Ready_Func(){
        Ready.SetActive(true);
        Set.SetActive(false);
       Go.SetActive(false);
    }
     void Set_Func(){
         Set.SetActive(true);
Ready.SetActive(false);
Go.SetActive(false);
    }
     void Go_Func(){

Go.SetActive(true);
Set.SetActive(false);
Ready.SetActive(false);
    }
}
