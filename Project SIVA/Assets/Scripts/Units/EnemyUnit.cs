using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyUnit : MonoBehaviour
{
    // Prefab
    public GameObject enemyPrefab;

    // Enemy
    public EnemyInfo enemyInfo;

    public float speed;

    private PathFinder pathFinder;

    private List<OverlayTile> path = new List<OverlayTile>();

    private OverlayTile curr;

    private CharacterInfo target;

    public MouseController mc;

    void Start()
    {
        pathFinder = new PathFinder();                 
    }
    

    void LateUpdate()
    {
        if (enemyInfo != null && enemyInfo.hitpoints <= 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            BattleSimulator.Instance.State = BattleState.END;
        }
        if (BattleSimulator.Instance.State == BattleState.ENEMY_TURN)
        {
            target = mc.character;
            /*
             * In the event that player is not instantiated, but enemy has
             * spawned in, there will be a debug message and no further
             * functions will be performed
             */
            if (target == null)
            {
                print("Target has no active tile");
                return;
            }

            // Instantiate enemy
            if (enemyInfo == null)
            {
                BoundsInt bounds =
                    MapController.Instance.GetComponentInChildren<Tilemap>().cellBounds;

                int x = int.MaxValue;
                int y = int.MaxValue;

                /*
                 * Search in tilemap dictionary for a valid tile to place the
                 * enemy.
                 * 
                 * This is purely for prototyping and testing purposes.
                 * Enemies on the levels will have predetermined spawn areas/locations.
                 */
                while (!MapController.Instance.map.ContainsKey(new Vector2Int(x,
                    y)))
                {
                    x = Random.Range(bounds.min.x, bounds.max.x);
                    y = Random.Range(bounds.min.y, bounds.max.y);
                }

                curr = MapController.Instance.map[new Vector2Int(x, y)];
                
                enemyInfo = Instantiate(enemyPrefab, gameObject.transform).GetComponent<EnemyInfo>();

                PositionEnemyOnTile(curr);

                curr.enemyOnTile = enemyInfo;              
            }

            
            gameObject.GetComponent<SpriteRenderer>().sortingOrder =
                curr.GetComponent<SpriteRenderer>().sortingOrder;
            
            if (path.Count > 0)
            {
                MoveAlongPath();
            }
        }
    }

    /*
     * Pathfind from current tile --> Player's active tile
     */
    public void MoveTrigger()
    {
        
        path = pathFinder.FindPath(enemyInfo.activeTile,
            MapController.Instance.GetNeighborTiles(target.activeTile)[0]);
        
    }

    /*
     * Function for enemy to emit light in a 3x3 area.
     * 
     * This is meant for a certain type of enemy only. Not every enemy will
     * emit light.
     */
    private void EmitLight(OverlayTile curr, bool trigger)
    {
        // Trigger == true --> Emit light
        // Trigger == false --> Turn off light (meant for moving and dynamic
        // lighting)
        int factor = trigger ? 1 : -1;

        List<OverlayTile> neighbors = MapController.Instance.Get3x3Grid(curr);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.isBlocked || !MapController.Instance.map.ContainsKey(new
                Vector2Int(neighbor.gridLocation.x , neighbor.gridLocation.y)) ||
                Mathf.Abs(curr.gridLocation.z - neighbor.gridLocation.z) > 1)
            {
                continue;
            }

            MapController.Instance.map[new Vector2Int(neighbor.gridLocation.x,
                neighbor.gridLocation.y)].light_level += factor;
        }
    }

    private void MoveAlongPath()
    {
        //EmitLight(enemyInfo.activeTile, false);

        enemyInfo.activeTile.enemyOnTile = null;

        var step = speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        enemyInfo.transform.position = Vector2.MoveTowards(enemyInfo.transform.position,
            path[0].transform.position, step);

        enemyInfo.transform.position = new Vector3(enemyInfo.transform.position.x,
            enemyInfo.transform.position.y, zIndex);

        if (Vector2.Distance(enemyInfo.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionEnemyOnTile(path[0]);
            path.RemoveAt(0);
        }
        //EmitLight(enemyInfo.activeTile, true);

        enemyInfo.activeTile.enemyOnTile = enemyInfo;
        
    }

    private void PositionEnemyOnTile(OverlayTile tile)
    { 
        enemyInfo.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y,
            tile.transform.position.z);

        enemyInfo.GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;

        enemyInfo.activeTile = tile;
    }
}
