using UnityEngine;


public class SoldierAnimationController : MonoBehaviour
{
    Animator animator;
    string currAnimation;
    Soldier soldier;
    public AttackStatus attackStatus;
    public enum AttackStatus { MELEE, RANGE, NIL }

    private static readonly string[] move_directions = { "Soldier_Move_NE", "Soldier_Move_SW", "Soldier_Move_NW", "Soldier_Move_SE" };
    private static readonly string[] melee_directions = { "Soldier_Melee_NE", "Soldier_Melee_SW", "Soldier_Melee_NW", "Soldier_Melee_SE" };
    private static readonly string[] range_directions = { "Soldier_Range_NE", "Soldier_Range_SW", "Soldier_Range_NW", "Soldier_Range_SE" };

    private void Start()
    {
        attackStatus = AttackStatus.NIL;
        currAnimation = "Soldier_Idle";
        soldier = gameObject.GetComponent<Soldier>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (attackStatus == AttackStatus.NIL)
        {
            MoveAnimation();

        }
        else if (attackStatus == AttackStatus.MELEE)
        {
            MeleeAnimation();

        }
        else if (attackStatus == AttackStatus.RANGE)
        {
            RangeAnimation();
        }
        animator.Play(currAnimation);
    }

    private void MoveAnimation()
    {
        Vector3Int prev = soldier.prev;
        Vector3Int cur = soldier.cur;

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
            currAnimation = "Soldier_Idle";
        }

    }

    private void MeleeAnimation()
    {
        Vector3Int playerLocation = soldier.player.activeTile.gridLocation;
        Vector3Int soldierLocation = soldier.activeTile.gridLocation;
        string animation = "";

        if (playerLocation.x - soldierLocation.x > 0 && playerLocation.y - soldierLocation.y == 0)
        {
            animation = melee_directions[0];
        }
        else if (playerLocation.x - soldierLocation.x < 0 && playerLocation.y - soldierLocation.y == 0)
        {
            animation = melee_directions[1];
        }
        else if (playerLocation.x - soldierLocation.x == 0 && playerLocation.y - soldierLocation.y > 0)
        {
            animation = melee_directions[2];
        }
        else if (playerLocation.x - soldierLocation.x == 0 && playerLocation.y - soldierLocation.y < 0)
        {
            animation = melee_directions[3];
        }
        else if (playerLocation.x - soldierLocation.x != 0)
        {
            animation = melee_directions[1];
        }
        else if (playerLocation.y - soldierLocation.y != 0)
        {
            animation = melee_directions[0];
        }

        currAnimation = animation;
    }

    private void RangeAnimation()
    {
        Vector3Int playerLocation = soldier.player.activeTile.gridLocation;
        Vector3Int soldierLocation = soldier.activeTile.gridLocation;
        string animation = "";

        if (playerLocation.x - soldierLocation.x > 0 && playerLocation.y - soldierLocation.y == 0)
        {
            animation = range_directions[0];
        }
        else if (playerLocation.x - soldierLocation.x < 0 && playerLocation.y - soldierLocation.y == 0)
        {
            animation = range_directions[1];
        }
        else if (playerLocation.x - soldierLocation.x == 0 && playerLocation.y - soldierLocation.y > 0)
        {
            animation = range_directions[2];
        }
        else if (playerLocation.x - soldierLocation.x == 0 && playerLocation.y - soldierLocation.y < 0)
        {
            animation = range_directions[3];
        }

        currAnimation = animation;
    }

}
