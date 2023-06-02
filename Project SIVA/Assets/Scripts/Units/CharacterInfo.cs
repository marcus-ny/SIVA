using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;

    public float hitpoints;

    private Animator animator;

    
    private void Start()
    {
        hitpoints = 100;
        animator = GetComponent<Animator>();
    }

    public void AnimatePlayer(string direction)
    {
        if (animator != null)
        {
            animator.Play(direction);
        }
    }

    IEnumerator DamageVisual()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
    public void DisplayDamageVisual()
    {       
        StartCoroutine("DamageVisual");
    }
}
