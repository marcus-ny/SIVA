using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{
    private void Start()
    {
        pathFinder = new PathFinder();
        path = new();
        hitpoints = 100;
    }
    private void Update()
    {
        if (player == null)
        {
            player = EnemyManager.Instance.mc.character;
        }

        gameObject.GetComponent<SpriteRenderer>().sortingOrder =
            EnemyManager.Instance.mc.GetComponent<SpriteRenderer>().sortingOrder;

        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }
    public override void Action()
    {
        SoldierMove();
        return;
    }

    private void SoldierMove()
    {
        path = pathFinder.FindPath(activeTile, player.activeTile, new List<OverlayTile>());
    }

    public override void TakeDamage(float damage)
    {
        hitpoints -= damage;
    }
}
