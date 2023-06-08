using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;

    public float hitpoints;

    private Animator animator;

    public Vector3Int cur, prev;

    private PathFinder pathFinder;
    private Rangefinder rangeFinder;

    private readonly int speed = 3;

    public List<OverlayTile> path;
    private List<OverlayTile> reachableTiles = new();

    public MouseController mouseController;

    private void Start()
    {
        hitpoints = 100;
        animator = GetComponent<Animator>();

        pathFinder = new();
        rangeFinder = new();

        path = new();
    }

    /*
     * For showing red visual when taking damage
     */
    IEnumerator DamageVisual()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    public void DisplayDamageVisual()
    {       
        StartCoroutine("DamageVisual");
    }

    /*
     * Move the character prefab to the desired tile
     */
    private void PositionCharacterOnTile(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x,
            tile.transform.position.y, tile.transform.position.z);

        gameObject.GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;
        activeTile = tile;
        activeTile.isBlocked = true;
    }
    /*
     * Set reachable tiles within a given range and highlight these tiles
     */
    private void GetMovementRange()
    {
        foreach (var tile in reachableTiles)
        {
            tile.HideTile();
        }

        reachableTiles = rangeFinder.GetReachableTiles(activeTile, 3);

        foreach (var tile in reachableTiles)
        {
            tile.ShowTile();
        }
    }

    // Set up a co-routine for this
    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        activeTile.isBlocked = false;

        // This animation code should be abstracted to somewhere else
        // Keep this file only for controlling the player and nothing else
        prev = path[0].previous.gridLocation;
        cur = path[0].gridLocation;

        transform.position = Vector2.MoveTowards(transform.position,
            path[0].transform.position, step);

        transform.position = new Vector3(transform.position.x,
            transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetMovementRange();
        }
        // character.activeTile.isBlocked = true;
    }
}
