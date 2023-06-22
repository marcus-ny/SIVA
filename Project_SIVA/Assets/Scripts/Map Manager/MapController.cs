using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
		map = new Dictionary<Vector2Int, OverlayTile>();
		var tileMap = gameObject.GetComponent<Tilemap>();
		//var tileMap = gameObject.GetComponentInChildren<Tilemap>();
		
		BoundsInt bounds = tileMap.cellBounds;

		// Loop through all tiles within bounds
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

	}

    private void Update()
    {
        foreach (OverlayTile tile in map.Values)
        {
			tile.ShowTile();
        }
    }

    public List<OverlayTile> GetNeighborTiles(OverlayTile curr, List<OverlayTile> validTiles, int jumpHeight)
	{
		
		
		Dictionary<Vector2Int, OverlayTile> searchRange = new();

		if (validTiles.Count > 0)
		{
			foreach (var tile in validTiles)
			{
				searchRange.Add(new Vector2Int(tile.gridLocation.x, tile.gridLocation.y), tile);
			}
		} else
		{
			searchRange = map;
		}

		
		List<OverlayTile> neighbors = new();

		int[,] directions = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };

		for (int i = 0; i < 4; i++)
		{
			Vector2Int locationToCheck = new(curr.gridLocation.x + directions[i, 0],
            curr.gridLocation.y + directions[i, 1]);

			if (searchRange.ContainsKey(locationToCheck) && Mathf.Abs(curr.gridLocation.z - searchRange[locationToCheck].gridLocation.z) <= jumpHeight)
			{
				neighbors.Add(searchRange[locationToCheck]);
			}
		}
		
		return neighbors;
	}

	
	public List<OverlayTile> Get3x3Grid(OverlayTile curr)
	{
		

		int[,] directions = new int[9, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 }, { 0, -1 },
			{ -1, 0 }, { 1, -1 }, { -1, 1 }, { -1, -1 }  };

		List<OverlayTile> neighbors = new();

		for (int i = 0; i < 9; i++)
		{
			Vector2Int locationToCheck = new Vector2Int(curr.gridLocation.x + directions[i,0],
				curr.gridLocation.y + directions[i,1]);

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
    public List<OverlayTile> GetPlusShapedAlongCenter(OverlayTile center)
    {
		int[,] directions = new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

		List<OverlayTile> plusShaped = new();

		for (int mult = 1; mult <= 10; mult++)
		{
			for (int i = 0; i < 4; i++)
			{
				Vector2Int locationToCheck = new Vector2Int(center.gridLocation.x + (directions[i, 0]*mult),
					center.gridLocation.y + (directions[i, 1]*mult));

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
			// think along NW -> SE
			for (int mult = 1; mult <= reach; mult++)
			{
				Vector2Int locationToCheck = new Vector2Int(reference.gridLocation.x + (directions[dir, 0] * mult),
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
			// think along NE -> SW
			for (int mult = 1; mult <= reach; mult++)
			{
				Vector2Int locationToCheck = new Vector2Int(reference.gridLocation.x + (directions[dir, 0] * mult),
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
				Vector2Int locationToCheck = new Vector2Int(reference.gridLocation.x + (directions[i, 0] * mult),
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

}