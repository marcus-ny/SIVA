using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rangefinder
{
   public List<OverlayTile> GetReachableTiles(OverlayTile cur, int range)
    {
        int steps = 0;

        List<OverlayTile> reachableTiles = new List<OverlayTile>();
        reachableTiles.Add(cur);

        List<OverlayTile> previous = new List<OverlayTile>();
        previous.Add(cur);

        while (steps < range)
        {
            List<OverlayTile> neighbors = new();

            foreach (var tile in previous)
            {
                neighbors.AddRange(MapController.Instance.GetNeighborTiles(tile, new List<OverlayTile>()));
            }

            reachableTiles.AddRange(neighbors);
            previous = neighbors.Distinct().ToList();
            steps++;
        }

        return reachableTiles.Distinct().ToList();
    }
}