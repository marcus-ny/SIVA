using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    public Animator transition;
    public float transitionTime = 1f;
    
    private void Awake(){
        Instance = this;
    }

    // All new scenes must be added into this enum
    public enum Scene{
        MainMenu,
        Level01,
        MS2PlayerDemo
    }

    // This method loads a scene specified in the parameter by name
    public void LoadScene(Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }

    // This method loads a new game meaning the level01 scene
    public void LoadNewGame(){
        StartCoroutine(LoadLevel(Scene.MS2PlayerDemo.ToString()));
    }

    /*
    // This method loads the next scene based on the build index
    public void LoadNextScene(){
        //!!LoadLevel method does not take in int
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    */

    // This method loads the main menu scene 
    public void LoadMainMenu(){
        //SceneManager.LoadScene(Scene.MainMenu.ToString());
        StartCoroutine(LoadLevel(Scene.MainMenu.ToString()));
    }

    IEnumerator LoadLevel(string levelName){
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load Scene
        SceneManager.LoadScene(levelName);
    }
}
