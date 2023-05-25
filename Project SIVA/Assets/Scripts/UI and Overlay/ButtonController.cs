using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ButtonController : MonoBehaviour
{
    public BattleSimulator battleSim;

    public Canvas UI;

    public GameObject overlayContainer;

    public bool truesight = false;

    /*
     * Function for switching turns between player and AI party
     */
    public void SwitchTurn()
    { 
        battleSim.switchTurns();      
    }

    /*
     * Function for players to be able to see which tiles are shadow and light
     * tied to the button "Toggle sight"
     */
    public void TrueSight()
    {
        if (!truesight)
        {
            foreach (Transform item in overlayContainer.transform)
            {
                item.GetComponent<OverlayTile>().ShowTile();
            }
            truesight = true;
        } else
        {
            foreach (Transform item in overlayContainer.transform)
            {
                item.GetComponent<OverlayTile>().HideTile();
            }
            truesight = false;
        }
    }

    /*
     * On clicking "Move" button in the UI
     * the units in this current turn will move
     */
    public void Move()
    {
        battleSim.MoveUnit();
    }

    public void Attack()
    {
        battleSim.DealDamage();
    }
    
}