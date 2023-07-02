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
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> validTilePool, int jumpHeight)
    {
        List<OverlayTile> openList = new();
        List<OverlayTile> closedList = new();

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
            //var neighborTiles = MapController.Instance.GetNeighborTiles(curr, validTilePool, jumpHeight);
            var neighborTiles = GetNeighborTiles(curr, validTilePool, jumpHeight);

            foreach (var neighbor in neighborTiles)
            {
                if (neighbor.isBlocked || closedList.Contains(neighbor))
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

    public int GetManhattenDistance(OverlayTile start, OverlayTile neighbor)
    {
        return Mathf.Abs(start.gridLocation.x - neighbor.gridLocation.x)
            + Mathf.Abs(start.gridLocation.y - neighbor.gridLocation.y);
    }

    /*
     * Given a target tile, return an array of tiles in a given range
     * sorted by order of closest to furthest to the target
     */
    public List<OverlayTile> GetClosestTilesInRange(OverlayTile target, List<OverlayTile> range)
    {
        // int lowest = int.MaxValue;
        // careful
        // OverlayTile nearest = null;

        List<OverlayTile> nearestTiles = new();
        foreach (OverlayTile tile in range)
        {
            if (tile.isBlocked)
            {
                continue;
            }
            else if (target == tile)
            {
                // If pathfinding breaks, check here
                nearestTiles.Clear();
                nearestTiles.Add(tile);
                return nearestTiles;
            }
            nearestTiles.Add(tile);
        }

        nearestTiles.Sort((x, y) => GetManhattenDistance(target, x).CompareTo(GetManhattenDistance(target, y)));
        return nearestTiles;
    }

    public List<OverlayTile> GetTilesBetweenInStraightLine(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> result = new();

        if (Mathf.Abs(end.gridLocation.x - start.gridLocation.x) > 1)
        {
            int i = (end.gridLocation.x > start.gridLocation.x) ? start.gridLocation.x : end.gridLocation.x;
            int limit = (end.gridLocation.x > start.gridLocation.x) ? end.gridLocation.x : start.gridLocation.x;
            for (; i < limit; i++)
            {
                if (MapController.Instance.map.ContainsKey(new Vector2Int(i, start.gridLocation.y)))
                {
                    result.Add(MapController.Instance.map[new Vector2Int(i, start.gridLocation.y)]);
                }
            }
        } else
        {
            int i = (end.gridLocation.y > start.gridLocation.y) ? start.gridLocation.y : end.gridLocation.y;
            int limit = (end.gridLocation.y > start.gridLocation.y) ? end.gridLocation.y : start.gridLocation.y;
            for (; i < limit; i++)
            {
                if (MapController.Instance.map.ContainsKey(new Vector2Int(i, start.gridLocation.y)))
                {
                    result.Add(MapController.Instance.map[new Vector2Int(start.gridLocation.x, i)]);
                }
            }
        }

        return result;
    }

    public List<OverlayTile> GetNeighborTiles(OverlayTile curr, List<OverlayTile> validTiles, int jumpHeight)
    {


        Dictionary<Vector2Int, OverlayTile> searchRange = new();

        if (validTiles.Count > 0)
        {
            foreach (var tile in validTiles)
            {
                searchRange.Add(new Vector2Int(tile.gridLocation.x, tile.gridLocation.y), tile);
            }
        }
        else
        {
            searchRange = MapController.Instance.map;
        }


        List<OverlayTile> neighbors = new();

        int[,] directions = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };

        for (int i = 0; i < 4; i++)
        {
            Vector2Int locationToCheck = new(curr.gridLocation.x + directions[i, 0],
            curr.gridLocation.y + directions[i, 1]);

            if (searchRange.ContainsKey(locationToCheck) && Mathf.Abs(curr.gridLocation.z - searchRange[locationToCheck].gridLocation.z) <= jumpHeight)
            {
                neighbors.Add(searchRange[locationToCheck]);
            }
        }

        return neighbors;
    }
}
