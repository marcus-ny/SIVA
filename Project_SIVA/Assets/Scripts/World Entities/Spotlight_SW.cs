using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight_SW : WorldEntity, IInteractable
{
    private bool active;

    public void Start()
    {
        EmitDirectionalLight(activeTile, true);
    }
    public void ReceiveInteraction()
    {
        if (active) EmitDirectionalLight(activeTile, false);
        else EmitDirectionalLight(activeTile, true);
    }

    public void EmitDirectionalLight(OverlayTile cur, bool trigger)
    {
        int factor = trigger ? 1 : -1;
        active = trigger ? true : false;

        List<OverlayTile> litUpTiles = MapController.Instance.GetUniDirectional(activeTile, 1, 3);

        foreach (OverlayTile tile in litUpTiles)
        {
            tile.AlterLightLevel(trigger);
        }
    }

    public override void Highlight(bool trigger)
    {
        
    }
}
