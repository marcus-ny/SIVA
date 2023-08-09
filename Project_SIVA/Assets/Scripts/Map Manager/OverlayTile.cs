using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PathArrowGenerator;

public class OverlayTile : MonoBehaviour
{
    // G, H and F are values required for A* pathfinding algorithm
    public int G;
    public int H;
    public int F { get { return G + H; } }

    public bool isBlocked;

    public Enemy enemy;

    // Indicator for whether a tile is LIGHT or SHADOW
    public int light_level;

    public OverlayTile previous;

    public Vector3Int gridLocation;
    public Vector2Int gridLocation2d { get { return new Vector2Int(gridLocation.x, gridLocation.y);  } }

    public List<Sprite> pathDirs;


    private void Start()
    {
        light_level = 0;
        SetPathDir(ArrowDir.None);
    }

    public void AlterLightLevel(bool trigger)
    {
        int previousLight = light_level;
        int increment = trigger ? 1 : -1;
        light_level += increment;
        if (PlayerController.Instance.character.activeTile == this)
        {
            if (light_level == 0 && previousLight > 0)
            {
                PlayerController.Instance.TransitionLTS();
            } else if (light_level > 0 && previousLight == 0)
            {
                PlayerController.Instance.TransitionSTL();
            }
        }
    }

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
        SetPathDir(ArrowDir.None);
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

    public void ShowBlueTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 200, 255);
    }

    public void SetPathDir(ArrowDir d)
    {
        var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
        
        if (d == ArrowDir.None)
        {
            arrow.color = new Color(1, 1, 1, 0);
        } else
        {
            arrow.color = new Color(1, 1, 1, 1);
            arrow.sprite = pathDirs[(int)d];
            arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
