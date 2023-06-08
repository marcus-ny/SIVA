using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
    // Start is called before the first frame update
    void Update()
    {
        animator = playerController.character_prefab.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public void AnimatePlayer() 
    {
        List<OverlayTile> path = playerController.character.path;

        Vector3Int prev = path[0].previous.gridLocation;
        Vector3Int cur = path[0].gridLocation;

        if (cur.x - prev.x > 0 && cur.y - prev.y == 0)
        {
            print("Moving NE");
            animator.Play("Player_move_NE");
        }
        else if (cur.x - prev.x < 0 && cur.y - prev.y == 0)
        {
            print("Moving SW");
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y > 0)
        {
            print("Moving NW");
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y < 0)
        {
            print("Moving SE");
        }
    }
}
