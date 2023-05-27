using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractablesManager : MonoBehaviour
{
    List<Interactable> entityList;
    public GameObject lampPostPrefab;
    public LampPost lampPost;
    private void Awake()
    {
        entityList = new List<Interactable>();
        
    }
    void Start()
    {
        
        
    }
        
    

    // Update is called once per frame
    void Update()
    {
        if (lampPost == null)
        {
            BoundsInt bounds =
                    MapController.Instance.GetComponentInChildren<Tilemap>().cellBounds;

            int x = int.MaxValue;
            int y = int.MaxValue;

            while (!MapController.Instance.map.ContainsKey(new Vector2Int(x,
                y)))
            {
                x = Random.Range(bounds.min.x, bounds.max.x);
                y = Random.Range(bounds.min.y, bounds.max.y);
            }
            lampPost = Instantiate(lampPostPrefab).GetComponent<LampPost>();
            OverlayTile tile = MapController.Instance.map[new Vector2Int(x, y)];
            tile.isBlocked = true;
            PositionOnTile(lampPost, tile);
            entityList.Clear();

        }
    }

    private void PositionOnTile(Interactable interactable, OverlayTile tile)
    {
        interactable.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        interactable.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        interactable.activeTile = tile;
    }
}
