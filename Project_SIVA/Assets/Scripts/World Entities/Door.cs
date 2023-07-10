using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : WorldEntity, IInteractable, ITeleportable
{
    
    public override void Highlight(bool trigger)
    {
        // highlight lmao
    }

    public void ReceiveInteraction()
    {
        Teleport();
    }

    public void Teleport()
    {
        // Temporary
        ScenesManager.Instance.LoadNextScene();
    }
}
