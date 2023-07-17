using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : WorldEntity, IInteractable, ITeleportable
{
    private DoorAnimator doorAnimator;

    private void Start()
    {
        doorAnimator = GetComponent<DoorAnimator>();
        activeTile.isBlocked = false;
    }
    public override void Highlight(bool trigger)
    {
        
    }

    public bool ReceiveInteraction()
    {
        if (BattleSimulator.Instance.levelComplete)
        {
            Teleport();
            return true;
        }
        return false;
    }
    public void Teleport()
    {
        StartCoroutine(TeleportCoroutine());
    }
    IEnumerator TeleportCoroutine()
    {
        doorAnimator.OpenDoor();
        yield return new WaitForSecondsRealtime(1);
        ScenesManager.Instance.LoadNextScene();
        
    }
}
