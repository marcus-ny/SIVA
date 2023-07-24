using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBoss : Enemy
{
    private bool weakened;
    private VampireBossAnimator animator;
    public override void Action()
    {
        StartCoroutine(TakeTurn());
    }
    IEnumerator TakeTurn()
    {
        while (actionsPerformed < maxAP)
        {
            while (state_moving)
            {
                yield return null;
            }
            if (weakened) {
                EscapeLight();
            }
            else if (DetectLowAllies())
            {
                Debug.Log("Boss detects an ally that is close to death");
                StartCoroutine(KillAlly());

            }
            else
            {
                Teleport();
                MeleeAttack();
            }
            actionsPerformed = maxAP;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new PathFinder();
        path = new();
        rangeFinder = new Rangefinder();
        range = new();
        weakened = false;
        animator = GetComponent<VampireBossAnimator>();
        // There should be a more intelligent way to set this variable
        maxAP = 4;
        hitpoints = maxHp = 300;

        actionsPerformed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = PlayerController.Instance.character;
        }
        
        if (hitpoints <= 0)
        {
            TriggerDeath();
        }

        if (activeTile != null && activeTile.light_level > 0)
        {
            Weaken();
        }

        
        
    }
    public void Weaken()
    {
        weakened = true;
        animator.currState = VampireBossAnimator.AnimationState.Weakened;
    }
    public override void TakeDamage(float damage)
    {
        if (weakened)
        {
            damage = damage * 2;
        }
        else
        {
            damage = 1;
        }
        hitpoints -= damage;
        DamageManager.Instance.RaiseEventEnemyHealthAltered(damage, this);
    }

    IEnumerator StartMeleeAttack()
    {
        int atkspeed = 3;
        while (state_moving)
        {
            yield return null;
        }
        state_moving = true;
        animator.currState = VampireBossAnimator.AnimationState.MeleeIn;
        yield return new WaitForSecondsRealtime(0.5f);

        animator.currState = VampireBossAnimator.AnimationState.MeleeInProgress;
        // Moving is here
        Vector2 initialPos = transform.position;
        while (Vector2.Distance(transform.position, player.activeTile.transform.position) > 0.001f)
        {
            var step = atkspeed * Time.deltaTime;
            var zIndex = player.activeTile.transform.position.z;
            transform.position = Vector2.MoveTowards(transform.position,
                player.activeTile.transform.position, step);
            transform.position = new Vector3(transform.position.x,
               transform.position.y, zIndex);
            yield return null;
        }

        while (Vector2.Distance(transform.position, initialPos) > 0.001f)
        {
            var step = atkspeed * Time.deltaTime;
            var zIndex = player.activeTile.transform.position.z;
            transform.position = Vector2.MoveTowards(transform.position,
                initialPos, step);
            transform.position = new Vector3(transform.position.x,
               transform.position.y, zIndex);
            yield return null;
        }
        animator.currState = VampireBossAnimator.AnimationState.MeleeOut;
        //yield return new WaitForSecondsRealtime(0.5f);

        // --------------
        DamageManager.Instance.DealDamageToPlayer(5.0f);
        yield return new WaitForSecondsRealtime(0.5f);
        animator.currState = VampireBossAnimator.AnimationState.Idle;
        //yield return new WaitForSeconds(0.8f);
        state_moving = false;
    }
    public void MeleeAttack()
    {
        List<OverlayTile> attackTiles = MapController.Instance.Get3x3Grid(activeTile);
        if (attackTiles.Contains(player.activeTile))
        {
            StartCoroutine(StartMeleeAttack());
        }
    }
    protected IEnumerator TeleportCoroutine(OverlayTile destinationTile)
    {
        while (state_moving)
        {
            yield return null;
        }

        state_moving = true;

        animator.currState = VampireBossAnimator.AnimationState.TeleportIn;

        activeTile.isBlocked = false;
        EnemyManager.Instance.enemyMap.Remove(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y));
        activeTile.enemy = null;
            // Before

        var step = SPEED * Time.deltaTime;
        var zIndex = transform.position.z;
        yield return new WaitForSecondsRealtime(1);
        PositionEnemyOnTile(destinationTile);
        animator.currState = VampireBossAnimator.AnimationState.TeleportOut;
        activeTile.enemy = this;
        activeTile.isBlocked = true;
        EnemyManager.Instance.enemyMap.Add(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y), this);
        // yield return null;
        yield return new WaitForSecondsRealtime(0.5f);
        animator.currState = VampireBossAnimator.AnimationState.Idle;
        state_moving = false;

    }
    private void Teleport()
    {
        List<OverlayTile> toFind = GetClosestTileToPlayer();
        //range = rangeFinder.GetReachableTiles(activeTile, 3);
        
        foreach (OverlayTile tile in toFind)
        {
            // If the tile is blocked, can't be teleported to. Skip
            if (tile.isBlocked || tile.light_level > 0) continue;
            // If boss is already within 1 block range, no need to teleport, skip
            if (pathFinder.GetManhattenDistance(activeTile, player.activeTile)
                == pathFinder.GetManhattenDistance(tile, player.activeTile))
            {
                break;
            }

            
            StartCoroutine(TeleportCoroutine(tile));
            break;
        }
        
        
        
    }

    private void EscapeLight()
    {
        OverlayTile destination = MapController.Instance.GetNearestShadowTile(activeTile);
        if (destination == null) return;
        StartCoroutine(TeleportCoroutine(destination));
        /*
        path = pathFinder.FindPath(activeTile, destination, new List<OverlayTile>(), 1);
        
        if (path.Count == 0)
        {
            actionsPerformed = maxAP;
        }
        Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());*/
    }

    private bool DetectLowAllies()
    {
        foreach (Enemy enemy in EnemyManager.Instance.enemyMap.Values)
        {
            if (enemy == this) continue;

            if (enemy.hpRatio < 0.2)
            {
                return true;
            }
        }
        return false;
    }
    private IEnumerator KillAlly()
    {
        foreach (Enemy enemy in EnemyManager.Instance.enemyMap.Values)
        {
            if (enemy == this) continue;

            if (enemy.hpRatio < 0.2)
            {
                TeleportToAlly(enemy);
                yield return new WaitForSecondsRealtime(1);


                int atkspeed = 3;
                while (state_moving)
                {
                    yield return null;
                }
                state_moving = true;
                animator.currState = VampireBossAnimator.AnimationState.MeleeIn;
                yield return new WaitForSecondsRealtime(0.5f);

                animator.currState = VampireBossAnimator.AnimationState.MeleeInProgress;
                // Moving is here
                Vector2 initialPos = transform.position;
                while (Vector2.Distance(transform.position, enemy.activeTile.transform.position) > 0.001f)
                {
                    var step = atkspeed * Time.deltaTime;
                    var zIndex = enemy.activeTile.transform.position.z;
                    transform.position = Vector2.MoveTowards(transform.position,
                        enemy.activeTile.transform.position, step);
                    transform.position = new Vector3(transform.position.x,
                       transform.position.y, zIndex);
                    yield return null;
                }

                while (Vector2.Distance(transform.position, initialPos) > 0.001f)
                {
                    var step = atkspeed * Time.deltaTime;
                    var zIndex = enemy.activeTile.transform.position.z;
                    transform.position = Vector2.MoveTowards(transform.position,
                        initialPos, step);
                    transform.position = new Vector3(transform.position.x,
                       transform.position.y, zIndex);
                    yield return null;
                }
                animator.currState = VampireBossAnimator.AnimationState.MeleeOut;
                //yield return new WaitForSecondsRealtime(0.5f);

                // --------------
                enemy.TriggerDeath();
                //yield return new WaitForSeconds(0.8f);
                state_moving = false;
                hitpoints += 30;
                break;
            }
        }
    }

    // Get a tile next to the player
    public List<OverlayTile> GetClosestTileToPlayer()
    {
        if (player == null)
        {
            return null;
        }

        List<OverlayTile> result = MapController.Instance.Get3x3Grid(player.activeTile);
        
        return result;
    }

    private List<OverlayTile> GetClosestTileToLowAlly(Enemy enemy)
    {
        List<OverlayTile> result = MapController.Instance.Get3x3Grid(enemy.activeTile);
        return result;
    }

    private void TeleportToAlly(Enemy enemy)
    {
        List<OverlayTile> toFind = GetClosestTileToLowAlly(enemy);
        //range = rangeFinder.GetReachableTiles(activeTile, 3);

        foreach (OverlayTile tile in toFind)
        {
            // If the tile is blocked, can't be teleported to. Skip
            if (tile.isBlocked || tile.light_level > 0) continue;
            // If boss is already within 1 block range, no need to teleport, skip
            if (pathFinder.GetManhattenDistance(activeTile, player.activeTile)
                == pathFinder.GetManhattenDistance(tile, player.activeTile))
            {
                break;
            }


            StartCoroutine(TeleportCoroutine(tile));
            break;
        }
    }

    public override string ToString()
    {
        return "Vampire Boss";
    }
}
