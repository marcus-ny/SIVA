using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimationController : MonoBehaviour
{
    Animator animator;
    string currAnimation;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currAnimation = "DroneIdle";
    }

    private void Update()
    {
        animator.Play(currAnimation);
    }
}
