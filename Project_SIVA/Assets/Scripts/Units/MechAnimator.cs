using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAnimator : MonoBehaviour
{
    public enum AnimationStatus { Powered, Disabled, Attack }
    public AnimationStatus currStatus;
    // Start is called before the first frame update
    string currAnimation;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        currAnimation = "BossIdle";
        currStatus = AnimationStatus.Powered;
    }

    // Update is called once per frame
    void Update()
    {
        if (currStatus == AnimationStatus.Powered)
        {
            currAnimation = "BossIdle";
        } else if (currStatus == AnimationStatus.Disabled)
        {
            currAnimation = "BossPowerDown";
        } else if (currStatus == AnimationStatus.Attack)
        {
            currAnimation = "BossPunch";
        }

        animator.Play(currAnimation);
    }


}
