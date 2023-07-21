using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ButtonController : MonoBehaviour
{
    public Canvas UI;

    

    public bool truesight = false;

    /*
     * Function for switching turns between player and AI party
     */
    public void SwitchTurn()
    {
        
        BattleSimulator.Instance.switchTurns();      
    }

    /*
     * Function for players to be able to see which tiles are shadow and light
     * tied to the button "Toggle sight"
     */
    
    

    /*
     * On clicking "Move" button in the UI
     * the units in this current turn will move
     */
    public void Move()
    {
        BattleSimulator.Instance.MoveUnit();
    }

    // Tied to Attack button in UI
    public void Melee()
    {
        BattleSimulator.Instance.DealMeleeDamage();
    }

    public void Fireball()
    {
        BattleSimulator.Instance.CastFireball();
    }

    public void AOE()
    {
        BattleSimulator.Instance.DealAOEDamage();
    }
    
    // Tied to Interact button in UI
    public void Interact()
    {
        BattleSimulator.Instance.InteractItem();
    }
}
