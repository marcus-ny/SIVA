using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class Soldier : Enemy
{
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
        state_moving = false;
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
        /*
        if (path.Count > 0)
        {
            state_moving = true;
            MoveAlongPath();
            //BattleSimulator.Instance.moving = false;
        } else
        {
            state_moving = false;
        }*/
    }
   
    // Delay the updating of path instead of thinking about how to sequence it
    public override void Action()
    {
        
        if (actionsPerformed <= maxAP)
        {
            SoldierAttack();
            SoldierMove();            
        }
        
    }

    private void SoldierAttack()
    {
        
        bool inAttackRange = (Mathf.Abs(activeTile.gridLocation.x - player.activeTile.gridLocation.x) < 2)
            && (Mathf.Abs(activeTile.gridLocation.y - player.activeTile.gridLocation.y) < 2);
        if (inAttackRange)
        {
            DamageManager.Instance.DealDamageToPlayer(5.0f);
            actionsPerformed += 2;
        }
    }
    private void SoldierMove()
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
                state_moving = true;
                break;
            }            
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
