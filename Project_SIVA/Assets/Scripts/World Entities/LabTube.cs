public class LabTube : WorldEntity
{
    private void Start()
    {
        activeTile.isBlocked = true;
        foreach (OverlayTile tile in MapController.Instance.Get3x3Grid(activeTile))
        {
            tile.isBlocked = true;
        }
    }


}
