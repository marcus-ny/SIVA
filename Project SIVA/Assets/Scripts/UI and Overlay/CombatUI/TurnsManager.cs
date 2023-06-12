using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsManager : MonoBehaviour
{
    [SerializeField] Button skipCutsceneButton;
    [SerializeField] GameObject CutsceneUI;
    [SerializeField] GameObject BattleUI;
    [SerializeField] GameObject BattleTurnsUI;
    public float bigBannerAnimationTime = 1f;
    public float turnAnimationTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        skipCutsceneButton.onClick.AddListener(EndCutscene);

        // Make the correct UI is active
        BattleUI.SetActive(false);
        BattleTurnsUI.SetActive(false);
        CutsceneUI.SetActive(true);

        BattleTurnsUI.transform.Find("BattleStart").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("PlayerTurn").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("EnemyTurn").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("PlayerWin").gameObject.SetActive(false);
        BattleTurnsUI.transform.Find("PlayerLose").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (BattleSimulator.Instance.State == BattleState.PlayerTurn)
        {
            StartCoroutine(PlayerTurnAnimation());
        }
        else if (BattleSimulator.Instance.State == BattleState.EnemyTurn)
        {
            StartCoroutine(EnemyTurnAnimation());
        }
        */
    }

    private void EndCutscene()
    {
        //Finish the cutscene

        //Disable CutsceneUI and enable BattleUI
        CutsceneUI.SetActive(false);

        //Start animation for BattleStart
        StartCoroutine(BattleStartAnimation());

        //Change BattleSimulator to PlayerTurn
        BattleSimulator.Instance.StartGame();
    }

    IEnumerator BattleStartAnimation()
    {
        // Play animation
        BattleTurnsUI.SetActive(true);
        BattleTurnsUI.transform.Find("BattleStart").gameObject.SetActive(true);
        yield return new WaitForSeconds(bigBannerAnimationTime);
        BattleUI.SetActive(true);
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

    // For Testing Purposes
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
}
