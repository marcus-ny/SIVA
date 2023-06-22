using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Drone : Enemy
{
    DroneBaseState currentState;

    public DroneIdleState droneIdleState = new();
    public DroneSearchState droneSearchState = new();

    public DroneAnimationController animationController;

    public void SwitchState(DroneBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        pathFinder = new PathFinder();
        path = new();
        rangeFinder = new Rangefinder();
        range = new();


        // There should be a more intelligent way to set this variable
        maxAP = 2;
        maxHp = 50;

        actionsPerformed = 0;
        currentState = droneIdleState;
        currentState.EnterState(this);
        animationController = gameObject.GetComponent<DroneAnimationController>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = EnemyManager.Instance.playerController.character;
        }
        //Debug.Log("Drone HP RATIO: " + hpRatio);
        if (hitpoints <= 0)
        {
            TriggerDeath();
        }
        range = rangeFinder.GetReachableTiles(activeTile, 5, 2);
    }
    public override void Action()
    {
        StartCoroutine("TakeTurn");
    }

    IEnumerator TakeTurn()
    {
        while (actionsPerformed < maxAP)
        {
            currentState.UpdateState(this);
            while (BattleSimulator.Instance.moving)
            {
                yield return null;
            }
        }
    }
    public override string ToString()
    {
        return "Drone";
    }
    public void EmitLight(bool trigger)
    {        
        List<OverlayTile> lighting = MapController.Instance.Get3x3Grid(activeTile);
        foreach (OverlayTile tile in lighting)
        {
            tile.AlterLightLevel(trigger);
        }
    }
    IEnumerator DroneMovement()
    {
        bool startedMoving = false;
        while (state_moving)
        {
            yield return null;
            startedMoving = true;
        }
        if (startedMoving)
            EmitLight(true);
    }

    public void Search()
    {
        
        List<OverlayTile> toFind = GetClosestTileToPlayer();
        //range = rangeFinder.GetReachableTiles(activeTile, 3);
        foreach (OverlayTile tile in toFind)
        {
            // If current tile distance to player is equal to enemy's distance to player
            // There is no point moving there and wasting AP, because you're not
            // closing the distance

            if (pathFinder.GetManhattenDistance(activeTile, player.activeTile)
                == pathFinder.GetManhattenDistance(tile, player.activeTile))
            {
                break;
            }

            path = pathFinder.FindPath(activeTile, tile, range, 2);

            if (path.Count > 0)
            {
                EmitLight(false);
                actionsPerformed += 2;
                break;
            }
        }
        // If the drone is closest to the player as possible, no need to move
        if (path.Count == 0)
        {
            // Skip turn
            actionsPerformed = maxAP;
        }

        Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());

        StartCoroutine(DroneMovement());
    }

    public override void PositionEnemyOnTile(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y,
            tile.transform.position.z + 2);

        GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;

        activeTile = tile;
        activeTile.enemy = this;
        activeTile.isBlocked = true;
    }

    public List<OverlayTile> GetClosestTileToPlayer()
    {
        if (player == null)
        {
            return null;
        }

        List<OverlayTile> result = pathFinder.GetClosestTilesInRange(player.activeTile, range);
        //Debug.Log("nearest: " + result.gridLocation);
        //Debug.Log("Player: " + player.activeTile.gridLocation);
        //Debug.Log("Is blocked? : " + result.isBlocked);
        return result;
    }

    public override void TriggerDeath()
    {
        // turns off generator light
        EmitLight(false);
        EnemyManager.Instance.enemyMap.Remove(new
        Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y));
        BattleSimulator.Instance.enemyList.Remove(this);
        Destroy(gameObject);
    }
}
