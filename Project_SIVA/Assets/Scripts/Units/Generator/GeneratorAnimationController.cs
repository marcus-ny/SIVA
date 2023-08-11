using System.Collections;
using UnityEngine;

public class GeneratorAnimationController : MonoBehaviour
{
    Animator animator;
    string currAnimation;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currAnimation = "Generator_Hidden";
    }

    private void Update()
    {
        animator.Play(currAnimation);
    }

    IEnumerator waitForActivation()
    {
        yield return new WaitForSeconds(0.5f);
        currAnimation = "Generator_Active";
    }

    public void StartActive()
    {
        currAnimation = "Generator_Appear";
        StartCoroutine(waitForActivation());
    }

    public void StayActive()
    {
        currAnimation = "Generator_Active";
    }
}
