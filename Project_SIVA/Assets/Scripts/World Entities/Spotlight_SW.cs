using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight_SW : WorldEntity, IInteractable
{
    private bool active;
    public enum Directions { NE = 0, SE = 1, SW = 2, NW = 3 }
    private Directions currDir;

    public void Start()
    {
        //EmitDirectionalLight(activeTile, true, 1);
        CheckDir();
        Debug.Log("Starting direction: " + currDir);
         
    }
    public bool ReceiveInteraction()
    {
        if (active) EmitDirectionalLight(activeTile, false, 1);
        else EmitDirectionalLight(activeTile, true, 1);

        return true;
    }

    public void Update()
    {
        
            CheckDir();
        
    }

    private void EmitDirectionalLight(OverlayTile cur, bool trigger, int dir)
    {
        int factor = trigger ? 1 : -1;
        active = trigger ? true : false;

        List<OverlayTile> litUpTiles = MapController.Instance.GetUniDirectional(activeTile, dir, 3);
        Debug.Log("Emitted light: " + trigger + " in direction " + (Directions)dir);
        Debug.Log("Tile count: " + litUpTiles.Count);
        foreach (OverlayTile tile in litUpTiles)
        {
            
            tile.AlterLightLevel(trigger);
        }
    }

    private void CheckDir()
    {
        Vector2Int playerPosition = PlayerController.Instance.character.activeTile.gridLocation2d;

        Vector2Int curPosition = activeTile.gridLocation2d;

        int xDiff = (playerPosition.x - curPosition.x);
        int yDiff = (playerPosition.y - curPosition.y);

        if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            
            // Emit light along X Axis
            
            if (xDiff > 0) // +X
            {
                // If directional difference is 2, clockwise/anticlockwise does not matter. Spin twice
                // If directional difference is 1, need to determine which direction
                Directions newDir = Directions.NE;
                int deltaDir = newDir - currDir;

                if (Mathf.Abs(deltaDir) == 2)
                {
                    RotateClockwise();
                    RotateClockwise();
                }

                else if (deltaDir == -3 || deltaDir == 1)
                {
                    RotateClockwise();
                }
                else if (deltaDir == -1 || deltaDir == 3)
                {
                    Debug.Log("Is this carried out?");
                    RotateCounterClockwise();
                }
            } else // -X
            {
                Directions newDir = Directions.SW;
                int deltaDir = newDir - currDir;

                if (Mathf.Abs(deltaDir) == 2)
                {
                    RotateClockwise();
                    RotateClockwise();
                }

                else if (deltaDir == -3 || deltaDir == 1)
                {
                    RotateClockwise();
                }
                else if (deltaDir == -1 || deltaDir == 3)
                {
                    RotateCounterClockwise();
                }
            }
        } else if (Mathf.Abs(yDiff) > Mathf.Abs(xDiff))
        {
            
            if (yDiff > 0)
            {
                
                
                Directions newDir = Directions.NW;
                int deltaDir = newDir - currDir;

                if (Mathf.Abs(deltaDir) == 2)
                {
                    Debug.Log("why is this path taken");
                    Debug.Log("currdir" + currDir);
                    Debug.Log("new dir: " + newDir);
                    RotateClockwise();
                    RotateClockwise();
                }

                else if (deltaDir == -3 || deltaDir == 1)
                {
                    RotateClockwise();
                }
                else if (deltaDir == -1 || deltaDir == 3)
                {
                    RotateCounterClockwise();
                }
            } else if (yDiff < 0)
            {
                
                Directions newDir = Directions.SE;
                int deltaDir = newDir - currDir;
                
                if (Mathf.Abs(deltaDir) == 2)
                {
                    Debug.Log("directional change 180 degrees from " + currDir + " to " + newDir);
                    RotateClockwise();
                    RotateClockwise();
                }

                else if (deltaDir == -3 || deltaDir == 1)
                {
                    RotateClockwise();
                }
                else if (deltaDir == -1 || deltaDir == 3)
                {
                    RotateCounterClockwise();
                }
            }
        }
        
    }

    private void RotateClockwise()
    {
        Directions prevDir = currDir;
        Directions newDir = (Directions) ((int)(currDir + 1)%4);
        currDir = newDir;
        Debug.Log("Rotated Clockwise to from " + prevDir + " to " + newDir);
        EmitDirectionalLight(activeTile, false, (int)prevDir);
        EmitDirectionalLight(activeTile, true, (int)newDir);
    }

    private void RotateCounterClockwise()
    {
        // public enum Directions { NE = 0, SE = 1, SW = 2, NW = 3 }
        Directions prevDir = currDir;
        
        int absoluteDiffDir = (int)currDir - 1;
        while ((int)absoluteDiffDir < 0) absoluteDiffDir += 4;
        Directions newDir = (Directions)(absoluteDiffDir);
        currDir = newDir;
        Debug.Log("Rotated CounterClockwise from " + prevDir + " to " + newDir);
        EmitDirectionalLight(activeTile, false, (int)prevDir);
        EmitDirectionalLight(activeTile, true, (int)newDir);
    }

    public override void Highlight(bool trigger)
    {
        
    }
}