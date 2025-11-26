using UnityEngine;

public class deadzone : MonoBehaviour
{
    public static bool respawn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(GameObject.FindWithTag("Player").transform.position.y<-7)
        respawn = true;
    }
   
}
