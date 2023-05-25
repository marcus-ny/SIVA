using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class stores all information about an enemy unit
 * Current holding information: HP, currentTile
 */

public class EnemyInfo : MonoBehaviour
{
    public OverlayTile activeTile;

    public int hitpoints;

    private void Start()
    {
        hitpoints = 100;
    }
}
