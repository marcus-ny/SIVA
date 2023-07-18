using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldEntitiesSpawnManager : MonoBehaviour
{
    List<Vector2Int> spawnLocations;
    // Start is called before the first frame update
    void Start()
    {
        var InteractableSpawnMap = gameObject.GetComponent<Tilemap>();

        BoundsInt bounds = InteractableSpawnMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);

                    if (InteractableSpawnMap.HasTile(tileLocation) && !EnemyManager.Instance.enemySpawns.ContainsKey(tileKey))
                    {
                        if (InteractableSpawnMap.GetTile<InteractableTile>(tileLocation) == null) Debug.Log("Interactable is null here");
                        GameObject prefab = InteractableSpawnMap.GetTile<InteractableTile>(tileLocation).interactable_prefab;

                        WorldEntitiesManager.Instance.entitySpawns.Add(tileKey, prefab);

                    }
                }
            }
        }

        gameObject.SetActive(false);
    }
}
