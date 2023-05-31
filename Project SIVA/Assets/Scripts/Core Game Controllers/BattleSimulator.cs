using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, END }
public class BattleSimulator : MonoBehaviour
{
    private static BattleSimulator _instance;
    public static BattleSimulator Instance { get { return _instance; } }

    public BattleState State; // Change to private later

    public readonly int MAX_ACTIONS = 20; // Constant

    public int actionsPerformed;

    /*
     * These variables are temporarily set to 1 each for testing purposes
     * They are also set to public so that we can assign the test prefabs
     * As the number of enemy and units under player's control grows, these variables will be changed to arrays - partyPlayer and partyEnemy
     * And the code will be adjusted accordingly
     */
    public Enemy enemy;

    public MouseController player;

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
    }
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
            State = BattleState.ENEMY_TURN;

            // reset
            actionsPerformed = 0;
        } else if(State == BattleState.ENEMY_TURN)
        {           
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
            if(player.MoveTrigger())
            {
                actionsPerformed += 1;
            };
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
