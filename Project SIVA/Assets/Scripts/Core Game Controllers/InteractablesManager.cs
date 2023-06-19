using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractablesManager : MonoBehaviour
{
    private static InteractablesManager _instance;

    public static InteractablesManager Instance { get { return _instance; } }

    // Array to store location of spawned interactables
    
    
    public GameObject lampPostPrefab;

    public Dictionary<Vector2Int, GameObject> entitySpawns;
    // Tile coordinates to interactable mapping
    public Dictionary<Vector2Int, Interactable> entityMap;

    private bool spawnComplete;
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
        entitySpawns = new();
        entityMap = new Dictionary<Vector2Int, Interactable>();
        spawnComplete = false;
    }
    void Start()
    {
        
    }
        
    void Update()
    {
        if (!spawnComplete && entityMap.Count != entitySpawns.Count)
        {
            foreach (KeyValuePair<Vector2Int, GameObject> kvp in entitySpawns)
            {
                // Possible workaround for abstraction:
                // TryGetComponent() method
                
                entityMap.Add(kvp.Key, Instantiate(kvp.Value).GetComponent<Interactable>());

                OverlayTile tile = MapController.Instance.map[kvp.Key];

                tile.isBlocked = true;
                
                PositionOnTile(entityMap[kvp.Key], tile);
            }
        }
    }
    public void HighlightAll()
    {
        foreach (Interactable interactable in entityMap.Values)
        {
            interactable.Highlight();
        }
    }
    
    public void Interact(OverlayTile curr)
    {
        Vector2Int coordinates = new Vector2Int(curr.gridLocation.x, curr.gridLocation.y);
        entityMap[coordinates].ReceiveInteraction();
    }
    // Should I move this somewhere else
    private void PositionOnTile(Interactable interactable, OverlayTile tile)
    {
        interactable.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        interactable.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        interactable.activeTile = tile;
    }
}
