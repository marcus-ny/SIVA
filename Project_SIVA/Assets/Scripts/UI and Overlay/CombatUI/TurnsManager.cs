using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsManager : Publisher, IObserver
{
    [SerializeField] Button skipCutsceneButton;
    [SerializeField] GameObject UIFolder;
    
    //TOBECHANGED
    //[SerializeField] LevelAudioManager AudioManager;
    public float bigBannerAnimationTime = 1f;
    public float turnAnimationTime = 1f;
    public float endCutsceneTransitionTime = 0.5f;

    public Animator EndCutsceneTransition;

    GameObject CutsceneUI;
    GameObject BannerUI;
    GameObject ActionsUI;
    GameObject StatsUI;
    GameObject LogBox;

    // Start is called before the first frame update
    void Start()
    {
        // Get necessary objects
        CutsceneUI = UIFolder.transform.Find("CutsceneUI").gameObject;
        BannerUI= UIFolder.transform.Find("BannerUI").gameObject;
        ActionsUI = UIFolder.transform.Find("ActionsUI").gameObject;
        StatsUI = UIFolder.transform.Find("StatsUI").gameObject;
        LogBox = UIFolder.transform.Find("LogBox").gameObject;

        // Configure UI
        CutsceneUI.SetActive(true);
        BannerUI.SetActive(false);
        ActionsUI.SetActive(false);
        StatsUI.SetActive(false);
        LogBox.SetActive(false);

        // Makesure banner is all off
        BannerUI.transform.Find("BattleStart").gameObject.SetActive(false);
        BannerUI.transform.Find("PlayerTurn").gameObject.SetActive(false);
        BannerUI.transform.Find("EnemyTurn").gameObject.SetActive(false);
        BannerUI.transform.Find("PlayerWin").gameObject.SetActive(false);
        BannerUI.transform.Find("PlayerLose").gameObject.SetActive(false);

        // Make this and observer of the game events
        BattleSimulator.Instance.AddObserver(this);

        // Add listener to skip cutscene button
        //skipCutsceneButton.onClick.AddListener(EndCutscene);
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (!BattleSimulator.Instance.levelComplete)
        {
            if (gameEvent == GameEvents.PlayerTurn)
            {
                Debug.Log("OnNotifiedPlayerTurnReceived");
                PlayerTurn();
            }
            if (gameEvent == GameEvents.EnemyTurn)
            {
                Debug.Log("OnNotifiedEnemyTurnReceived");
                EnemyTurn();
            }
            
        }
        else
        {
            if (gameEvent == GameEvents.PlayerWin)
            {
                PlayerWin();
            }
            if (gameEvent == GameEvents.PlayerLose)
            {
                PlayerLose();
            }
        }
    }

    public void StartCutscene()
    {
        Debug.Log("Cutscene begins, all other UI disabled");
        CutsceneUI.SetActive(true);
        BannerUI.SetActive(false);
        ActionsUI.SetActive(false);
        StatsUI.SetActive(false);
        LogBox.SetActive(false);
    }
    public void EndCutscene()
    {
        //Disable CutsceneUI and enable BattleUI
        StartCoroutine(EndCutsceneAnimation());

        // Notify BattleStart
        NotifyObservers(GameEvents.BattleStart);
        
        //Start animation for BattleStart
        StartCoroutine(BattleStartAnimation());
    }

    // Banner Switching Animation Methods
    public void PlayerWin()
    {
        StartCoroutine(PlayerWinAnimation());
    }
    public void PlayerLose()
    {
        StartCoroutine(PlayerLoseAnimation());
    }
    public void PlayerTurn()
    {
        StartCoroutine(PlayerTurnAnimation());
    }
    public void EnemyTurn()
    {
        StartCoroutine(EnemyTurnAnimation());
    }

    IEnumerator BattleStartAnimation()
    {
        if (!BattleSimulator.Instance.levelComplete)
        {
            // Play animation
            BannerUI.SetActive(true);
            BannerUI.transform.Find("BattleStart").gameObject.SetActive(true);
        }
            yield return new WaitForSeconds(bigBannerAnimationTime);
            
            StatsUI.SetActive(true);
            ActionsUI.SetActive(true);
            LogBox.SetActive(true);
            
        // Change BattleSimulator to PlayerTurn
        BattleSimulator.Instance.StartGame();

        // Notify Observers
        NotifyObservers(GameEvents.BattleStart);
    }

    IEnumerator PlayerTurnAnimation()
    {
        BannerUI.transform.Find("PlayerTurn").gameObject.SetActive(true);
        yield return new WaitForSeconds(turnAnimationTime);
        BannerUI.transform.Find("PlayerTurn").gameObject.SetActive(false);
    }

    IEnumerator EnemyTurnAnimation()
    {
        BannerUI.transform.Find("EnemyTurn").gameObject.SetActive(true);
        yield return new WaitForSeconds(turnAnimationTime);
        BannerUI.transform.Find("EnemyTurn").gameObject.SetActive(false);
    }
    
    IEnumerator PlayerWinAnimation()
    {
        BannerUI.transform.Find("PlayerWin").gameObject.SetActive(true);
        yield return new WaitForSeconds(bigBannerAnimationTime);
        BannerUI.transform.Find("PlayerWin").gameObject.SetActive(false);
    }

    IEnumerator PlayerLoseAnimation()
    {
        BannerUI.transform.Find("PlayerLose").gameObject.SetActive(true);
        yield return new WaitForSeconds(bigBannerAnimationTime);
    }

    IEnumerator EndCutsceneAnimation()
    {
        EndCutsceneTransition.SetTrigger("Trigger");

        yield return new WaitForSeconds(endCutsceneTransitionTime);

        //Disable CutsceneUI and enable BattleUI
        CutsceneUI.SetActive(false);
        NotifyObservers(GameEvents.DialogueEnd);
    }
}
