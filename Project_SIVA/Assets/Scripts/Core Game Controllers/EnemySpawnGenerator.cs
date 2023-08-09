using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * Handles enemy spawning at the start of the game
 * Finds placed enemies on the tilemap (in edit mode) and instantiates them at runtime
 */
public class EnemySpawnGenerator : MonoBehaviour
{
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

     
}
