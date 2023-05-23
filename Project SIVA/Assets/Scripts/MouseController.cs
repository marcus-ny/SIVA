using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public GameObject characterPrefab;
    public CharacterInfo character;

    public float speed;
    private List<OverlayTile> path = new List<OverlayTile>();

    private PathFinder pathFinder;

    public BattleSimulator battleSim;

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new PathFinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (battleSim.State == BattleState.PLAYER_TURN)
        {


            var focusedTileHit = GetFocusedOnTile();

            if (focusedTileHit.HasValue)
            {
                OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
                transform.position = overlayTile.transform.position;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

                if (Input.GetMouseButtonDown(0))
                {
                    overlayTile.GetComponent<OverlayTile>().ShowTile();

                    if (character == null)
                    {
                        character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnTile(overlayTile);
                    }
                    else
                    {
                        path = pathFinder.FindPath(character.activeTile, overlayTile);
                        print("Player moves to " + overlayTile.gridLocation.ToString());
                    }
                }
            }
            

            if (path.Count > 0)
            {

                MoveAlongPath();
            }
        }
        
    }

    private void MoveAlongPath()
    {
        // print("Player path count " + path.Count);
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        // Mouse click may hit multiple tiles, but we always want to get the top layer
        // Get a list of tiles hit --> Pick the first one in order
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        // print("Cursor is at " + tile.gridLocation.x + ", " + tile.gridLocation.y + ", " + tile.gridLocation.z);
        // character.transform.position = new Vector3(tile.gridLocation.x, tile.gridLocation.y, tile.gridLocation.z);
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }
}
