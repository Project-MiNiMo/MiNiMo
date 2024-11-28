using UniRx;
using UnityEngine;

public class EditManager : ManagerBase
{
    public ReactiveProperty<bool> IsEditing { get; } = new(false);
    public GridObject CurrentEditObject { get; private set; }
    public ReactiveProperty<Vector3> CurrentCellPosition { get; } = new();

    [SerializeField] private GridLayout _gridLayout;
    
    public void StartEdit(GridObject gridObject)
    {
        if (CurrentEditObject && CurrentEditObject != gridObject)
        {
            CancelEdit();
        }
        
        CurrentEditObject = gridObject;
        IsEditing.Value = true;
        
        CurrentEditObject.StartEdit();
    }

    public void CancelEdit()
    {
        if (CurrentEditObject.IsPlaced) 
        {
            Destroy(CurrentEditObject.gameObject);
        }
        else
        {
            CurrentEditObject.EndEdit();
        }
        
        CurrentEditObject = null;
        IsEditing.Value = false;
    }
    
    public void ConfirmEdit()
    {
        if (!App.GetManager<GridManager>().CheckCanInstall(CurrentEditObject))
        {
            return;
        }
        
        CurrentEditObject.Place();
        
        CurrentEditObject = null;
        IsEditing.Value = false;
    }

    public void MoveObject(Vector2 touchPosition)
    {
        var cellPosition = _gridLayout.WorldToCell(touchPosition);
        CurrentEditObject.transform.position = _gridLayout.CellToWorld(cellPosition) + _gridLayout.cellSize * 0.5f;
        CurrentEditObject.Area.position = cellPosition;
        CurrentCellPosition.Value = CurrentEditObject.transform.position;
    }
}
