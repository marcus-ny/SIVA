using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    /*
    public Animator pauseTransition;
    public float pauseTransitionTime = 1f;
    */

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        //StartCoroutine(PauseAnimation());
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /*
    IEnumerator PauseAnimation() {
        Debug.Log("Entered");
        pauseTransition.SetTrigger("Close");
        yield return new WaitForSeconds(pauseTransitionTime);
    }
    */
}
