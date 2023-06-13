using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioManager : MonoBehaviour
{
    [SerializeField] GameObject BackgroundAudioSource;
    [SerializeField] GameObject SoundFX;
    [SerializeField] Animator lobbyBGMAnimation;
    public float battleBGMTransitionTime = 1f;
    public float changeTurnTransitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the correct music is playing
        BackgroundAudioSource.transform.Find("LobbyBGM").gameObject.SetActive(true);
        BackgroundAudioSource.transform.Find("BattleBGM").gameObject.SetActive(false);
        BackgroundAudioSource.transform.Find("CutsceneBGM").gameObject.SetActive(false);

        // Make sure all soundfx are off
        SoundFX.transform.Find("Writing").gameObject.SetActive(false);
        SoundFX.transform.Find("ChangeTurn").gameObject.SetActive(false);
        SoundFX.transform.Find("PlayerWin").gameObject.SetActive(false);
        SoundFX.transform.Find("PlayerLose").gameObject.SetActive(false);

        // Get animator in child
        //GameObject lobbyBGM = BackgroundAudioSource.transform.Find("LobbyBGM").gameObject;
        //Animator lobbyBGMAnimation = lobbyBGM.GetComponentInChilden<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        // Start transtion from lobby music to battle music
        StartCoroutine(BattleBGMAnimation());

        // Start banner pencil sound effect
        SoundFX.transform.Find("Writing").gameObject.SetActive(true);
    }

    public void PlayerWin()
    {
        // Start player win sound effect
        SoundFX.transform.Find("PlayerWin").gameObject.SetActive(true);
        // Start banner pencil sound effect
        SoundFX.transform.Find("Writing").gameObject.SetActive(true);
    }

    public void PlayerLose()
    {
        // Start player lose sound effect
        SoundFX.transform.Find("PlayerLose").gameObject.SetActive(true);
        // Start banner pencil sound effect
        SoundFX.transform.Find("Writing").gameObject.SetActive(true);
    }

    public void ChangeTurn()
    {
        // Start change turn sound effect
        StartCoroutine(ChangeTurnAnimation()); 
    }
    
    IEnumerator BattleBGMAnimation()
    {
        lobbyBGMAnimation.SetTrigger("End");
        yield return new WaitForSeconds(battleBGMTransitionTime);
        BackgroundAudioSource.transform.Find("BattleBGM").gameObject.SetActive(true);
    }

    IEnumerator ChangeTurnAnimation()
    {
        SoundFX.transform.Find("ChangeTurn").gameObject.SetActive(true);
        yield return new WaitForSeconds(changeTurnTransitionTime);
        SoundFX.transform.Find("ChangeTurn").gameObject.SetActive(false);
    }
}
