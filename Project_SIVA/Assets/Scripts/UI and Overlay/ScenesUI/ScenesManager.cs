using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    public Animator transition;
    public float transitionTime = 1f;
    private GameScene cur;
    
    private void Awake(){
        Instance = this;
    }

    // All new scenes must be added into this enum
    public enum GameScene{
        MainMenu,
        Tutorial_P0,
        Tutorial_P1, 
        Tutorial_P2,
        Tutorial_P3,
        Tutorial_P4
    }

    // This method loads a scene specified in the parameter by name
    public void LoadScene(GameScene scene){
        SceneManager.LoadScene(scene.ToString());
        cur = scene;
    }

    public int GetActiveScene()
    {
        Scene curScene = SceneManager.GetActiveScene();
        int buildIndex = curScene.buildIndex;
        return buildIndex;
    }

    public void LoadNextScene()
    {
        int curSceneIndex = GetActiveScene();
        Debug.Log(curSceneIndex);
        var nextScene = (GameScene) (curSceneIndex + 1);
        Debug.Log(nextScene);
        LoadScene(nextScene);

    }
    // This method loads a new game meaning the level01 scene
    public void LoadNewGame(){
        StartCoroutine(LoadLevel(GameScene.Tutorial_P1.ToString()));
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
        StartCoroutine(LoadLevel(GameScene.MainMenu.ToString()));
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
