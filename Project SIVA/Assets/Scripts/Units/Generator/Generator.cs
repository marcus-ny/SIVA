using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Enemy
{
    GeneratorBaseState currentState;

    public GeneratorHiddenState generatorHiddenState = new();
    public GeneratorActiveState generatorActiveState = new();
    public GeneratorDeadState generatorDeadState = new();

    public void SwitchState(GeneratorBaseState nextState)
    {
        currentState = nextState;
        currentState.EnterState(this);
    }
    // Start is called before the first frame update
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
        currentState = generatorHiddenState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (player == null)
        {
            player = EnemyManager.Instance.playerController.character;
        }

        //gameObject.GetComponent<SpriteRenderer>().sortingOrder =
        //EnemyManager.Instance.playerController.GetComponent<SpriteRenderer>().sortingOrder;

        // This part needs to be looked at later
        
        range = rangeFinder.GetReachableTiles(activeTile, 4);
        
    }

    public override void Action()
    {
        StartCoroutine("TakeTurn");
    }

    IEnumerator TakeTurn()
    {
        currentState.UpdateState(this);
        yield return null;
    }

    // Trigger = true for ON, false for OFF
    public void EmitLight(bool trigger)
    {
        int factor = trigger ? 1 : -1;
        foreach (OverlayTile tile in range)
        {
            tile.light_level += factor;
        }
    }

    // Scan the generator's range for player existence
    // To be used in triggering the trap
    public bool PlayerDetectedInRange()
    {
        foreach (OverlayTile tile in range)
        {
            if (tile == player.activeTile) return true;
        }

        return false;
    }
}
