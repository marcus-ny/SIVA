using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBoss : Enemy, IObserver
{
    List<OverlayTile> previousLightSpot;
    private bool disabled;
    private int turnsSinceDisabled;
    private void Start()
    {
        maxAP = 4;
        maxHp = 350;
        previousLightSpot = new();
        actionsPerformed = 0;
        disabled = false;
        turnsSinceDisabled = 0;
    }
    public override void Action()
    {
        if (disabled && turnsSinceDisabled < 2) {
            // Shoulder lights disabled
            foreach (OverlayTile tile in previousLightSpot)
            {
                tile.AlterLightLevel(false);
            }
            previousLightSpot.Clear();
            turnsSinceDisabled += 1;
            // Skip turn
            actionsPerformed = maxAP;
        } else if (disabled && turnsSinceDisabled >= 2)
        {
            disabled = false;
            turnsSinceDisabled = 0;
            // Watch out for dangerous recursive call here
            Action();
        } else
        {
            // All turn actions go here
            // No need for state machine since this is not a universal enemy

            TracePlayer();
            // Valid Melee Range
            OverlayTile tempRef = MapController.Instance.map[new Vector2Int(activeTile.gridLocation.x - 1, activeTile.gridLocation.y)];
            List<OverlayTile> meleeTiles = MapController.Instance.GetAoeAttackTiles(tempRef, activeTile, 4);

            if (meleeTiles.Contains(PlayerController.Instance.character.activeTile))
            {
                MeleeAttack();
            } else
            {
                RangedAttack();
            }
        }
        
    }

    public override void TakeDamage(float damage)
    {
        if (disabled)
        {
            hitpoints -= damage * 1.5f;
        } else
        {
           // Raise a dialogue saying player can't hit the boss
        }
    }

    IEnumerator TakeTurn()
    {
        while (actionsPerformed < maxAP)
        {
            while (state_moving)
            {
                yield return null;
            }


        }
    }

    // Method for tracking lights from shoulders
    private void TracePlayer()
    {
        foreach (OverlayTile tile in previousLightSpot)
        {
            tile.AlterLightLevel(false);
        }

        OverlayTile playerTile = PlayerController.Instance.character.activeTile;

        List<OverlayTile> lightSpots = MapController.Instance.GetPlusShapedAlongCenter(playerTile, 1);
        lightSpots.Add(playerTile);
        previousLightSpot = lightSpots;

        foreach (OverlayTile tile in lightSpots)
        {
            tile.AlterLightLevel(true);
        }
    }

    // Only in front of boss
    private void MeleeAttack()
    {
        actionsPerformed += 4;
        StartCoroutine(PerformMeleeAttack());
    }

    IEnumerator PerformMeleeAttack()
    {
        while (state_moving)
        {
            yield return null;
        }
        state_moving = true;
        DamageManager.Instance.DealDamageToPlayer(10.0f);
        yield return new WaitForSecondsRealtime(1);
        state_moving = false;
    }

    // Global (can attack anywhere on map, but lower damage than melee)
    private void RangedAttack()
    {
        actionsPerformed += 4;
        StartCoroutine(PerformRangedAttack());
    }

    IEnumerator PerformRangedAttack()
    {
        while (state_moving)
        {
            yield return null;
        }
        state_moving = true;
        DamageManager.Instance.DealDamageToPlayer(4.0f);
        yield return new WaitForSecondsRealtime(1);
        state_moving = false;
    }

    public void OnNotify(GameEvents gameEvent)
    {
        disabled = true;
    }
}
