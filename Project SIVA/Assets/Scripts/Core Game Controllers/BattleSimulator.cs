using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, END }
public class BattleSimulator : MonoBehaviour
{
    public BattleState State; // Change to private later

    private int MAX_ACTIONS = 2; // Constant

    private int actionsPerformed;

    /*
     * These variables are temporarily set to 1 each for testing purposes
     * They are also set to public so that we can assign the test prefabs
     * As the number of enemy and units under player's control grows, these variables will be changed to arrays - partyPlayer and partyEnemy
     * And the code will be adjusted accordingly
     */
    public EnemyUnit enemy;

    public MouseController player;

    // Start is called before the first frame update
    void Start()
    {
        State = BattleState.PLAYER_TURN;
        actionsPerformed = 0;
    }

    
    public void switchTurns()
    {
        if(State == BattleState.PLAYER_TURN)
        {
            // print("It is now enemy's turn");
            State = BattleState.ENEMY_TURN;

            // reset
            actionsPerformed = 0;
        } else if(State == BattleState.ENEMY_TURN)
        {
            // print("It is now player's turn");
            State = BattleState.PLAYER_TURN;
            DamageManager.Instance.tickDamage();

            // reset
            actionsPerformed = 0;
        }
    }

    public void MoveUnit()
    {
        // If a party runs out of action points
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        actionsPerformed += 1;

        if (State == BattleState.ENEMY_TURN)
        {
            enemy.MoveTrigger();
        } else if (State == BattleState.PLAYER_TURN)
        {
            player.MoveTrigger();
        }
    }
}
