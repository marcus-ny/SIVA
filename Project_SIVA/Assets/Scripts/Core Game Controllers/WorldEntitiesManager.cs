using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * A singleton that manages all obstacles on top of the tilemap (obstacles, interactables, etc.)
 */
public class WorldEntitiesManager : Publisher
{
    private static WorldEntitiesManager _instance;

    public static WorldEntitiesManager Instance { get { return _instance; } }

    // A map keeping track of entity locations in edit mode. WorldEntitiesSpawnManager accesses this
    public Dictionary<Vector2Int, GameObject> entitySpawns;

    // Tile coordinates to interactable mapping
    public Dictionary<Vector2Int, WorldEntity> entityMap;

    // An indicator to check whether items have spawned
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

            if (interactable is IInteractable highlightTarget)
            {
                highlightTarget.Highlight(trigger);
            }
        }
    }
        
    void Update()
    {
        if (!spawnComplete && entityMap.Count != entitySpawns.Count)
        {
            foreach (KeyValuePair<Vector2Int, GameObject> kvp in entitySpawns)
            {
                entityMap.Add(kvp.Key, Instantiate(kvp.Value, transform).GetComponent<WorldEntity>());

                OverlayTile tile = MapController.Instance.map[kvp.Key];

                tile.isBlocked = true;

                PositionOnTile(entityMap[kvp.Key], tile);
            }

            spawnComplete = true;
        }
        
    }
    
    // Interact with the object of type IInteractable if any, on the given tile
    public bool Interact(OverlayTile curr)
    {
        
        Vector2Int coordinates = new(curr.gridLocation.x, curr.gridLocation.y);
        if (entityMap[coordinates] is IInteractable interactable)
        {
            if (interactable.ReceiveInteraction())
            {
                return true;
            }

        }
        return false;

    }

    // Place the given interactable on the correct tile position
    private void PositionOnTile(WorldEntity interactable, OverlayTile tile)
    {
        interactable.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        interactable.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        interactable.activeTile = tile;
    }
}
