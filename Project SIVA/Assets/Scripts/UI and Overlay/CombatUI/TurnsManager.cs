using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsManager : MonoBehaviour, IObserver
{
    [SerializeField] Button skipCutsceneButton;
    [SerializeField] GameObject CutsceneUI;
    [SerializeField] GameObject BattleUI;
    [SerializeField] GameObject ActionsUI;
    [SerializeField] GameObject BattleTurnsUI;
    //TOBECHANGED
    [SerializeField] LevelAudioManager AudioManager;
    public float bigBannerAnimationTime = 1f;
    public float turnAnimationTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Make this and observer of the game events
        BattleSimulator.Instance.AddObserver(this);

        // Add listener to skip cutscene button
        skipCutsceneButton.onClick.AddListener(EndCutscene);

        // Make the correct UI is active
        BattleUI.SetActive(false);
        ActionsUI.SetActive(false);
        BattleTurnsUI.SetActive(false);
        CutsceneUI.SetActive(true);

        BattleTurnsUI.transform.Find("BattleStart").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("PlayerTurn").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("EnemyTurn").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("PlayerWin").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("PlayerLose").gameObject.SetActive(false);

    }

    public void OnNotify(GameEvents gameEvent)
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
        if (gameEvent == GameEvents.PlayerWin)
        {
            PlayerWin();
        }
        if (gameEvent == GameEvents.PlayerLose)
        {
            PlayerLose();
        }
    }

    private void EndCutscene()
    {
        //Finish the cutscene

        //Game BGM on
        AudioManager.StartGame();

        //Disable CutsceneUI and enable BattleUI
        CutsceneUI.SetActive(false);

        //Start animation for BattleStart
        StartCoroutine(BattleStartAnimation());

        //Change BattleSimulator to PlayerTurn
        BattleSimulator.Instance.StartGame();
    }

    // Banner Switching Animation Methods
    public void PlayerWin()
    {
        StartCoroutine(PlayerWinAnimation());
        AudioManager.PlayerWin();
    }
    public void PlayerLose()
    {
        StartCoroutine(PlayerLoseAnimation());
        AudioManager.PlayerLose();
    }
    public void PlayerTurn()
    {
        StartCoroutine(PlayerTurnAnimation());
        AudioManager.ChangeTurn();
    }
    public void EnemyTurn()
    {
        StartCoroutine(EnemyTurnAnimation());
        AudioManager.ChangeTurn();
    }

    IEnumerator BattleStartAnimation()
    {
        // Play animation
        BattleTurnsUI.SetActive(true);
        BattleTurnsUI.transform.Find("BattleStart").gameObject.SetActive(true);
        yield return new WaitForSeconds(bigBannerAnimationTime);
        BattleUI.SetActive(true);
        ActionsUI.SetActive(true);
    }

    IEnumerator PlayerTurnAnimation()
    {
        BattleTurnsUI.transform.Find("PlayerTurn").gameObject.SetActive(true);
        yield return new WaitForSeconds(turnAnimationTime);
        BattleTurnsUI.transform.Find("PlayerTurn").gameObject.SetActive(false);
    }

    IEnumerator EnemyTurnAnimation()
    {
        BattleTurnsUI.transform.Find("EnemyTurn").gameObject.SetActive(true);
        yield return new WaitForSeconds(turnAnimationTime);
        BattleTurnsUI.transform.Find("EnemyTurn").gameObject.SetActive(false);
    }
    
    IEnumerator PlayerWinAnimation()
    {
        BattleTurnsUI.transform.Find("PlayerWin").gameObject.SetActive(true);
        yield return new WaitForSeconds(bigBannerAnimationTime);
    }

    IEnumerator PlayerLoseAnimation()
    {
        BattleTurnsUI.transform.Find("PlayerLose").gameObject.SetActive(true);
        yield return new WaitForSeconds(bigBannerAnimationTime);
    }
}
