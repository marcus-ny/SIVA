using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour
{
    public OverlayTile activeTile;

    public abstract void ReceiveInteraction();

    public abstract void Highlight(bool trigger);
}
