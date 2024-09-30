using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum TileType
{
    Empty,
    Possible,
    Impossible,
}

public class GridManager : ManagerBase, IListener
{
    [SerializeField] private GridLayout gridLayout;

    [SerializeField] private Tilemap checkTilemap;
    [SerializeField] private Tilemap markTilemap;
    [SerializeField] private TileBase[] tileBases;

    private GridObject currentObject;
    private Vector3Int currentCellPosition;
    private TileBase[] currentTiles;

    private bool isEditing = false;
    private bool isDragging = false;

    private EditCirclePanel editCircle;

    private void Start()
    {
        App.Instance.GetManager<EventManager>().AddListener(EventCode.EditStart, this);
        App.Instance.GetManager<EventManager>().AddListener(EventCode.EditEnd, this);

        editCircle = App.Instance.GetManager<UIManager>().GetPanel<EditCirclePanel>();
    }

    private void Update()
    {
        if (!isEditing) return;

        HandleInput();
    }

    private void HandleInput()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = gridLayout.WorldToCell(touchPosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            isDragging = true;

            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("GridObject"))
            {
                GridObject gridObject = hit.collider.GetComponentInParent<GridObject>();

                if (currentObject == null || gridObject != currentObject)
                {
                    SetObject(gridObject);
                }
            }
            else
            {
                if (currentObject != null && currentCellPosition != cellPosition)
                {
                    SetObjectPosition(cellPosition);
                    FollowObject();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && currentObject != null)
        {
            if (currentCellPosition != cellPosition)
            {
                SetObjectPosition(cellPosition);
                FollowObject();
            }
        }
    }

    private void SetObjectPosition(Vector3Int cellPosition)
    {
        currentObject.transform.position = gridLayout.CellToWorld(cellPosition) + gridLayout.cellSize * 0.5f;
        currentCellPosition = cellPosition;
    }

    private void FollowObject()
    {
        ClearArea();

        currentObject.area.position = gridLayout.WorldToCell(currentObject.transform.position);
        editCircle.SetPosition(currentObject.transform);

        BoundsInt buildingArea = currentObject.area;
        TileBase[] baseArray = checkTilemap.GetTilesBlock(buildingArea);
        TileBase[] tileArray = new TileBase[baseArray.Length];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[(int)TileType.Empty])
            {
                tileArray[i] = tileBases[(int)TileType.Possible];
            }
            else
            {
                tileArray[i] = tileBases[(int)TileType.Impossible];
            }
        }

        currentTiles = tileArray;
        markTilemap.SetTilesBlock(buildingArea, tileArray);
    }

    private void ClearArea()
    {
        if (currentObject == null) return;

        BoundsInt area = currentObject.area;
        TileBase[] emptyTiles = new TileBase[area.size.x * area.size.y * area.size.z];
        markTilemap.SetTilesBlock(area, emptyTiles);
    }

    #region Manage Object
    public void SetObject(GridObject gridObject, bool isPressing = true)
    {
        if (currentObject != null)
        {
            CancelObject();
        }

        currentObject = gridObject;
        currentObject.StartEdit();
        SetObjectPosition(currentObject.area.position);
        editCircle.OpenPanel();

        isDragging = isPressing;

        FollowObject();
    }

    public void ConfirmObject()
    {
        if (currentObject == null) return;

        if (CheckCanPlace())
        {
            currentObject.Place();
            ClearArea();

            currentObject = null;
            editCircle.ClosePanel();
        }
    }

    public void CancelObject(bool keepObject = false)
    {
        ClearArea();

        if (!currentObject.IsPlaced || keepObject)
        {
            App.Instance.GetManager<UIManager>().GetPanel<StoragePanel>().AddObjectCount(currentObject.name, 1);
            Destroy(currentObject.gameObject);
        }
        else
        {
            SetObjectPosition(currentObject.PreviousArea.position);
            currentObject.EndEdit();
        }

        currentObject = null;
        currentTiles = null;
        editCircle.ClosePanel();
    }

    public void RotateObject()
    {
        currentObject?.Rotate();
        FollowObject();
    }
    #endregion

    private bool CheckCanPlace()
    {
        foreach (var tile in currentTiles)
        {
            if (tile == tileBases[(int)TileType.Impossible])
            {
                return false;
            }
        }

        return true;
    }

    public void OnEvent(EventCode code, Component sender, object param = null)
    {
        switch (code)
        {
            case EventCode.EditStart:
                isEditing = true;
                break;

            case EventCode.EditEnd:
                isEditing = false;
                if (currentObject != null)
                {
                    CancelObject();
                }
                break;
        }
    }
}
