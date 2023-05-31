using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public OverlayTile activeTile;
    public GameObject enemyPrefab;

    protected CharacterInfo player;

    protected PathFinder pathFinder;
    protected Rangefinder rangeFinder;

    protected List<OverlayTile> path;
    protected List<OverlayTile> range;

    public float hitpoints;

    public int maxAP;
    public int actionsPerformed;

    protected readonly static int SPEED = 3;

    public abstract void Action();

    public abstract void TakeDamage(float damage);

    public void MoveAlongPath()
    {
        activeTile.isBlocked = false;
        EnemyManager.Instance.enemyMap.Remove(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y));
        activeTile.enemy = null;

        var step = SPEED * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position,
            path[0].transform.position, step);

        transform.position = new Vector3(transform.position.x,
            transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionEnemyOnTile(path[0]);
            path.RemoveAt(0);
        }
        
        activeTile.enemy = this;
        activeTile.isBlocked = true;
        EnemyManager.Instance.enemyMap.Add(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y), this);
    }

    public void PositionEnemyOnTile(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y,
            tile.transform.position.z);

        GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;

        activeTile = tile;
        activeTile.enemy = this;
        activeTile.isBlocked = true;
    }

    // Damage Visuals
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
    public void SwitchColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
        StartCoroutine("Delay");
    }
}
