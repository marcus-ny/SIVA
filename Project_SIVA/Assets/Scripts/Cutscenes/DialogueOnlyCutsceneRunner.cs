using System.Collections;
using UnityEngine;

public class DialogueOnlyCutsceneRunner : CutsceneRunner, IObserver
{
    [SerializeField] TurnsManager turnsManager;

    void Start()
    {
        turnsManager.AddObserver(this);
    }

    public override void RunCutscene()
    {

        StartCoroutine(StartCutscene());
    }

    IEnumerator StartCutscene()
    {
        yield return null;
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.DialogueEnd)
        {
            ScenesManager.Instance.LoadNextScene();
        }
    }
}
