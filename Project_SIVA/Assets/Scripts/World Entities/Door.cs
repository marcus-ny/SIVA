using System.Collections;
using UnityEngine;

public class Door : WorldEntity, ITeleportable
{
    private DoorAnimator doorAnimator;

    private void Start()
    {
        doorAnimator = GetComponent<DoorAnimator>();
        activeTile.isBlocked = false;
    }


    private void Update()
    {
        if (PlayerController.Instance.character.activeTile == activeTile && BattleSimulator.Instance.levelComplete)
        {
            Teleport();
        }
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
