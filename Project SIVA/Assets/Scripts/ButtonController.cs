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
    public void SwitchTurn()
    {
        print("switching turns");
        battleSim.switchTurns();      
    }

    /*
     * Function for players to be able to see which tiles are shadow and light, possibly tied to a button
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

    public void Move()
    {
        battleSim.MoveUnit();
    }
    
}
