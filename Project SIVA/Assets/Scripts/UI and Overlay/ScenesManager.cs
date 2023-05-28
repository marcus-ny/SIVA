using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    
    private void Awake(){
        Instance = this;
    }

    public enum Scene{
        MainMenu,
        Level01
    }

    public void LoadScene(Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadNewGame(){
        SceneManager.LoadScene(Scene.Level01.ToString());
    }

    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }
}
