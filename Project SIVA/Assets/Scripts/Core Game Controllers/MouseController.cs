using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private static MouseController _instance;

    public static MouseController Instance { get { return _instance; } }

    public OverlayTile mouseOverTile;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (BattleSimulator.Instance.State == BattleState.PLAYER_TURN)
        {
            var focusedTileHit = GetFocusedOnTile();

            // If the raycast on cursor hits a valid cell
            if (focusedTileHit.HasValue)
            {
                OverlayTile overlayTile =
                    focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

                mouseOverTile = overlayTile;

                transform.position = overlayTile.transform.position;

                gameObject.GetComponent<SpriteRenderer>().sortingOrder =
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder;               
            }
            
        }

    }
    
    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        // Mouse click may hit multiple tiles, but we always want to get the top layer
        // Get a list of tiles hit --> sort by descending Z value --> Pick the first one in order
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }
}
