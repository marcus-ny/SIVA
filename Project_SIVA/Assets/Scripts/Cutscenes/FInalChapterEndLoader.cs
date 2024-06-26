using System.Collections;
using UnityEngine;

public class FInalChapterEndLoader : CutsceneRunner, IObserver
{
    [SerializeField] TurnsManager turnsManager;
    // Start is called before the first frame update
    void Start()
    {
        turnsManager.AddObserver(this);
        foreach (OverlayTile tile in MapController.Instance.map.Values)
        {
            tile.HideTile();
        }
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
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}
