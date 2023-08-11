using System.Collections.Generic;
using UnityEngine;

public class Spotlight_SW : WorldEntity, IInteractable
{
    private bool active;
    public enum Directions { NE = 0, SE = 1, SW = 2, NW = 3 }
    private Directions currDir;
    private SpotlightAnimator animator;

    public void Awake()
    {
        animator = GetComponent<SpotlightAnimator>();
    }
    public void Start()
    {
        EmitDirectionalLight(activeTile, true, (int)currDir);
        CheckDir();

    }
    public bool ReceiveInteraction()
    {
        if (active) EmitDirectionalLight(activeTile, false, (int)currDir);
        else
        {
            EmitDirectionalLight(activeTile, true, (int)currDir);
            CheckDir();
        }

        return true;
    }

    public void Update()
    {
        // Temporary fix, but can be improved later

        animator.AnimateSpin(currDir);

        if (BattleSimulator.Instance.State != BattleState.PLAYER_TURN)
        {
            if (active)
            {
                CheckDir();
            }
        }

    }

    private void EmitDirectionalLight(OverlayTile cur, bool trigger, int dir)
    {
        int factor = trigger ? 1 : -1;
        active = trigger;

        List<OverlayTile> litUpTiles = MapController.Instance.GetUniDirectional(activeTile, dir, 3);

        foreach (OverlayTile tile in litUpTiles)
        {
            tile.AlterLightLevel(trigger);
        }
        animator.AnimateSpin((Directions)dir);
    }

    private void CheckDir()
    {
        Vector2Int playerPosition = PlayerController.Instance.character.activeTile.gridLocation2d;

        Vector2Int curPosition = activeTile.gridLocation2d;

        int xDiff = (playerPosition.x - curPosition.x);
        int yDiff = (playerPosition.y - curPosition.y);

        if (Mathf.Abs(xDiff) >= Mathf.Abs(yDiff))
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
                    RotateCounterClockwise();
                }
            }
            else // -X
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
        }
        else if (Mathf.Abs(yDiff) > Mathf.Abs(xDiff))
        {

            if (yDiff > 0)
            {
                Directions newDir = Directions.NW;
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
            else if (yDiff < 0)
            {

                Directions newDir = Directions.SE;
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
        }

    }

    private void RotateClockwise()
    {
        Directions prevDir = currDir;
        Directions newDir = (Directions)((int)(currDir + 1) % 4);
        currDir = newDir;
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
        EmitDirectionalLight(activeTile, false, (int)prevDir);
        EmitDirectionalLight(activeTile, true, (int)newDir);
    }

    public void Highlight(bool trigger)
    {

    }
}
