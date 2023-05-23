using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, END }
public class BattleSimulator : MonoBehaviour
{
    public BattleState State; // Change to private later

    // Start is called before the first frame update
    void Start()
    {
        State = BattleState.PLAYER_TURN;
    }

    
    public void switchTurns()
    {
        if(State == BattleState.PLAYER_TURN)
        {
            print("It is now enemy's turn");
            State = BattleState.ENEMY_TURN;
        } else if(State == BattleState.ENEMY_TURN)
        {
            print("It is now player's turn");
            State = BattleState.PLAYER_TURN;
            DamageManager.Instance.tickDamage();
        }
    }
}
