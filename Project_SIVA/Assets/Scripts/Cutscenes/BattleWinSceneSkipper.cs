using System.Collections;
using UnityEngine;

/*
 * Can this class be incorporated into BattleSimulator and only use the DialogueSkipper for non-
 * combat levels
 */
public class BattleWinSceneSkipper : MonoBehaviour, IObserver
{
    [SerializeField] TurnsManager turnsManager;

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerWin)
        {
            StartCoroutine(PassLevel());
        }
        else if (gameEvent == GameEvents.PlayerLose)
        {
            StartCoroutine(FailLevel());
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
