using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }

    private List<Vector2Int> enemySpawns;

    public Dictionary<Vector2Int, Enemy> enemyMap;

    public GameObject soldier_Prefab;

    public MouseController mc;

    private bool spawnComplete = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        enemySpawns = new();
        enemyMap = new();
    }

    private void Start()
    {
        // Test
        enemySpawns.Add(new Vector2Int(3, -4));
    }

    private void Update()
    {
        foreach (KeyValuePair<Vector2Int, Enemy> kvp in enemyMap)
        {
            int defeated = 0;
            if(kvp.Value != null && kvp.Value.hitpoints <= 0)
            {
                Destroy(transform.GetChild(0).gameObject);
                defeated++;
                kvp.Value.activeTile.enemy = null;
            }

            if (defeated == enemyMap.Count)
            {
                BattleSimulator.Instance.State = BattleState.END;
            }
        }
        if (!spawnComplete && enemySpawns.Count != enemyMap.Count)
        {
            foreach (Vector2Int spawn in enemySpawns)
            {
                enemyMap.Add(spawn, Instantiate(soldier_Prefab, gameObject.transform).GetComponent<Enemy>());

                OverlayTile tile = MapController.Instance.map[spawn];

                Enemy curr = enemyMap[spawn];

                curr.PositionEnemyOnTile(tile);
            }

            // This boolean is used to make sure that enemies do not spawn again after
            // getting killed. I could empty the array but altering array elements in
            // runtime is not ideal.
            spawnComplete = true;
        }

        if (BattleSimulator.Instance.State == BattleState.ENEMY_TURN)
        {
            foreach (KeyValuePair<Vector2Int, Enemy> kvp in enemyMap)
            {
                kvp.Value.Action();
            }
        }
    }
}
