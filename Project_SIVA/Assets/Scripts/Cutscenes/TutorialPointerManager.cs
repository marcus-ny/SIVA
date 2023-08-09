using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPointerManager : MonoBehaviour, IObserver
{
    [SerializeField] GameEvents triggerEvent;
    [SerializeField] GameEvents completionEvent;
    [SerializeField] TurnsManager turnsManager;
    [SerializeField] GenerationDelay genDelay;
    [SerializeField] int delayTimeInSeconds;
    [SerializeField] UIActionBoxManager actionBoxManager;

    public bool completed;

    public enum GenerationDelay { Instantaneous, Delayed }

    private void Awake()
    {
        ShowTips(false);
    }

    private void Start()
    {
        completed = false;
        BattleSimulator.Instance.AddObserver(this);
        PlayerController.Instance.AddObserver(this);
        WorldEntitiesManager.Instance.AddObserver(this);
        turnsManager.AddObserver(this);
        actionBoxManager.AddObserver(this);       
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (completed) return;
        
        if (gameEvent == triggerEvent)
        {
            if (genDelay == GenerationDelay.Instantaneous) ShowTips(true);
            else if (genDelay == GenerationDelay.Delayed) StartCoroutine(ShowTipsIntro(true));

        } else if (gameEvent == completionEvent)
        {
            ShowTips(false);
            completed = true;
        }
    }

    IEnumerator ShowTipsIntro(bool trigger)
    {
        yield return new WaitForSecondsRealtime(delayTimeInSeconds);
        ShowTips(trigger);
    }

    /*
     * Iterate through the children tooltips and activates them one by one
     */
    private void ShowTips(bool trigger)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(trigger);
        }
    }
    
}
