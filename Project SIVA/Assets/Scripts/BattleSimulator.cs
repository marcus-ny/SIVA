using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, END }
public class BattleSimulator : MonoBehaviour
{
    public BattleState state; // Change to private later

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.PLAYER_TURN;
    }

    
    public void switchTurns()
    {
        if(state == BattleState.PLAYER_TURN)
        {
            print("It is now enemy's turn");
            state = BattleState.ENEMY_TURN;
        } else if(state == BattleState.ENEMY_TURN)
        {
            print("It is now player's turn");
            state = BattleState.PLAYER_TURN;
            DamageManager.Instance.tickDamage();
        }
    }
}
