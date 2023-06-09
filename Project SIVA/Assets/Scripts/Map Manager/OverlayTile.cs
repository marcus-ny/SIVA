using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverlayTile : MonoBehaviour
{

    // G, H and F are values required for A* pathfinding algorithm
    public int G;
    public int H;
    public int F { get { return G + H; } }

    // Indicator for whether this tile cannot be moved through
    // Currently have no uses, but will be used in implementing smoother and
    // more realistic pathfinding later on
    public bool isBlocked;

    public Enemy enemy;

    // Indicator for whether a tile is LIGHT or SHADOW
    public int light_level;

    public OverlayTile previous;

    public Vector3Int gridLocation;

    private void Start()
    {
        light_level = 0;
    }

    // This method removse the range highlighting, therefore is temporarily disabled
    // Might be removed in the future.

    /*void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();           
        }
    }*/

    // Show light/dark tiles
    public void ShowTile()
    {
        if (light_level > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
    }

    // Return to original color without highlighting
    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    // Show green
    public void ShowGreenTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
    }
}
