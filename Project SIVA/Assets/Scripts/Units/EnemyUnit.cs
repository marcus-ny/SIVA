using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyUnit : MonoBehaviour
{
    public GameObject enemyPrefab;      // Prefab

    public EnemyInfo enemy;             // Enemy

    public int speed;

    private PathFinder pathFinder;

    private List<OverlayTile> path = new List<OverlayTile>();

    OverlayTile curr;

    public CharacterInfo target;

    public MouseController mc;

    public BattleSimulator battleSim;

    void Start()
    {
        pathFinder = new PathFinder();                 
    }
    

    void LateUpdate()
    {
        if (battleSim.State == BattleState.ENEMY_TURN)
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
            if (enemy == null)
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
                 * Enemies on the levels will not be randomly placed.
                 */
                while (!MapController.Instance.map.ContainsKey(new Vector2Int(x,
                    y)))
                {
                    x = Random.Range(bounds.min.x, bounds.max.x);
                    y = Random.Range(bounds.min.y, bounds.max.y);

                    
                }

                curr = MapController.Instance.map[new Vector2Int(x, y)];
                
                enemy = Instantiate(enemyPrefab).GetComponent<EnemyInfo>();

                PositionEnemyOnTile(curr);

                EmitLight(curr, true);
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
        
        path = pathFinder.FindPath(enemy.activeTile,
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
        EmitLight(enemy.activeTile, false);
        
        var step = speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position,
            path[0].transform.position, step);

        enemy.transform.position = new Vector3(enemy.transform.position.x,
            enemy.transform.position.y, zIndex);

        if (Vector2.Distance(enemy.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionEnemyOnTile(path[0]);
            path.RemoveAt(0);
        }
        EmitLight(enemy.activeTile, true);
    }

    private void PositionEnemyOnTile(OverlayTile tile)
    { 
        enemy.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y,
            tile.transform.position.z);

        enemy.GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;

        enemy.activeTile = tile;
    }
}
