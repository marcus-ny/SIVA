using System.Collections;

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


        // There should be a more intelligent way to set this variable
        maxAP = 4;
        maxHp = 75;

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

        if (hitpoints <= 0)
        {
            TriggerDeath();
        }

        // This part needs to be looked at later

        range = rangeFinder.GetReachableTiles(activeTile, 4, 1);

    }
    public override string ToString()
    {
        return "Generator";
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

        foreach (OverlayTile tile in range)
        {
            tile.AlterLightLevel(trigger);
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

    public override void TriggerDeath()
    {
        // turns off generator light
        EmitLight(false);
        EnemyManager.Instance.KillEnemy(activeTile);
        BattleSimulator.Instance.enemyList.Remove(this);
        activeTile.isBlocked = false;
        Destroy(gameObject);

    }
}
