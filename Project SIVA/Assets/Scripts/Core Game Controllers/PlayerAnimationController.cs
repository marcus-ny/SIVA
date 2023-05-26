using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public MouseController mc;
    public Animator animator;
    // Start is called before the first frame update
    void Update()
    {
        animator = mc.characterPrefab.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public void AnimatePlayer() 
    {
        List<OverlayTile> path = mc.path;

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
