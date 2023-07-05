using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialCutsceneRunner : CutsceneRunner
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RunCutscene()
    {

        StartCoroutine(StartCutscene());
    }

    IEnumerator StartCutscene()
    {
        Debug.Log("Cutscene function called");
        while (!EnemyManager.Instance.spawnComplete || !MapController.Instance.generationComplete)
        {
            yield return null;
        }
        List<Enemy> moveList = EnemyManager.Instance.enemyMap.Values.ToList();
        foreach (Enemy enemy in moveList)
        {
            Debug.Log("Moving");
            PathFinder pf = new();
            OverlayTile dest = MapController.Instance.map[new Vector2Int(enemy.activeTile.gridLocation2d.x - 2, enemy.activeTile.gridLocation2d.y)];
            enemy.path = pf.FindPath(enemy.activeTile, dest, MapController.Instance.map.Values.ToList(), 1);
            enemy.UMove();
        }
        yield return null;
    }
}
