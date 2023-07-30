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
        } else if (gameEvent == GameEvents.PlayerLose)
        {
            StartCoroutine(FailLevel());
        }
    }

    IEnumerator PassLevel()
    {
        yield return new WaitForSecondsRealtime(3f);
        ScenesManager.Instance.LoadNextScene();
    }

    IEnumerator FailLevel()
    {
        yield return new WaitForSecondsRealtime(3f);
        ScenesManager.Instance.ReloadScene();
    }
    // Start is called before the first frame update
    void Start()
    {
        BattleSimulator.Instance.AddObserver(this);
    }



    

    
}
