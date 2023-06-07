using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawnGenerator : MonoBehaviour
{
    List<Vector2Int> spawnLocations;
    // Start is called before the first frame update
    void Start()
    {
        

        var EnemySpawnMap = gameObject.GetComponent<Tilemap>();

        BoundsInt bounds = EnemySpawnMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    
                    if (EnemySpawnMap.HasTile(tileLocation) && !EnemyManager.Instance.enemySpawns.ContainsKey(tileKey))
                    {
                        GameObject prefab = EnemySpawnMap.GetTile<EnemyTile>(tileLocation).enemy_prefab;
                        EnemyManager.Instance.enemySpawns.Add(tileKey, prefab);
                        
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }

            // Update is called once per frame
    void Update()
    {
        
    }
}
