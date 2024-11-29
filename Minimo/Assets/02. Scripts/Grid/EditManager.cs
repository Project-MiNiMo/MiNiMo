using System;
using UniRx;
using UnityEngine;

public class EditManager : ManagerBase
{
    public ReactiveProperty<bool> IsEditing { get; } = new(false);
    public GridObject CurrentEditObject { get; private set; }
    public ReactiveProperty<Vector3> CurrentCellPosition { get; } = new();

    [SerializeField] private GridLayout _gridLayout;
    
    private InstallChecker _installChecker;

    private void Start()
    {
        _installChecker = GetComponent<InstallChecker>();
    }
    
    public void StartEdit(GridObject gridObject)
    {
        Debug.Log(CurrentEditObject == null);   
        if (CurrentEditObject && CurrentEditObject != gridObject)
        {
            CancelEdit();
        }
        
        CurrentEditObject = gridObject;
        IsEditing.Value = true;
        
        CurrentEditObject.StartEdit();
        CurrentCellPosition.Value = CurrentEditObject.transform.position;
        
        Debug.Log(CurrentEditObject == null);  
    }

    public void CancelEdit()
    {
        CurrentEditObject.Cancel();
        
        CurrentEditObject = null;
        IsEditing.Value = false;
    }
    
    public void ConfirmEdit()
    {
        if (!_installChecker.CheckCanInstall(CurrentEditObject))
        {
            return;
        }
        
        CurrentEditObject.Place();
        
        CurrentEditObject = null;
        IsEditing.Value = false;
        
        Debug.Log(CurrentEditObject == null);  
    }

    public void MoveObject(Vector3 touchPosition)
    {
        var cellPosition = _gridLayout.WorldToCell(touchPosition);
        SetCurrentPosition(cellPosition);
    }
    
    public void MoveObject(BoundsInt area)
    {
        SetCurrentPosition(area.position);
    }
    
    private void SetCurrentPosition(Vector3Int position)
    {
        CurrentEditObject.transform.position = _gridLayout.CellToWorld(position) + _gridLayout.cellSize * 0.5f;
        CurrentEditObject.Area.position = _gridLayout.WorldToCell(CurrentEditObject.transform.position);
        CurrentCellPosition.Value = CurrentEditObject.transform.position;
    }
}
