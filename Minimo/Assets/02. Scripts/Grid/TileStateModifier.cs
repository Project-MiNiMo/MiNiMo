using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileState
{
    Empty,
    Installed
}

public class TileStateModifier : MonoBehaviour
{
    [SerializeField] private Tilemap _installTilemap;
    [SerializeField] private TileBase _installedTile;

    public void ModifyTileState(BoundsInt area, TileState tileState)
    {
        var totalSize = area.size.x * area.size.y;
        var tiles = new TileBase[totalSize];
        
        if (tileState == TileState.Installed)
        {
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i] = _installedTile;
            }
        }
        
        _installTilemap.SetTilesBlock(area, tiles);
    }
}
