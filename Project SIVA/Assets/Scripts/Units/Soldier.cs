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
            SwitchState(soldierRetreatState);
        }
        Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());        
    }

    public override void TakeDamage(float damage)
    {
        hitpoints -= damage;
    }

    // Within its movable range, returns the tile closest to the player
    public List<OverlayTile> GetClosestTileToPlayer()
    {
        if (player == null)
        {
            return null;
        }
        
        List<OverlayTile> result = pathFinder.GetClosest(player.activeTile, range);
        //Debug.Log("nearest: " + result.gridLocation);
        //Debug.Log("Player: " + player.activeTile.gridLocation);
        //Debug.Log("Is blocked? : " + result.isBlocked);
        return result;
    }
}
