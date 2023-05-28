using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractablesManager : MonoBehaviour
{
    private static InteractablesManager _instance;

    public static InteractablesManager Instance { get { return _instance; } }

    // Array to store location of spawned interactables
    private List<Vector2Int> entityLocations;
    
    public GameObject lampPostPrefab;

    // Tile coordinates to interactable mapping
    public Dictionary<Vector2Int, Interactable> entityMap;
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
        entityLocations = new List<Vector2Int>();
        entityMap = new Dictionary<Vector2Int, Interactable>();
    }
    void Start()
    {
        entityLocations.Add(new Vector2Int(0, 0));
        entityLocations.Add(new Vector2Int(-3, -5));
    }
        
    void Update()
    {
        if (entityMap.Count != entityLocations.Count)
        {
            foreach (Vector2Int location in entityLocations)
            {
                entityMap.Add(location, Instantiate(lampPostPrefab).GetComponent<LampPost>());

                OverlayTile tile = MapController.Instance.map[location];

                tile.isBlocked = true;
                
                PositionOnTile(entityMap[location], tile);
            }
        }
    }

    
    public void Interact(OverlayTile curr)
    {
        Vector2Int coordinates = new Vector2Int(curr.gridLocation.x, curr.gridLocation.y);
        entityMap[coordinates].ReceiveInteraction();
    }
    
    private void PositionOnTile(Interactable interactable, OverlayTile tile)
    {
        interactable.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        interactable.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        interactable.activeTile = tile;
    }
}
