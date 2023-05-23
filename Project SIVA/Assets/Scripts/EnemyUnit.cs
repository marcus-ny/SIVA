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
            
            if (Input.GetMouseButtonDown(2))
            {
                target = mc.character;

                if (target == null)
                {
                    print("Target has no active tile");
                    return;
                }

                

                if (enemy == null)
                {
                    curr = MapController.Instance.GetNeighborTiles(target.activeTile)[0];
                    enemy = Instantiate(enemyPrefab).GetComponent<EnemyInfo>();
                    PositionEnemyOnTile(curr);
                    emitLight(curr, true);
                }

                //transform.position = target.transform.position;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = curr.GetComponent<SpriteRenderer>().sortingOrder;

                // print("Path finding FROM " + enemy.activeTile.gridLocation.ToString() + " TO " + target.activeTile.gridLocation.ToString());
                path = pathFinder.FindPath(enemy.activeTile, MapController.Instance.GetNeighborTiles(target.activeTile)[0]);
            }


            if (path.Count > 0)
            {
                MoveAlongPath();
            }
        }
    }
    private void emitLight(OverlayTile curr, bool trigger)
    {
        int factor = trigger ? 1 : -1;

        List<OverlayTile> neighbors = MapController.Instance.GetNineByNine(curr);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.isBlocked || !MapController.Instance.map.ContainsKey(new Vector2Int(neighbor.gridLocation.x , neighbor.gridLocation.y)) || Mathf.Abs(curr.gridLocation.z - neighbor.gridLocation.z) > 1)
            {
                continue;
            }

            MapController.Instance.map[new Vector2Int(neighbor.gridLocation.x, neighbor.gridLocation.y)].light_level += factor;
        }
    }
    private void MoveAlongPath()
    {
        emitLight(enemy.activeTile, false);
        // print("Path count " + path.Count);
        // print("Enemy attempting to move to " + path[0].gridLocation.ToString());
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, path[0].transform.position, step);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, zIndex);

        if (Vector2.Distance(enemy.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionEnemyOnTile(path[0]);
            path.RemoveAt(0);
        }
        emitLight(enemy.activeTile, true);
    }

    private void PositionEnemyOnTile(OverlayTile tile)
    {
        // print("Placed enemy at coordinates" + tile.transform.position.x + ", " + tile.transform.position.y + ", " + tile.transform.position.z);
        // print("Placed enemy at coordinates" + tile.gridLocation.x + ", " + tile.gridLocation.y + ", " + tile.gridLocation.z);
        
        enemy.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        enemy.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        enemy.activeTile = tile;
    }
}
