using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPost : Interactable
{
    private void Start()
    {
        EmitLight(activeTile, true);
    }
    public void EmitLight(OverlayTile curr, bool trigger)
    {
        // Trigger == true --> Emit light
        // Trigger == false --> Turn off light (meant for moving and dynamic
        // lighting)
        int factor = trigger ? 1 : -1;

        List<OverlayTile> neighbors = MapController.Instance.Get3x3Grid(curr);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.isBlocked || !MapController.Instance.map.ContainsKey(new
                Vector2Int(neighbor.gridLocation.x, neighbor.gridLocation.y)) ||
                Mathf.Abs(curr.gridLocation.z - neighbor.gridLocation.z) > 1)
            {
                continue;
            }

            MapController.Instance.map[new Vector2Int(neighbor.gridLocation.x,
                neighbor.gridLocation.y)].light_level += factor;
        }
        curr.light_level += 1;
    }
}