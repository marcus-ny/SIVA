using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class Soldier : Enemy
{
    private SoldierBaseState currentState;

    public SoldierIdleState soldierIdleState = new();
    public SoldierAggroState soldierAggroState = new();
    public SoldierRetreatState soldierRetreatState = new();
    public SoldierDeadState soldierDeadState = new();

    public void SwitchState(SoldierBaseState newState)
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
        hitpoints = 100;

        // There should be a more intelligent way to set this variable
        maxAP = 4;

        actionsPerformed = 0;
        currentState = soldierAggroState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        if (player == null)
        {
            player = EnemyManager.Instance.mc.character;
        }

        gameObject.GetComponent<SpriteRenderer>().sortingOrder =
            EnemyManager.Instance.mc.GetComponent<SpriteRenderer>().sortingOrder;

        range = rangeFinder.GetReachableTiles(activeTile, 3);
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

    public void RangeAttack()
    {
        DamageManager.Instance.DealDamageToPlayer(3.0f);
        actionsPerformed += 2;
    }
    public void MeleeAttack()
    {       
        DamageManager.Instance.DealDamageToPlayer(5.0f);
        actionsPerformed += 2;       
    }

    public void AggroMove()
    {
        List<OverlayTile> toFind = GetClosestTileToPlayer();
        //range = rangeFinder.GetReachableTiles(activeTile, 3);
        foreach (OverlayTile tile in toFind)
        {
            // If the current tile is indeed the nearest possible, then do nothing
            /*if (tile == activeTile)
            {
                return;
            }*/

            // If current tile distance to player is equal to enemy's distance to player
            // There is no point moving there and wasting AP, because you're not
            // closing the distance

            if (pathFinder.GetManhattenDistance(activeTile, player.activeTile)
                == pathFinder.GetManhattenDistance(tile, player.activeTile))
            {
                break;
            }
                   
            path = pathFinder.FindPath(activeTile, tile, range);

            if (path.Count > 0)
            {
                actionsPerformed += 2;
                break;
            }            
        }
        if (path.Count == 0)
        {
            // Skip turn
            actionsPerformed = maxAP;
        }
        Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());        
    }

    public void TakePositionForRange()
    {
        
        List<OverlayTile> toFind = GetRangeAttackTiles();
        
        //range = rangeFinder.GetReachableTiles(activeTile, 3);

        // If soldier is already in plus shaped tiles
        if (MapController.Instance.GetPlusShapedAlongCenter(player.activeTile).Contains(activeTile))
        {
            return;
        }
        // Find the nearest tile among all the plus shaped tiles
        
        foreach (OverlayTile tile in toFind)
        {
            // If the current tile is indeed the nearest possible, then do nothing
            /*if (tile == activeTile)
            {
                return;
            }*/

            // Unnecessary here because you are not pathfinding to player anymore
            /*if (pathFinder.GetManhattenDistance(activeTile, player.activeTile)
                == pathFinder.GetManhattenDistance(tile, player.activeTile))
            {
                break;
            }*/

            path = pathFinder.FindPath(activeTile, tile, range);

            if (path.Count > 0)
            {
                actionsPerformed += 2;
                break;
            }
        }
        // Be careful of this segment below
        // Is it necessary for range movement
        if (path.Count == 0)
        {
            // Skip turn
            actionsPerformed = maxAP;
        }
        Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());
    }

    public void RetreatMove()
    {

        List<OverlayTile> toFind = FindNearestMechanicLocation();              
        // Find the nearest tile among all the plus shaped tiles
        
        
        
        foreach (OverlayTile tile in toFind)
        {
            // If the current tile is indeed the nearest possible, then do nothing
            /*if (tile == activeTile)
            {
                return;
            }*/

            Mechanic mechanic = EnemyManager.Instance.FindMechanicLocations()[0];
            
            if (pathFinder.GetManhattenDistance(activeTile, mechanic.activeTile)
                == pathFinder.GetManhattenDistance(tile, mechanic.activeTile))
            {
                break;
            }

            path = pathFinder.FindPath(activeTile, tile, range);

            if (path.Count > 0)
            {
                Debug.Log("path count: " + path.Count);
                actionsPerformed += 2;
                break;
            }
        }
        // Be careful of this segment below
        // Is it necessary for range movement
        if (path.Count == 0)
        {
            // Skip turn
            actionsPerformed = maxAP;
            Debug.Log("turn skipped in retreat");
        }
        Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());
    }

    

    
    // Within its movable range, returns the tile closest to the player
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

    public List<OverlayTile> GetRangeAttackTiles()
    {
        if (player == null)
        {
            return null;
        }

        // Returns a list of all tiles in plus shape around the player
        List<OverlayTile> plusShaped =
            MapController.Instance.GetPlusShapedAlongCenter(player.activeTile);
        // Find the tile closest to the enemy among plusShaped
        List<OverlayTile> nearestTiles = pathFinder.GetClosestTilesInRange(activeTile, plusShaped);
        // We can be sure that nearest tiles always exist
        List<OverlayTile> result = pathFinder.GetClosestTilesInRange(nearestTiles[0], range);

        return result;
    }

    public List<OverlayTile> FindNearestMechanicLocation()
    {
        List<Mechanic> mechanicsLocations = EnemyManager.Instance.FindMechanicLocations();
        
        //List<OverlayTile> nearestTiles = pathFinder.GetClosestTilesInRange(activeTile, mechanicsLocations);
        //Debug.Log("nearest tiles count: " + nearestTiles.Count);
        List<OverlayTile> result = pathFinder.GetClosestTilesInRange(mechanicsLocations[0].activeTile, range);

        return result;
    }
}
