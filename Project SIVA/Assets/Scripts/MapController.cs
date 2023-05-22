using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private static MapController _instance;

    public static MapController Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;

    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();         // 2D coordinates to tile mapping
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();     // Get the tilemap component of this grid

        BoundsInt bounds = tileMap.cellBounds;                          // Getting map bounds of the tilemap

        // Loop through all tiles within bounds
        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);         // 3D coordinates of current cell
                    var tileKey = new Vector2Int(x, y);                 // 2D coordinates (key for map)
                    // print(tileLocation.ToString());
                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))     // If there is a tile at this 3D location and that tile is NOT contained in the map
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);       // Create the invisible overlay
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);                   // Return the centre of the cell

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);    // Put the overlay on TOP of cell (hence z + 1)
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;     // Set sorting order for safety
                        overlayTile.gridLocation = tileLocation;        //  Set the overlay 3D coordinates to the current tile's location ( this does NOT move the overlay )
                        map.Add(tileKey, overlayTile);                  //  Add to map
                    }
                }
            }
        }

    }

    public List<OverlayTile> GetNeighborTiles(OverlayTile curr)
    {
        var map = MapController.Instance.map;

        List<OverlayTile> neighbors = new List<OverlayTile>();

        Vector2Int locationToCheck = new Vector2Int(
            curr.gridLocation.x,
            curr.gridLocation.y + 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }

        locationToCheck = new Vector2Int(
            curr.gridLocation.x + 1,
            curr.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }

        locationToCheck = new Vector2Int(
            curr.gridLocation.x - 1,
            curr.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }

        locationToCheck = new Vector2Int(
            curr.gridLocation.x,
            curr.gridLocation.y - 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }

        return neighbors;
    }

    
    public List<OverlayTile> GetNineByNine(OverlayTile curr)
    {
        var map = MapController.Instance.map;
        int[,] directions = new int[9, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 }, { 0, -1 }, { -1, 0 }, { 1, -1 }, { -1, 1 }, { -1, -1 }  };
        List<OverlayTile> neighbors = new List<OverlayTile>();

        for (int i = 0; i < 9; i++)
        {
            Vector2Int locationToCheck = new Vector2Int(curr.gridLocation.x + directions[i,0], curr.gridLocation.y + directions[i,1]);
            if (map.ContainsKey(locationToCheck))
            {
                neighbors.Add(map[locationToCheck]);
            }
        }

        return neighbors;
    }
}
