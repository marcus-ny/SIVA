using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;

    public static PlayerController Instance { get { return _instance; } }

    // Manually tie this in unity
    public GameObject character_prefab;

    public Vector2Int playerSpawn;
    // Manually tie this in unity
    public CharacterInfo character;

    public List<OverlayTile> path;
    public List<OverlayTile> reachableTiles;
    PathFinder pathFinder;
    Rangefinder rangeFinder;

    private OverlayTile destinationTile;

    private readonly int speed = 3;

    private void Start()
    {
        pathFinder = new();
        rangeFinder = new();
        path = new();
        reachableTiles = new();
    }

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

    private void Update()
    {
        if (BattleSimulator.Instance.State == BattleState.PLAYER_TURN) {
            var tileHit = MouseController.Instance.GetFocusedOnTile();
            OverlayTile overlayTile;
            if (tileHit.HasValue)
            {

                overlayTile = tileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
                
                if (Input.GetMouseButtonDown(0))
                {
                    if (character == null)
                    {
                        character = Instantiate(character_prefab).GetComponent<CharacterInfo>();
                        // Place the enemy on spawn
                        PositionCharacterOnTile(MapController.Instance.map[playerSpawn]);
                        destinationTile = character.activeTile;
                        GetMovementRange();
                    }
                    else
                    {
                        destinationTile = overlayTile;
                    }
                }
            }

            /*
            if (path.Count > 0)
            {
                MoveAlongPath();
            }
            else if (path.Count == 0 && character != null)
            {
                character.cur = character.activeTile.gridLocation;
                character.prev = character.activeTile.gridLocation;


                GetMovementRange();
            }*/
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (path.Count > 0)
        {
            var step = speed * Time.deltaTime;

            var zIndex = path[0].transform.position.z;

            character.activeTile.isBlocked = false;

            // This animation code should be abstracted to somewhere else
            // Keep this file only for controlling the player and nothing else
            character.prev = path[0].previous.gridLocation;
            character.cur = path[0].gridLocation;

            character.transform.position = Vector2.MoveTowards(character.transform.position,
                path[0].transform.position, step);

            character.transform.position = new Vector3(character.transform.position.x,
                character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
            {
                PositionCharacterOnTile(path[0]);
                path.RemoveAt(0);
            }

            yield return null;
        }

        if (path.Count == 0)
        {
            character.prev = character.cur = character.activeTile.gridLocation;
            GetMovementRange();
        }
        // character.activeTile.isBlocked = true;
    }

    public bool AttackTrigger()
    {
        bool inRange = (character.activeTile.gridLocation.x - destinationTile.gridLocation.x < 2) && (character.activeTile.gridLocation.y - destinationTile.gridLocation.y < 2);
        if (destinationTile.enemy != null && inRange)
        {
            DamageManager.Instance.DealDamageToEnemy(80, destinationTile.enemy);
            return true;
        }
        else if (destinationTile.enemy == null)
        {
            Debug.Log("No target");
        }
        else if (!inRange)
        {
            Debug.Log("Target out of range");
        }
        return false;
    }

    IEnumerator WaitForMovementInput()
    {
        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        path = pathFinder.FindPath(character.activeTile, destinationTile, reachableTiles);
        Coroutine movingCoroutine = StartCoroutine(MoveAlongPath());

    }
    public bool MoveTrigger()
    {
        path = pathFinder.FindPath(character.activeTile, destinationTile, reachableTiles);
        if (path.Count == 0) return false;
        Coroutine movingCoroutine = StartCoroutine(MoveAlongPath());
        //StartCoroutine(WaitForMovementInput());
        //Debug.Log("Is this executed early?");

        return true;
    }

    public bool InteractTrigger()
    {
        bool inRange = (character.activeTile.gridLocation.x - destinationTile.gridLocation.x < 2) && (character.activeTile.gridLocation.y - destinationTile.gridLocation.y < 2);
        Vector2Int coordinates = new(destinationTile.gridLocation.x, destinationTile.gridLocation.y);
        if (inRange && InteractablesManager.Instance.entityMap.ContainsKey(coordinates))
        {
            InteractablesManager.Instance.Interact(destinationTile);
            return true;
        }
        else if (!inRange)
        {
            Debug.Log("Interactable out of range!");
        }
        else
        {
            Debug.Log("There is no interactable on this tile");
        }
        return false;
    }

    private void GetMovementRange()
    {
        foreach (var tile in reachableTiles)
        {
            tile.HideTile();
        }

        reachableTiles = rangeFinder.GetReachableTiles(character.activeTile, 3);

        foreach (var tile in reachableTiles)
        {
            tile.ShowTile();
        }
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x,
            tile.transform.position.y, tile.transform.position.z);

        character.GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
        character.activeTile.isBlocked = true;
    }
}
