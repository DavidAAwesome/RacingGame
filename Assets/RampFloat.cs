using UnityEngine;

public class RampFloat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider col){
        
        if(col.gameObject.tag=="Player"||col.gameObject.tag=="AI"){
            Debug.Log("test2");
            col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,100000*Time.deltaTime,0));
        }
    }
}
