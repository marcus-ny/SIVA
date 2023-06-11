using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }

    public Dictionary<Vector2Int, GameObject> enemySpawns;

    public Dictionary<Vector2Int, Enemy> enemyMap;

    public GameObject soldier_Prefab;

    public PlayerController playerController;

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
        
    }

    private void Update()
    {
        // Keeping count of which enemies have died
        
        /*
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
        }*/

        // Enemy spawning (only carried out once)
        if (!spawnComplete && enemySpawns.Count != enemyMap.Count)
        {
            //Debug.Log(enemySpawns.Count);
            foreach (KeyValuePair<Vector2Int, GameObject> spawn in enemySpawns)
            {
                enemyMap.Add(spawn.Key, Instantiate(spawn.Value, gameObject.transform).GetComponent<Enemy>());

                OverlayTile tile = MapController.Instance.map[spawn.Key];

                Enemy curr = enemyMap[spawn.Key];

                curr.PositionEnemyOnTile(tile);

                
            }

            // This boolean is used to make sure that enemies do not spawn again after
            // getting killed. I could empty the array but altering array elements in
            // runtime is not ideal.
            spawnComplete = true;
        }

        if (enemyMap.Count == 0)
        {
            BattleSimulator.Instance.State = BattleState.PLAYER_WIN;
        }


    }

    // Might use a PQ for this but need a PQ that supports modifying elements and priority
    // Returns the ally with the lowest hp ratio
    public Enemy GetLowestHpAlly()
    {
        Enemy lowest = null; ;
        foreach (KeyValuePair<Vector2Int, Enemy> enemy in enemyMap)
        {
            if (lowest == null) {
                lowest = enemy.Value;
            } else
            {
                lowest = enemy.Value.hpRatio < lowest.hpRatio ? enemy.Value : lowest;
            }
        }

        return lowest;
    }

    public List<Mechanic> FindMechanicLocations()
    {
        List<Mechanic> mechanics = new();
        List<OverlayTile> locations = new();
        foreach (KeyValuePair<Vector2Int, Enemy> kvp in enemyMap)
        {
            if(kvp.Value.GetType() == typeof(Mechanic))
            {
                // Safe to typecast?
                mechanics.Add((Mechanic)kvp.Value);
            }
        }
        
        return mechanics;
    }
}
