using System.Collections;
using System.Collections.Generic;
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

    public List<OverlayTile> GetNeighborTiles(OverlayTile curr, List<OverlayTile> validTiles)
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

			if (searchRange.ContainsKey(locationToCheck) && Mathf.Abs(curr.gridLocation.z - searchRange[locationToCheck].gridLocation.z) <= 1)
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
}
