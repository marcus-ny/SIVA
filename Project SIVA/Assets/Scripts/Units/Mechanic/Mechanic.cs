using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : Enemy
{
	MechanicBaseState currentState;

	MechanicIdleState mechanicIdleState = new();
	MechanicHealState mechanicHealState = new();
	MechanicDeadState mechanicDeadState = new();

	public Enemy allyLowHp;
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
		hitpoints = 100;

		// There should be a more intelligent way to set this variable
		maxAP = 4;

		actionsPerformed = 0;
		currentState = mechanicHealState;
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
	public void Heal()
	{
		// Lazy implementation, add a heal function later
		
		DamageManager.Instance.DealDamageToEnemy(-10, allyLowHp);
		actionsPerformed += 2;
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

			path = pathFinder.FindPath(activeTile, tile, range);

			if (path.Count > 0)
			{
				actionsPerformed += 2;
				break;
			}
		}
		if (path.Count <= 0)
		{
			SwitchState(mechanicIdleState);
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
