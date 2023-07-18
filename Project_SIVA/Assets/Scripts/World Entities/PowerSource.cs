using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : WorldEntity, IInteractable
{
    public void Highlight(bool trigger)
    {
        // Add highlighting later
    }

    public bool ReceiveInteraction()
    {
        BattleSimulator.Instance.DisableBoss();
        return true;
    }
}
