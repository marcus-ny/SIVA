using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : Enemy
{
	MechanicBaseState currentState;

	public MechanicIdleState mechanicIdleState = new();
	public MechanicHealState mechanicHealState = new();
	public MechanicDeadState mechanicDeadState = new();

	public Enemy allyLowHp;

	MechanicAnimationController animationController;
	public void SwitchState(MechanicBaseState newState)
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
		maxAP = 4;
		maxHp = 75;

		actionsPerformed = 0;
		currentState = mechanicIdleState;
		currentState.EnterState(this);
		animationController = gameObject.GetComponent<MechanicAnimationController>();
	}

	private void Update()
	{
		if (player == null)
		{
			player = EnemyManager.Instance.playerController.character;
		}

		if (hitpoints <= 0)
		{
			TriggerDeath();
		}

		range = rangeFinder.GetReachableTiles(activeTile, 3, 1);
	}

	public override void Action()
	{

		StartCoroutine("TakeTurn");

	}
	public override string ToString()
	{
		return "Mechanic";
	}
	IEnumerator TakeTurn()
	{
		while (actionsPerformed < maxAP)
		{
			currentState.UpdateState(this);
			while (state_moving)
			{
				yield return null;
			}
		}
	}
	IEnumerator startHealing()
	{
        while (state_moving)
        {
            yield return null;
        }
        state_moving = true;
		// lazy, fix later
		if (allyLowHp == this)
		{
			// skip turn temporarily

			actionsPerformed = maxAP;
		}
		else
		{
			DamageManager.Instance.HealEnemy(10, allyLowHp);
		}
        animationController.status = MechanicAnimationController.Status.HEALING;
        yield return new WaitForSeconds(0.8f);
        animationController.status = MechanicAnimationController.Status.NIL;
        state_moving = false;
    }
	public void Heal()
	{
		actionsPerformed += 2;
		StartCoroutine(startHealing());
	}

	public void MoveToAlly()
	{
		List<OverlayTile> toFind = GetClosestTileToAlly();
		//range = rangeFinder.GetReachableTiles(activeTile, 3);
		foreach (OverlayTile tile in toFind)
		{
			// If the current tile is indeed the nearest possible, then do nothing
			/*if (tile == activeTile)
			{
				return;
			}*/
			if (pathFinder.GetManhattenDistance(activeTile, allyLowHp.activeTile)
				== pathFinder.GetManhattenDistance(tile, allyLowHp.activeTile))
			{
				break;
			}

			path = pathFinder.FindPath(activeTile, tile, range, 1);

			if (path.Count > 0)
			{
				actionsPerformed += 2;
				break;
			}
		}
		if (path.Count == 0)
		{
			actionsPerformed = maxAP;
		}
		Coroutine MovingCoroutine = StartCoroutine(MoveAlongPath());
	}

	

	// Within its movable range, returns the tile closest to the player
	public List<OverlayTile> GetClosestTileToAlly()
	{
		allyLowHp = EnemyManager.Instance.GetLowestHpAlly();

		List<OverlayTile> result = pathFinder.GetClosestTilesInRange(allyLowHp.activeTile, range);

		return result;
	}

	public List<OverlayTile> FindNearestMechanicLocation()
	{
		return null;
	}
}
