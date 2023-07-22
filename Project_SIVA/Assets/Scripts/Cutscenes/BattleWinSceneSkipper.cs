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
            ScenesManager.Instance.LoadNextScene();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BattleSimulator.Instance.AddObserver(this);
    }



    

    
}
