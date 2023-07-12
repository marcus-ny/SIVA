using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public OverlayTile activeTile;

    public CharacterInfo player;

    protected PathFinder pathFinder;
    protected Rangefinder rangeFinder;

    public List<OverlayTile> path;
    public List<OverlayTile> range;

    public Vector3Int cur;
    public Vector3Int prev;

    public float maxHp;
    public float hitpoints;
    public float hpRatio { get { return (hitpoints / maxHp); } }

    public int maxAP;
    public int actionsPerformed;

    public bool state_moving;

    protected readonly static int SPEED = 2;

    public abstract void Action();

    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
    }

    protected IEnumerator MoveAlongPath()
    {
        while (state_moving)
        {
            yield return null;
        }
        state_moving = true;
        
        while (path.Count > 0)
        {
            
            activeTile.isBlocked = false;
            EnemyManager.Instance.enemyMap.Remove(new Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y));
            activeTile.enemy = null;
            // Before

            var step = SPEED * Time.deltaTime;
            var zIndex = path[0].transform.position.z;

            cur = path[0].gridLocation;
            prev = path[0].previous.gridLocation;

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
            yield return null;
            
        }
        
        if (path.Count == 0)
        {
            prev = cur = activeTile.gridLocation;
        }
        
        state_moving = false;

    }

    public virtual void PositionEnemyOnTile(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y,
            tile.transform.position.z);

        GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;

        activeTile = tile;
        activeTile.enemy = this;
        activeTile.isBlocked = true;
    }
    
    public virtual void TriggerDeath()
    {
        EnemyManager.Instance.enemyMap.Remove(new
            Vector2Int(activeTile.gridLocation.x, activeTile.gridLocation.y));
        BattleSimulator.Instance.enemyList.Remove(this);
        activeTile.isBlocked = false;
        Destroy(gameObject);
        //Debug.Log(EnemyManager.Instance.enemyMap.Count);
    }

    public void SwitchColor(string _color)
    {
        if (_color == "Red")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
            StartCoroutine("Delay");
        }
        else if (_color == "Green")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 1);
            StartCoroutine("Delay");
        }
    }

    public void UMove()
    {
        StartCoroutine(MoveAlongPath());
    }
    // Damage Visuals
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

}
