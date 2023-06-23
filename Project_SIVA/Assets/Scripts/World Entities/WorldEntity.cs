using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldEntity: MonoBehaviour
{
    public OverlayTile activeTile;
    public abstract void Highlight(bool trigger);
}
