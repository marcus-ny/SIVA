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
}
