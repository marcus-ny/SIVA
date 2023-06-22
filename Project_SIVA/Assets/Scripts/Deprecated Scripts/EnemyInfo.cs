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

    public float hitpoints;

    private void Start()
    {
        hitpoints = 100;
    }

    IEnumerator delay()
    { 
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
    public void SwitchColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
        StartCoroutine("delay");
    }
}
