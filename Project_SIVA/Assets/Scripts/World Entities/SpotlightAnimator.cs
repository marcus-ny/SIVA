using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightAnimator : MonoBehaviour
{
    Animator animator;
    string currAnim;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    // Update is called once per frame
    public void AnimateSpin(Spotlight_SW.Directions cur)
    {
        if (cur == Spotlight_SW.Directions.NE)
        {
            animator.Play("Spotlight_Idle_NE");
        }else if (cur == Spotlight_SW.Directions.NW)
        {
            animator.Play("Spotlight_Idle_NW");
        }else if (cur == Spotlight_SW.Directions.SE)
        {
            animator.Play("Spotlight_Idle_SE");
        }else if (cur == Spotlight_SW.Directions.SW)
        {
            animator.Play("Spotlight_Idle_SW");
        }
    }

}
