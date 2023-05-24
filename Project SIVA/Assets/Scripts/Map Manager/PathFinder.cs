using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*  A* pathfinding algorithm
 *  Used for player and enemy movement
 */
public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            OverlayTile curr = openList.OrderBy(i => i.F).First();

            openList.Remove(curr);
            closedList.Add(curr);

            if(curr == end)
            {
                // end path
                return getFinishedList(start, end);
            }

            // Get neighbor tiles
            var neighborTiles = MapController.Instance.GetNeighborTiles(curr);

            foreach (var neighbor in neighborTiles)
            {
                if (neighbor.isBlocked || closedList.Contains(neighbor) ||
                    Mathf.Abs(curr.gridLocation.z - neighbor.gridLocation.z) > 1 )
                {
                    continue;
                }

                neighbor.G = GetManhattenDistance(start, neighbor);
                neighbor.H = GetManhattenDistance(end, neighbor);

                neighbor.previous = curr;

                if(!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> getFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();

        OverlayTile curr = end;

        while (curr != start)
        {
            finishedList.Add(curr);
            curr = curr.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbor)
    {
        return Mathf.Abs(start.gridLocation.x - neighbor.gridLocation.x)
            + Mathf.Abs(start.gridLocation.y - neighbor.gridLocation.y);
    }

    
}
