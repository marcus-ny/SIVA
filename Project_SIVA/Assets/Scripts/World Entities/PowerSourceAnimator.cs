using UnityEngine;

public class PowerSourceAnimator : MonoBehaviour
{
    Animator animator;
    string currAnimation;
    PowerSource powerSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        powerSource = GetComponent<PowerSource>();
        currAnimation = "BossPowerSourceIdle";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("CurrAnimation" + currAnimation);
        if (!powerSource.used) currAnimation = "BossPowerSourceIdle";
        else currAnimation = "BossPowerSourceOff";
        animator.Play(currAnimation);
    }
}
