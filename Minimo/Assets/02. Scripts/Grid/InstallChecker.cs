using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InstallChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _markTilemap;
    [SerializeField] private TileBase _possibleTile;
    [SerializeField] private TileBase _impossibleTile;

    private EditManager _editManager;
    private GridManager _gridManager;

    private void Start()
    {
        _editManager = App.GetManager<EditManager>();
        _gridManager = App.GetManager<GridManager>();
        
        _editManager.IsEditing
            .Subscribe((isEditing) =>
            {
                if (!isEditing)
                {
                    ClearMarkTiles();
                }
            }).AddTo((gameObject));

        _editManager.CurrentCellPosition
            .Subscribe((position) =>
            {
                Debug.Log("편집중");
                if(!_editManager.CurrentEditObject)
                {
                    return;
                }
                Debug.Log("하는 중");
                SetMarkTiles(_editManager.CurrentEditObject);
            }).AddTo(gameObject);
    }
    
    private void ClearMarkTiles()
    {
        _markTilemap.ClearAllTiles();
    }

    private void SetMarkTiles(GridObject gridObject)
    {
        ClearMarkTiles();
        Debug.Log("체크중");
        foreach (var position in gridObject.Area.allPositionsWithin)
        {
            _markTilemap.SetTile(position, _gridManager.CheckCanInstall(position) ? 
                _possibleTile : _impossibleTile);
        }
    }
}
