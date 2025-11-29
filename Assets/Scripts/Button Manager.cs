
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
   
    public GameObject Overlay;
    enum CurrentSceneName
    {
        Menu,
        MainGame1,
        MainGame2,
        MainGame3,
None,

    }
    CurrentSceneName currentScene = CurrentSceneName.None;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene == CurrentSceneName.Menu)
        {
            SceneManager.LoadScene("Main Menu");
        }
        if (currentScene == CurrentSceneName.MainGame1)
        {
            SceneManager.LoadScene("Level 1");
        }
        if (currentScene == CurrentSceneName.MainGame2)
        {
           SceneManager.LoadScene("Level 2");
        }
          if (currentScene == CurrentSceneName.MainGame3)
        {
             SceneManager.LoadScene("Level 3");
        }

    }
    void OnMouseDown()
    {
        Debug.Log("1");
        if (gameObject.name.Contains("1"))
        {
            currentScene = CurrentSceneName.MainGame1;
        }
         if (gameObject.name.Contains("2"))
        {
            currentScene = CurrentSceneName.MainGame2;
        }
         if (gameObject.name.Contains("3"))
        {
            currentScene = CurrentSceneName.MainGame3;
        }

        if (gameObject.name.Contains("Exit"))
        {
            currentScene = CurrentSceneName.Menu;
        }
        
         if (gameObject.name.Contains("Resume"))
        {
            DisplayOverlay.Overlay1_Global.SetActive(false);
        }
    }
}
