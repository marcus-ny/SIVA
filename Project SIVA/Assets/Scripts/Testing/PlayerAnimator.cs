using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly string[] directions = { "Player_move_NE", "Player_move_SW", "Player_move_NW", "Player_move_SE" };

    private Animator animator;

    private string currAnimation;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currAnimation = "Player_still";
    }

    // Update is called once per frame
    void Update()
    {
        GetAnimation();
        animator.Play(currAnimation);
    }

    void GetAnimation()
    {
        Vector3Int cur = gameObject.GetComponent<CharacterInfo>().cur;
        Vector3Int prev = gameObject.GetComponent<CharacterInfo>().prev;

        if (cur.x - prev.x > 0 && cur.y - prev.y == 0)
        {
            currAnimation = directions[0];
        }
        else if (cur.x - prev.x < 0 && cur.y - prev.y == 0)
        {
            currAnimation = directions[1];
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y > 0)
        {
            currAnimation = directions[2];
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y < 0)
        {
            currAnimation = directions[3];
        } else if (cur == prev)
        {
            currAnimation = "Player_still";
        }
    }
}
