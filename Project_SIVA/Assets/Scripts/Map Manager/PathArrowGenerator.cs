using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArrowGenerator
{
    public enum ArrowDir
    {
        None = 0,
        NE = 1,
        SW = 2,
        NW = 3,
        SE = 4,
        // All directions are respective to positive X
        RightBtm = 5, // SW to SE
        LeftBtm = 6, // SW to NW
        RightTop = 7, // SE to NE
        LeftTop = 8, // SW to NE
        NEend = 9,
        SWend = 10,
        NWend = 11,
        SEend = 12
    }

    public ArrowDir Translate(OverlayTile prev, OverlayTile cur, OverlayTile next)
    {
        bool isEnd = (next == null);

        Vector2Int prevDir = prev != null ? cur.gridLocation2d - prev.gridLocation2d : new Vector2Int(0, 0);
        Vector2Int nextDir = next != null ? next.gridLocation2d - cur.gridLocation2d : new Vector2Int(0, 0);
        Vector2Int dir = prevDir != nextDir ? prevDir + nextDir : nextDir;
        // Positive Y
        if (dir == new Vector2Int(0, 1))
        {
            return !isEnd ? ArrowDir.NW : ArrowDir.NWend;
        }
        // Negative Y
        if (dir == new Vector2Int(0, -1))
        {
            return !isEnd ? ArrowDir.SE : ArrowDir.SEend;
        }
        // Positive X
        if (dir == new Vector2Int(1, 0))
        {
            return !isEnd ? ArrowDir.NE : ArrowDir.NEend;
        }
        // Negative X
        if (dir == new Vector2Int(-1, 0))
        {
            return !isEnd ? ArrowDir.SW : ArrowDir.SWend;
        }

        if (dir == new Vector2Int(1, 1))
        {
            if (prevDir.y < nextDir.y)
            {
                return ArrowDir.LeftBtm;
            } 
            else
                return ArrowDir.RightTop;
        }

        if (dir == new Vector2Int(1, -1))
        {
            if (prevDir.y < nextDir.y)
            {
                return ArrowDir.LeftTop;
            }
            else
                return ArrowDir.RightBtm;
        }

        if (dir == new Vector2Int(-1, 1))
        {
            if (prevDir.y < nextDir.y)
            {
                return ArrowDir.LeftTop;
            }
            else
                return ArrowDir.RightBtm;
        }

        if (dir == new Vector2Int(-1, -1))
        {

            if (prevDir.y < nextDir.y)
            {
                return ArrowDir.LeftBtm;
            }
            else
                return ArrowDir.RightTop;
        }

        return ArrowDir.None;
    }
}
