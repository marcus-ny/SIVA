using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicAnimationController : MonoBehaviour
{
    Animator animator;
    string currAnimation;
    Mechanic mechanic;

    public enum Status { HEALING, NIL }
    public Status status;

    private static readonly string[] move_directions = { "Mechanic_Move_NE", "Mechanic_Move_SW", "Mechanic_Move_NW", "Mechanic_Move_SE" };
    private static readonly string[] heal_directions = { "Mechanic_Fix_NE", "Mechanic_Fix_SW", "Mechanic_Fix_NW", "Mechanic_Fix_SE" };

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        mechanic = gameObject.GetComponent<Mechanic>();
        currAnimation = "Mechanic_Idle";
        status = Status.NIL;
    }

    private void Update()
    {
        if (status == Status.NIL)
        {
            MoveAnimation();
        } else if (status == Status.HEALING)
        {
            HealAnimation();
        }
        animator.Play(currAnimation);
    }

    private void MoveAnimation()
    {
        Vector3Int prev = mechanic.prev;
        Vector3Int cur = mechanic.cur;

        if (cur.x - prev.x > 0 && cur.y - prev.y == 0)
        {
            currAnimation = move_directions[0];
        }
        else if (cur.x - prev.x < 0 && cur.y - prev.y == 0)
        {
            currAnimation = move_directions[1];
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y > 0)
        {
            currAnimation = move_directions[2];
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y < 0)
        {
            currAnimation = move_directions[3];
        }
        else if (cur == prev)
        {
            currAnimation = "Mechanic_Idle";
        }

    }

    private void HealAnimation()
    {
        Vector3Int playerLocation = mechanic.allyLowHp.activeTile.gridLocation;
        Vector3Int soldierLocation = mechanic.activeTile.gridLocation;
        string animation = "";

        if (playerLocation.x - soldierLocation.x > 0 && playerLocation.y - soldierLocation.y == 0)
        {
            animation = heal_directions[0];
        }
        else if (playerLocation.x - soldierLocation.x < 0 && playerLocation.y - soldierLocation.y == 0)
        {
            animation = heal_directions[1];
        }
        else if (playerLocation.x - soldierLocation.x == 0 && playerLocation.y - soldierLocation.y > 0)
        {
            animation = heal_directions[2];
        }
        else if (playerLocation.x - soldierLocation.x == 0 && playerLocation.y - soldierLocation.y < 0)
        {
            animation = heal_directions[3];
        }

        currAnimation = animation;
    }

}
