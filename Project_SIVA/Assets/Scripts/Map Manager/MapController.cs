using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Tilemaps;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class MapController : MonoBehaviour
{
    private static MapController _instance;

    public bool isCutscene;

    public static MapController Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;

    public GameObject overlayContainer;

    public bool generationComplete = false;

    public Dictionary<Vector2Int, OverlayTile> map;

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
    }

    void Start()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();
        var tileMap = gameObject.GetComponent<Tilemap>();

        BoundsInt bounds = tileMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    // 3D coordinates of current cell
                    var tileLocation = new Vector3Int(x, y, z);

                    // 2D coordinates (key for map)
                    var tileKey = new Vector2Int(x, y);

                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        // Create the invisible overlay
                        var overlayTile = Instantiate(overlayTilePrefab,
                            overlayContainer.transform);
                        // Return the centre of the cell
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x,
                            cellWorldPosition.y, cellWorldPosition.z + 1);

                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder =
                            tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                        //  Set the overlay 3D coordinates to the current tile's location
                        //  ( this does NOT move the overlay )
                        overlayTile.gridLocation = tileLocation;
                        map.Add(tileKey, overlayTile);
                    }
                }
            }
        }

        generationComplete = true;

    }

    private void Update()
    {
        if (!isCutscene)
        {
            foreach (OverlayTile tile in map.Values)
            {
                tile.ShowTile();
            }
        }
    }

    public List<OverlayTile> Get3x3Grid(OverlayTile curr)
    {


        int[,] directions = new int[9, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 }, { 0, -1 },
            { -1, 0 }, { 1, -1 }, { -1, 1 }, { -1, -1 }  };

        List<OverlayTile> neighbors = new();

        for (int i = 0; i < 9; i++)
        {
            Vector2Int locationToCheck = new Vector2Int(curr.gridLocation.x + directions[i, 0],
                curr.gridLocation.y + directions[i, 1]);

            if (map.ContainsKey(locationToCheck))
            {
                neighbors.Add(map[locationToCheck]);
            }
        }

        return neighbors;
    }

    /*
	 * Given an overlay tile, returns an array of tiles that go out of center in a + sign
	 */
    public List<OverlayTile> GetPlusShapedAlongCenter(OverlayTile center, int reach)
    {
        int[,] directions = new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        List<OverlayTile> plusShaped = new();

        for (int mult = 1; mult <= reach; mult++)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2Int locationToCheck = new Vector2Int(center.gridLocation.x + (directions[i, 0] * mult),
                    center.gridLocation.y + (directions[i, 1] * mult));

                if (map.ContainsKey(locationToCheck))
                {
                    plusShaped.Add(map[locationToCheck]);
                }
            }
        }

        return plusShaped;
    }

    public List<OverlayTile> GetAoeAttackTiles(OverlayTile cursor, OverlayTile reference, int reach)
    {
        int[,] directions = new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        List<OverlayTile> result = new();

        int xDiff = cursor.gridLocation.x - reference.gridLocation.x;
        int yDiff = cursor.gridLocation.y - reference.gridLocation.y;

        int dir;

        if (yDiff == 0)
        {
            dir = xDiff > 0 ? 1 : 3;

            for (int mult = 1; mult <= reach; mult++)
            {
                Vector2Int locationToCheck = new(reference.gridLocation.x + (directions[dir, 0] * mult),
                    reference.gridLocation.y + (directions[dir, 1] * mult));

                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.y += 1;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.y -= 2;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

            }
        }
        else if (xDiff == 0)
        {
            dir = yDiff > 0 ? 0 : 2;

            for (int mult = 1; mult <= reach; mult++)
            {
                Vector2Int locationToCheck = new(reference.gridLocation.x + (directions[dir, 0] * mult),
                    reference.gridLocation.y + (directions[dir, 1] * mult));

                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.x += 1;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.x -= 2;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

            }
        }

        return result;
    }

    public List<OverlayTile> GetAllAoeTiles(OverlayTile reference, int reach)
    {
        int[,] directions = new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        List<OverlayTile> result = new();

        for (int mult = 1; mult <= reach; mult++)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2Int locationToCheck = new(reference.gridLocation.x + (directions[i, 0] * mult),
                    reference.gridLocation.y + (directions[i, 1] * mult));

                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.y += 1;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.y -= 2;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck = new Vector2Int(reference.gridLocation.x + (directions[i, 0] * mult),
                    reference.gridLocation.y + (directions[i, 1] * mult));

                locationToCheck.x += 1;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);

                locationToCheck.x -= 2;
                if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);
            }
        }

        return result;
    }

    public List<OverlayTile> GetUniDirectional(OverlayTile reference, int dir, int range)
    {
        int[,] directions = new int[4, 2] { { 1, 0 }, { 0, -1 }, { -1, 0 }, { 0, 1 } };
        List<OverlayTile> result = new();
        for (int mult = 1; mult <= range; mult++)
        {
            Vector2Int locationToCheck = new(reference.gridLocation.x + (directions[dir, 0] * mult),
                    reference.gridLocation.y + (directions[dir, 1] * mult));

            if (map.ContainsKey(locationToCheck)) result.Add(map[locationToCheck]);
        }

        return result;
    }

    public OverlayTile GetNearestShadowTile(OverlayTile cur)
    {
        OverlayTile nearest = null;
        PathFinder pathFinder = new();
        foreach (OverlayTile tile in map.Values)
        {
            if (tile.light_level > 0 || tile.isBlocked) continue;

            if (nearest == null)
            {
                nearest = tile;
            }
            else if (pathFinder.GetManhattenDistance(cur, tile) < pathFinder.GetManhattenDistance(cur, nearest))
            {
                nearest = tile;
            }
        }
        return nearest;
    }
}
