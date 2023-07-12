using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Door opened");
        animator.Play("DoorOpen");
    }
}
