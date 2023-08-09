using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * No longer used, can be safely deleted
 */
public class InvisibleTrigger : WorldEntity
{
    public bool detected;
    

    // Start is called before the first frame update
    void Start()
    {
        // set to invisible so that players can't see it, but level editors can place easily
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        detected = false;
        activeTile.isBlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.character.activeTile == activeTile)
        {
            detected = true;
        } else
        {
            detected = false;
        }
    }
}
