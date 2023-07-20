using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightRight : Enemy, IObserver
{

    bool active;
    int curPos;
    int turnsSincedisabled;
    bool onReturnTrip;
    private void Start()
    {
        turnsSincedisabled = 0;
        BattleSimulator.Instance.AddObserver(this);
        pathFinder = new PathFinder();
        path = new();
        rangeFinder = new Rangefinder();
        range = new();
        active = true;
        curPos = 0;
        onReturnTrip = false;
        maxHp = 999;
        maxAP = 10;
        EmitDirectionalLight(activeTile, true, 2);
    }
    public override void Action()
    {
        if (active)
        {
            Patrol();
        } else if (!active && turnsSincedisabled >= 2)
        {
            active = true;
            Patrol();
        } else
        {
            turnsSincedisabled += 1;
        }
    }

    private void Patrol()
    {
        EmitDirectionalLight(activeTile, false, 2);
        path = pathFinder.FindPath(activeTile, GetNextTile(), MapController.Instance.Get3x3Grid(activeTile), 1);
        if (onReturnTrip)
        {
            curPos -= 1;
        }
        else
        {
            curPos += 1;
        }
        StartCoroutine(PatrolPath());
    }

    IEnumerator PatrolPath()
    {
        while (state_moving)
        {
            yield return null;
        }
        state_moving = true;

        while (path.Count > 0)
        {

            activeTile.isBlocked = false;
            EnemyManager.Instance.enemyMap.Remove(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y));
            activeTile.enemy = null;
            // Before

            var step = SPEED * Time.deltaTime;
            var zIndex = path[0].transform.position.z;

            cur = path[0].gridLocation;
            prev = path[0].previous.gridLocation;

            transform.position = Vector2.MoveTowards(transform.position,
                path[0].transform.position, step);

            transform.position = new Vector3(transform.position.x,
                transform.position.y, zIndex);

            if (Vector2.Distance(transform.position, path[0].transform.position) < 0.0001f)
            {
                PositionEnemyOnTile(path[0]);
                path.RemoveAt(0);
            }

            activeTile.enemy = this;
            activeTile.isBlocked = true;
            EnemyManager.Instance.enemyMap.Add(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y), this);
            yield return null;

        }

        if (path.Count == 0)
        {
            prev = cur = activeTile.gridLocation;
        }
        EmitDirectionalLight(activeTile, true, 2);
        state_moving = false;

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

    private void EmitDirectionalLight(OverlayTile cur, bool trigger, int dir)
    {
        int factor = trigger ? 1 : -1;

        List<OverlayTile> litUpTiles = MapController.Instance.GetUniDirectional(activeTile, dir, 3);

        foreach (OverlayTile tile in litUpTiles)
        {
            tile.AlterLightLevel(trigger);
        }

    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.BossPowerDisabled)
        {
            active = false;
            EmitDirectionalLight(activeTile, false, 2);
        }
    }
}
