using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;

    public float hitpoints;

    private void Start()
    {
        hitpoints = 100;
    }
}
