using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class InstallChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _checkTilemap;
    [SerializeField] private Tilemap _installTilemap;
    
    [SerializeField] private TileBase _checkTile;
    
    public bool CheckCanInstall(BuildingObject gridObject)
    {
        var buildingArea = gridObject.Area;
        
        var checkArray = _checkTilemap.GetTilesBlock(buildingArea);
        var installArray = _installTilemap.GetTilesBlock(buildingArea);
   
        return checkArray.All(tile => tile == _checkTile) && installArray.All(tile => tile == null);
    }

    public bool CheckCanInstall(Vector3Int position)
    {
        var checkTile = _checkTilemap.GetTile(position);
        var installTile = _installTilemap.GetTile(position);
        
        return checkTile == _checkTile && installTile == null;
    }
    
    public List<Vector3> GetInstallablePositions()
    {
        var positions = new List<Vector3>();
        
        foreach (var position in _installTilemap.cellBounds.allPositionsWithin)
        {
            if (CheckCanInstall(position))
            {
                var worldPosition = _installTilemap.CellToWorld(position);
                worldPosition += _installTilemap.cellSize / 2;
                positions.Add(worldPosition);
            }
        }

        return positions;
    }
}
