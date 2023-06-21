using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, TRANSITION, PLAYER_WIN, ENEMY_WIN }
public class BattleSimulator : Publisher
{
    private static BattleSimulator _instance;
    public static BattleSimulator Instance { get { return _instance; } }

    public BattleState State; // Change to private later

    public readonly int MAX_ACTIONS = 4; // Constant

    public int actionsPerformed;

    /*
     * These variables are temporarily set to 1 each for testing purposes
     * They are also set to public so that we can assign the test prefabs
     * As the number of enemy and units under player's control grows, these variables will be changed to arrays - partyPlayer and partyEnemy
     * And the code will be adjusted accordingly
     */

    public List<Enemy> enemyList;
    public PlayerController player;

    public bool moving;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        enemyList = new();
    }
    // Start is called before the first frame update
    void Start()
    {
        State = BattleState.START;
        actionsPerformed = 0;
    }

    public void StartGame()
    {
        if (State == BattleState.START) {
            State = BattleState.PLAYER_TURN;
            NotifyObservers(GameEvents.PlayerTurn);
            Debug.Log("NotifyObserversStartPlayerTurn");
        }
    }

    private void Update()
    {
        if (enemyList.Count != EnemyManager.Instance.transform.childCount)
        {
            for (int i = 0; i < EnemyManager.Instance.transform.childCount; i++)
            {
                //enemyList.Clear();
                enemyList.Add(EnemyManager.Instance.transform.GetChild(i).GetComponent<Enemy>());
            }
        }
        // Debug.Log("Enemy list: " + enemyList.Count);
        if (State == BattleState.ENEMY_TURN)
        {
            Coroutine EnemyCoroutine = StartCoroutine(EnemyTakeActions());
            State = BattleState.TRANSITION;
            
        }
        // Debug.Log("Battlestate: " + State);
    }

    /*
     * All enemy actions sequentially executed
     */
    IEnumerator EnemyTakeActions()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        //foreach (KeyValuePair<Vector2Int, Enemy> enemy in EnemyManager.Instance.enemyMap)
        foreach (Enemy enemy in enemyList)
        {
            enemy.Action();
            while (enemy.state_moving)
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1.0f);

        }
        switchTurns();

    }
    
    public void switchTurns()
    {
        if(State == BattleState.PLAYER_TURN)
        {           
            // Change to EnemyTurn and notify observers
            State = BattleState.ENEMY_TURN;
            NotifyObservers(GameEvents.EnemyTurn);
            

            foreach (Enemy enemy in enemyList)
            {
                // Reset AP
                enemy.actionsPerformed = 0;
            }
            // reset
            actionsPerformed = 0;
        } else if(State == BattleState.TRANSITION)
        {
            foreach (Enemy enemy in enemyList)
            {
                // Reset AP
                enemy.actionsPerformed = 0;
            }
            // Change to PlayerTurn and notify observers
            State = BattleState.PLAYER_TURN;
            NotifyObservers(GameEvents.PlayerTurn);
            

            // Check for tickDamage
            DamageManager.Instance.tickDamage();

            // reset
            actionsPerformed = 0;
        }
    }

    IEnumerator WaitForInteractInput()
    {
        InteractablesManager.Instance.HightlightAll(true);
        while (!Input.GetMouseButtonDown(0))
        {
            player.GetInteractRange();
            yield return null;
        }
        if (player.InteractTrigger())
        {
            
            actionsPerformed += 1;
        }
        InteractablesManager.Instance.HightlightAll(false);
    }
    public void InteractItem()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }
        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForInteractInput());
        }
    }
    public void MoveUnit()
    {
        // If a party runs out of action points
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForPlayerMoveInput());
            
        }
    }
    IEnumerator WaitForPlayerMoveInput()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            player.GetMovementRange();
            yield return null;           
        }
        if (player.MoveTrigger())
        {
            actionsPerformed += 1;
        }
    }
    IEnumerator WaitForMeleeInput()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            // Show attack range tiles
            player.GetMeleeRange();
            yield return null;
        }
        if (player.MeleeTrigger())
        {
            actionsPerformed += 1;
        }
    }
    IEnumerator WaitForAoeInput()
    {
        Debug.Log("Coroutine started");
        while (!Input.GetMouseButtonDown(0))
        {
            player.ShowAoeTiles();
            yield return null;
        }
        if (player.AoeAttackTrigger())
        {
            actionsPerformed += 2;
        }
    }
    public void DealMeleeDamage()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            Debug.Log("Deal damage button clicked");
            StartCoroutine(WaitForMeleeInput());
        }
    }
    public void DealAOEDamage()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
           // Debug.Log("Deal damage button clicked");
            StartCoroutine(WaitForAoeInput());
        }
    }

    public void EnemyWin()
    {
        State = BattleState.ENEMY_WIN;
        NotifyObservers(GameEvents.PlayerLose);
    }

    public void EnemyLose()
    {
        State = BattleState.PLAYER_WIN;
        NotifyObservers(GameEvents.PlayerWin);
    }
}
