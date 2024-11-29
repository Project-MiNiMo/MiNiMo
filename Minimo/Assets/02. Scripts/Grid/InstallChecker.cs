using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

public class InstallChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _checkTilemap;
    
    private TileBase[] _currentTiles;
    
    public bool CheckCanInstall(BuildingObject gridObject)
    {
        var buildingArea = gridObject.Area;
        var baseArray = _checkTilemap.GetTilesBlock(buildingArea);
   
        return baseArray.All(tile => tile);
    }

    public bool CheckCanInstall(Vector3Int position)
    {
        var tile = _checkTilemap.GetTile(position);
        
        return tile;
    }
}
