using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldEntitiesManager : MonoBehaviour
{
    private static WorldEntitiesManager _instance;

    public static WorldEntitiesManager Instance { get { return _instance; } }

    // Array to store location of spawned interactables
    public GameObject lampPostPrefab;

    public Dictionary<Vector2Int, GameObject> entitySpawns;
    // Tile coordinates to interactable mapping
    public Dictionary<Vector2Int, WorldEntity> entityMap;

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
        entityMap = new Dictionary<Vector2Int, WorldEntity>();
        spawnComplete = false;
    }

    public void HightlightAll(bool trigger)
    {
        foreach (WorldEntity interactable in entityMap.Values)
        {
            interactable.Highlight(trigger);
        }
    }
        
    void Update()
    {
        if (!spawnComplete && entityMap.Count != entitySpawns.Count)
        {
            foreach (KeyValuePair<Vector2Int, GameObject> kvp in entitySpawns)
            {
                // Possible workaround for abstraction:
                // TryGetComponent() method
                
                entityMap.Add(kvp.Key, Instantiate(kvp.Value).GetComponent<WorldEntity>());

                OverlayTile tile = MapController.Instance.map[kvp.Key];

                tile.isBlocked = true;
                
                PositionOnTile(entityMap[kvp.Key], tile);
            }
        }
    }
    
    
    public void Interact(OverlayTile curr)
    {
        
        Vector2Int coordinates = new Vector2Int(curr.gridLocation.x, curr.gridLocation.y);
        if (entityMap[coordinates] is IInteractable)
        {
            IInteractable interactable = (IInteractable) entityMap[coordinates];
            interactable.ReceiveInteraction();
        }
        
    }
    // Should I move this somewhere else
    private void PositionOnTile(WorldEntity interactable, OverlayTile tile)
    {
        interactable.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        interactable.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        interactable.activeTile = tile;
    }
}