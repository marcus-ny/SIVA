using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightRight : Enemy
{
    int minPos = 0;
    int maxPos = 3;

    int curPos;

    bool onReturnTrip;
    private void Start()
    {
        pathFinder = new PathFinder();
        path = new();
        rangeFinder = new Rangefinder();
        range = new();

        curPos = 0;
        onReturnTrip = false;
        maxHp = 999;
        maxAP = 10;

    }
    public override void Action()
    {
        Patrol();
    }

    private void Patrol()
    {
        path = pathFinder.FindPath(activeTile, GetNextTile(), MapController.Instance.Get3x3Grid(activeTile), 1);
        if (onReturnTrip)
        {
            curPos -= 1;
        }
        else
        {
            curPos += 1;
        }
        StartCoroutine(MoveAlongPath());
    }

    private OverlayTile GetNextTile()
    {
        if (curPos == 3) onReturnTrip = true;
        else if (curPos == 0) onReturnTrip = false;

        if (!onReturnTrip)
        {
            Vector2Int nextCoordinate = new(activeTile.gridLocation.x, activeTile.gridLocation.y - 1);
            OverlayTile res = MapController.Instance.map[nextCoordinate];
            return res;
        }
        else
        {
            Vector2Int nextCoordinate = new(activeTile.gridLocation.x, activeTile.gridLocation.y + 1);
            OverlayTile res = MapController.Instance.map[nextCoordinate];
            return res;
        }
    }
}
