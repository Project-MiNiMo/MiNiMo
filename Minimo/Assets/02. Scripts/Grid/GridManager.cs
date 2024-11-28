using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : ManagerBase
{
    [SerializeField] private Tilemap _checkTilemap;
    [SerializeField] private TileBase _emptyTile;

    private GridLayout _gridLayout;
    
    private TileBase[] _currentTiles;

    private Camera _mainCamera;

    private void Start()
    {
        _gridLayout = GetComponent<GridLayout>();
        _mainCamera = Camera.main;
    }

    public bool CheckCanInstall(GridObject gridObject)
    {
        var buildingArea = gridObject.Area;
        var baseArray = _checkTilemap.GetTilesBlock(buildingArea);
   
        return baseArray.All(tile => tile == _emptyTile);
    }

    public bool CheckCanInstall(Vector3Int position)
    {
        var tile = _checkTilemap.GetTile(position);
        
        return tile == _emptyTile;
    }
}
