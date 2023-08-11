using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIActionBoxManager : Publisher, IObserver
{
    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject attackBox;
    [SerializeField] Button attackButton;
    [SerializeField] Button returnButton;
    [SerializeField] Button endTurnButton;

    public float transitionTime = 1f;
    private bool isInAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        actionBox.SetActive(true);
        attackBox.SetActive(false);
        BattleSimulator.Instance.AddObserver(this);
        //endTurnButton.interactable = false;
    }

    public void ActionPhase()
    {
        if (isInAnimation == true)
        {
            return;
        }
        StartCoroutine(ActionPhaseAnimation());
    }

    public void AttackPhase()
    {
        if (isInAnimation == true)
        {
            return;
        }
        NotifyObservers(GameEvents.AttackButtonPressed);
        StartCoroutine(AttackPhaseAnimation());
    }

    IEnumerator AttackPhaseAnimation()
    {
        isInAnimation = true;
        attackBox.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        actionBox.SetActive(false);
        isInAnimation = false;
    }

    IEnumerator ActionPhaseAnimation()
    {
        isInAnimation = true;
        actionBox.SetActive(true);
        attackBox.SetActive(false);
        yield return new WaitForSeconds(transitionTime);
        //attackBox.SetActive(false);
        isInAnimation = false;
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerWin)
        {
            endTurnButton.GetComponent<Button>().interactable = false;
        }
        else if (gameEvent == GameEvents.PlayerTurn && !BattleSimulator.Instance.levelComplete)
        {
            endTurnButton.GetComponent<Button>().interactable = true;
        }
        else if (gameEvent == GameEvents.EnemyTurn)
        {
            endTurnButton.GetComponent<Button>().interactable = false;
        }
    }
}
