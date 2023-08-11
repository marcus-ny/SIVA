using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.Play("DoorIdle");
    }

    public void OpenDoor()
    {
        animator.Play("DoorOpen");
    }
}
