using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWinSceneSkipper : MonoBehaviour, IObserver
{
    [SerializeField] TurnsManager turnsManager;

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerWin)
        {
            StartCoroutine(PassLevel());
            //PassLevelF();
        } else if (gameEvent == GameEvents.PlayerLose)
        {
            StartCoroutine(FailLevel());
            //FailLevelF();
        }
    }

    IEnumerator PassLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        ScenesManager.Instance.LoadNextScene();
    }

    public void PassLevelF()
    {
        ScenesManager.Instance.LoadNextScene();
    }

    public void FailLevelF()
    {
        ScenesManager.Instance.ReloadScene();
    }
    IEnumerator FailLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        ScenesManager.Instance.ReloadScene();
    }
    // Start is called before the first frame update
    void Start()
    {
        BattleSimulator.Instance.AddObserver(this);
    }



    

    
}
