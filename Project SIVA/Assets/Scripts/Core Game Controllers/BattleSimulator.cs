using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, TRANSITION, END }
public class BattleSimulator : MonoBehaviour
{
    private static BattleSimulator _instance;
    public static BattleSimulator Instance { get { return _instance; } }

    public BattleState State; // Change to private later

    public readonly int MAX_ACTIONS = 2; // Constant

    public int actionsPerformed;

    /*
     * These variables are temporarily set to 1 each for testing purposes
     * They are also set to public so that we can assign the test prefabs
     * As the number of enemy and units under player's control grows, these variables will be changed to arrays - partyPlayer and partyEnemy
     * And the code will be adjusted accordingly
     */

    List<Enemy> enemyList;
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
        State = BattleState.PLAYER_TURN;
        actionsPerformed = 0;
    }

    public void StartGame()
    {
        if (State == BattleState.START) {
            State = BattleState.PLAYER_TURN;
        }
    }

    private void Update()
    {
        if (enemyList.Count != EnemyManager.Instance.transform.childCount)
        {
            for (int i = 0; i < EnemyManager.Instance.transform.childCount; i++)
            {
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
    IEnumerator EnemyTakeActions()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.Action();
            while (enemy.state_moving)
            {
                yield return null;
            }
            //yield return new WaitForSecondsRealtime(0.3f);
        }
    }
    
    public void switchTurns()
    {
        if(State == BattleState.PLAYER_TURN)
        {           
            State = BattleState.ENEMY_TURN;
            
            // reset
            actionsPerformed = 0;
        } else if(State == BattleState.TRANSITION)
        {
            foreach (Enemy enemy in enemyList)
            {
                // Reset AP
                enemy.actionsPerformed = 0;
            }
            State = BattleState.PLAYER_TURN;
            DamageManager.Instance.tickDamage();

            // reset
            actionsPerformed = 0;
        }
    }
    public void InteractItem()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }
        if (State == BattleState.PLAYER_TURN)
        {
            if (player.InteractTrigger())
            {
                actionsPerformed += 1;
            }
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
    public void DealDamage()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            if(player.AttackTrigger()) actionsPerformed += 1;
        }
    }
}
