using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBossAnimator : MonoBehaviour
{
    Animator animator;
    string currAnimation;

    public enum AnimationState { Idle, MeleeIn, MeleeInProgress, MeleeOut, TeleportIn, TeleportOut }
    public AnimationState currState;
    
    void Start()
    {
        currState = AnimationState.Idle;
        animator = GetComponent<Animator>();
        currAnimation = "BossIdle";
    }

    void Update()
    {
        switch(currState)
        {
            case AnimationState.Idle:
                currAnimation = "BossIdle";
                break;
            case AnimationState.MeleeIn:
                currAnimation = "BossEnterMelee";
                break;
            case AnimationState.MeleeInProgress:
                currAnimation = "BossInMelee";
                break;
            case AnimationState.MeleeOut:
                currAnimation = "BossExitMelee";
                break;
            case AnimationState.TeleportIn:
                currAnimation = "BossEnterTeleport";
                break;
            case AnimationState.TeleportOut:
                currAnimation = "BossExitTeleport";
                break;
        }
        animator.Play(currAnimation);
    }
}
