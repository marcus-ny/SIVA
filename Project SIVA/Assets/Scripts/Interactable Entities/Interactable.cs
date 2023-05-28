using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour
{
    public OverlayTile activeTile;
    public GameObject prefab;

    public abstract void ReceiveInteraction();
}
