using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class EditManager : ManagerBase
{
    public ReactiveProperty<bool> IsEditing { get; } = new(false);
    public ReactiveProperty<Vector3> CurrentCellPosition { get; } = new();
    public BuildingObject CurrentEditObject { get; private set; }
    
    [SerializeField] private GridLayout _gridLayout;
    
    private InstallChecker _installChecker;
    private TileStateModifier _tileStateModifier;

    private void Start()
    {
        _installChecker = GetComponent<InstallChecker>();
        _tileStateModifier = GetComponent<TileStateModifier>();

        InstallExistBuildings();
    }
    
    private async void InstallExistBuildings()
    {
        var buildingManager = App.GetManager<BuildingManager>();
        var buildings = await buildingManager.GetBuildingsAsync();
        
        foreach (var building in buildings)
        {
            if (building.BuildingType == "TestBuilding")
            {
                continue;
            }
            var prefabPath = $"Building/{building.BuildingType}";
            Debug.Log(building.BuildingType);
            var objectPrefab = Resources.Load<GameObject>(prefabPath);
            var position = building.Position != null
                ? new Vector3(building.Position[0], building.Position[1], building.Position[2])
                : Vector3.zero;
            var buildingObject = Instantiate(objectPrefab, position, Quaternion.identity).GetComponent<BuildingObject>();
            buildingObject.Initialize(building);
            _tileStateModifier.ModifyTileState(buildingObject.Area, TileState.Installed);
        }
    }
    
    public void StartEdit(BuildingObject gridObject)
    {
        if (CurrentEditObject && CurrentEditObject != gridObject)
        {
            CancelEdit();
        }
        
        CurrentEditObject = gridObject;
        IsEditing.Value = true;
        
        CurrentEditObject.StartEdit();
        CurrentCellPosition.Value = CurrentEditObject.transform.position;
        
        _tileStateModifier.ModifyTileState(CurrentEditObject.Area, TileState.Empty);
    }

    public void CancelEdit()
    {
        CurrentEditObject.Cancel();
        
        CurrentEditObject = null;
        IsEditing.Value = false;
    }
    
    public async void ConfirmEdit()
    {
        if (!_installChecker.CheckCanInstall(CurrentEditObject))
        {
            return;
        }

        if (await CurrentEditObject.Install())
        {
            _tileStateModifier.ModifyTileState(CurrentEditObject.Area, TileState.Installed);
            
            CurrentEditObject = null;
            IsEditing.Value = false;
        }
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
