using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Publisher
{
    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }

    public Dictionary<Vector2Int, GameObject> enemySpawns;

    public Dictionary<Vector2Int, Enemy> enemyMap;

    public PlayerController playerController;

    public bool spawnComplete = false;

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

    private void Update()
    {
        // Enemy spawning (only carried out once)
        if (!spawnComplete && enemySpawns.Count != enemyMap.Count)
        {

            foreach (KeyValuePair<Vector2Int, GameObject> spawn in enemySpawns)
            {
                enemyMap.Add(spawn.Key, Instantiate(spawn.Value, gameObject.transform).GetComponent<Enemy>());

                OverlayTile tile = MapController.Instance.map[spawn.Key];

                Enemy curr = enemyMap[spawn.Key];

                curr.PositionEnemyOnTile(tile);
            }

            spawnComplete = true;

        }
        else if (!spawnComplete && enemySpawns.Count == 0)
        {
            spawnComplete = true;
            NotifyObservers(GameEvents.PlayerWin);

        }

    }

    // Might use a PQ for this but need a PQ that supports modifying elements and priority
    // Returns the ally with the lowest hp ratio
    public Enemy GetLowestHpAlly()
    {
        Enemy lowest = null;
        foreach (KeyValuePair<Vector2Int, Enemy> enemy in enemyMap)
        {
            if (lowest == null)
            {
                lowest = enemy.Value;
            }
            else
            {
                lowest = enemy.Value.hpRatio < lowest.hpRatio ? enemy.Value : lowest;
            }
        }

        return lowest;
    }

    public void KillEnemy(OverlayTile tile)
    {
        enemyMap.Remove(new
            Vector2Int(tile.gridLocation.x, tile.gridLocation.y));
        if (enemyMap.Count == 0)
        {
            NotifyObservers(GameEvents.PlayerWin);
        }
    }

    public List<Mechanic> FindMechanicLocations()
    {
        List<Mechanic> mechanics = new();
        List<OverlayTile> locations = new();
        foreach (KeyValuePair<Vector2Int, Enemy> kvp in enemyMap)
        {
            if (kvp.Value.GetType() == typeof(Mechanic))
            {
                // Safe to typecast?
                mechanics.Add((Mechanic)kvp.Value);
            }
        }

        return mechanics;
    }
}
