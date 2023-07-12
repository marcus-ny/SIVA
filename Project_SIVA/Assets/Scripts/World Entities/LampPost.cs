using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPost : WorldEntity, IInteractable
{
    public bool active;
    private LampPostAnimationController animationController;

    private void Start()
    {
        EmitLight(activeTile, true);
        active = true;
        animationController = gameObject.GetComponent<LampPostAnimationController>();
    }

    public override void Highlight(bool trigger)
    {
        animationController.Highlight(trigger);
    }
    public bool ReceiveInteraction()
    {
        if (active)
        {
            EmitLight(activeTile, false);
        }
        else
        {
            EmitLight(activeTile, true);
        }

        return true;
    }

    public void EmitLight(OverlayTile curr, bool trigger)
    {
        // Trigger == true --> Emit light
        // Trigger == false --> Turn off light (meant for moving and dynamic
        // lighting)
        int factor = trigger ? 1 : -1;
        active = trigger ? true : false;
        List<OverlayTile> neighbors = MapController.Instance.Get3x3Grid(curr);

        foreach (var neighbor in neighbors)
        {
            if (!MapController.Instance.map.ContainsKey(new
                Vector2Int(neighbor.gridLocation.x, neighbor.gridLocation.y)) ||
                Mathf.Abs(curr.gridLocation.z - neighbor.gridLocation.z) > 1)
            {
                continue;
            }

            MapController.Instance.map[new Vector2Int(neighbor.gridLocation.x,
                neighbor.gridLocation.y)].AlterLightLevel(trigger);
        }
        curr.AlterLightLevel(trigger);
    }
}
