using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawnManager : MonoBehaviour
{
    Vector2Int spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        var PlayerSpawnMap = gameObject.GetComponent<Tilemap>();

        BoundsInt bounds = PlayerSpawnMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);

                    if (PlayerSpawnMap.HasTile(tileLocation))
                    {
                        PlayerController.Instance.playerSpawn = tileKey;
                        break;
                        // Pass this value into the player manager
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }
}
