using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : WorldEntity, IInteractable
{
    public bool used;

    private void Start()
    {
        used = false;
    }
    public void Highlight(bool trigger)
    {
        // Add highlighting later
    }

    public bool ReceiveInteraction()
    {
        if (used) return false;
        else
        {
            BattleSimulator.Instance.DisableBoss();
            used = true;
            return true;
        }
    }
}
