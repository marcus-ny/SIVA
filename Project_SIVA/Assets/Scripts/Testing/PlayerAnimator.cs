using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly string[] shadow_directions = { "Player_move_NE", "Player_move_SW", "Player_move_NW", "Player_move_SE" };
    private static readonly string[] light_directions = { "Player_light_move_NE", "Player_light_move_SW", "Player_light_move_NW", "Player_light_move_SE" };

    public enum Status { WEAKENING, STRONGER, NIL, MELEEING }

    public Status status;
    private Animator animator;

    private string currAnimation;

    private CharacterInfo character;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currAnimation = "Player_still";
        character = gameObject.GetComponent<CharacterInfo>();
        status = Status.NIL;
    }

    // Update is called once per frame
    void Update()
    {
        if (status == Status.WEAKENING)
        {
            ShadowToLightTransition();
        }
        else if (status == Status.STRONGER)
        {
            LightToShadowTransition();
        }
        else if (status == Status.NIL)
        {
            MoveAnimation();
        }
        else if (status == Status.MELEEING)
        {
            MeleeAnimation();
        }

        animator.Play(currAnimation);
    }

    void MoveAnimation()
    {
        Vector3Int cur = character.cur;
        Vector3Int prev = character.prev;


        if (cur.x - prev.x > 0 && cur.y - prev.y == 0)
        {
            currAnimation = (character.activeTile.light_level > 0) ? light_directions[0] : shadow_directions[0];
        }
        else if (cur.x - prev.x < 0 && cur.y - prev.y == 0)
        {
            currAnimation = (character.activeTile.light_level > 0) ? light_directions[1] : shadow_directions[1];
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y > 0)
        {
            currAnimation = (character.activeTile.light_level > 0) ? light_directions[2] : shadow_directions[2];
        }
        else if (cur.x - prev.x == 0 && cur.y - prev.y < 0)
        {
            currAnimation = (character.activeTile.light_level > 0) ? light_directions[3] : shadow_directions[3];
        }
        else if (cur == prev)
        {
            currAnimation = (character.activeTile.light_level > 0) ? "Player_weak_still" : "Player_still";
        }
    }
    private void MeleeAnimation()
    {
        Vector3Int targetTile = PlayerController.Instance.destinationTile.gridLocation;
        Vector3Int playerTile = character.activeTile.gridLocation;
        string animation = "";

        if (targetTile.x - playerTile.x > 0 && targetTile.y - playerTile.y == 0)
        {
            // NE
            animation = "Player_melee_NE";
        }
        else if (targetTile.x - playerTile.x < 0 && targetTile.y - playerTile.y == 0)
        {
            // SW
            animation = "Player_melee_SW";
        }
        else if (targetTile.x - playerTile.x == 0 && targetTile.y - playerTile.y > 0)
        {
            // NW
            animation = "Player_melee_NW";
        }
        else if (targetTile.x - playerTile.x == 0 && targetTile.y - playerTile.y < 0)
        {
            // SE
            animation = "Player_melee_SE";
        }
        else if (targetTile.x - playerTile.x > 0 && targetTile.y - playerTile.y > 0)
        {
            // N
            animation = "Player_melee_N";
        }
        else if (targetTile.x - playerTile.x > 0 && targetTile.y - playerTile.y < 0)
        {
            // E
            animation = "Player_melee_E";
        }
        else if (targetTile.x - playerTile.x < 0 && targetTile.y - playerTile.y < 0)
        {
            // S
            animation = "Player_melee_S";
        }
        else if (targetTile.x - playerTile.x < 0 && targetTile.y - playerTile.y > 0)
        {
            // W
            animation = "Player_melee_W";
        }

        currAnimation = animation;
    }
    void LightToShadowTransition()
    {
        currAnimation = "Player_light_to_shadow";
    }

    void ShadowToLightTransition()
    {
        currAnimation = "Player_shadow_to_light";
    }


}
