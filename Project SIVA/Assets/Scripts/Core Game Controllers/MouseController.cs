using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject characterPrefab;

    public CharacterInfo character;

    public float speed;

    public List<OverlayTile> path = new();

    private List<OverlayTile> reachableTiles = new();

    private PathFinder pathFinder;

    private Rangefinder rangeFinder;

    private OverlayTile destinationTile;

    private static readonly string[] directions = { "Player_move_NE", "Player_move_SW", "Player_move_NW", "Player_move_SE" };

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new PathFinder();
        rangeFinder = new Rangefinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if (BattleSimulator.Instance.State == BattleState.PLAYER_TURN)
        {
            
            var focusedTileHit = GetFocusedOnTile();

            // If the raycast on cursor hits a valid cell
            if (focusedTileHit.HasValue)
            {
                OverlayTile overlayTile =
                    focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

                transform.position = overlayTile.transform.position;

                gameObject.GetComponent<SpriteRenderer>().sortingOrder =
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

                if (Input.GetMouseButtonDown(0))
                {
                    // Selected tile was highlighted by this line of code below
                    // overlayTile.GetComponent<OverlayTile>().ShowTile();

                    if (character == null)
                    {
                        character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnTile(overlayTile);
                        destinationTile = character.activeTile;
                        GetMovementRange();  
                    }
                    else
                    {
                        destinationTile = overlayTile;
                    }
                    
                }
            }
            
            if (path.Count > 0)
            {
                MoveAlongPath();
            } else if (path.Count == 0 && character != null)
            {
                character.AnimatePlayer("Player_still");
                GetMovementRange();
            }
        }
        
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

    public bool AttackTrigger()
    {
        bool inRange = (character.activeTile.gridLocation.x - destinationTile.gridLocation.x < 2) && (character.activeTile.gridLocation.y - destinationTile.gridLocation.y < 2);
        if (destinationTile.enemy != null && inRange)
        {
            DamageManager.Instance.DealDamageToEnemy(50, destinationTile.enemy);
            return true;
        } else if (destinationTile.enemy == null)
        {
            Debug.Log("No target");       
        } else if (!inRange)
        {
            Debug.Log("Target out of range");
        }
        return false;
    }

    public bool MoveTrigger()
    {
        path = pathFinder.FindPath(character.activeTile, destinationTile, reachableTiles);
        Debug.Log("Path length is: " + path.Count);
        Debug.Log("reachable tiles count: " + reachableTiles.Count);
        
        return (path.Count != 0);
    }

    public bool InteractTrigger()
    {
        bool inRange = (character.activeTile.gridLocation.x - destinationTile.gridLocation.x < 2) && (character.activeTile.gridLocation.y - destinationTile.gridLocation.y < 2);
        Vector2Int coordinates = new(destinationTile.gridLocation.x, destinationTile.gridLocation.y);
        if (inRange && InteractablesManager.Instance.entityMap.ContainsKey(coordinates))
        {
            InteractablesManager.Instance.Interact(destinationTile);
            return true;
        } else if (!inRange)
        {
            Debug.Log("Interactable out of range!");
        } else
        {
            Debug.Log("There is no interactable on this tile");
        }
        return false;
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        // This animation code should be abstracted to somewhere else
        // Keep this file only for controlling the player and nothing else
        Vector3Int prev = path[0].previous.gridLocation;
        Vector3Int cur = path[0].gridLocation;

        if (cur.x - prev.x > 0 && cur.y - prev.y == 0)
        {
            character.AnimatePlayer(directions[0]);
        }
        else if (cur.x - prev.x < 0 && cur.y - prev.y == 0)
        {
            character.AnimatePlayer(directions[1]);
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y > 0)
        {
            character.AnimatePlayer(directions[2]);
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y < 0)
        {
            character.AnimatePlayer(directions[3]);
        }
        character.transform.position = Vector2.MoveTowards(character.transform.position,
            path[0].transform.position, step);

        character.transform.position = new Vector3(character.transform.position.x,
            character.transform.position.y, zIndex);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetMovementRange();
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        // Mouse click may hit multiple tiles, but we always want to get the top layer
        // Get a list of tiles hit --> sort by descending Z value --> Pick the first one in order
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x,
            tile.transform.position.y, tile.transform.position.z);

        character.GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }
}
