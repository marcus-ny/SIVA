using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCutscenePt2Runner : CutsceneRunner
{
    [SerializeField] TurnsManager turnsManager;
    // Start is called before the first frame update
    void Start()
    {
        //turnsManager.AddObserver(this);
        
        RunCutscene();
    }



    public override void RunCutscene()
    {

        StartCoroutine(StartCutscene());
    }


    IEnumerator StartCutscene()
    {
        yield return new WaitForSecondsRealtime(1);
        EnemyManager.Instance.enemyMap.Clear();
        //yield return null;
    }

    
}
