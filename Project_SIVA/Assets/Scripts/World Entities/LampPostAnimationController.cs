using UnityEngine;

public class LampPostAnimationController : MonoBehaviour
{
    Animator animator;
    string currAnimation;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currAnimation = "Lamppost_Idle";
    }

    private void Update()
    {
        animator.Play(currAnimation);
    }
    public void Highlight(bool trigger)
    {
        currAnimation = trigger ? "Lamppost_Highlight" : "Lamppost_Idle";
    }
}
