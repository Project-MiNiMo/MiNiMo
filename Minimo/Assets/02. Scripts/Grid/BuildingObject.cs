using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

using MinimoShared;

public class BuildingObject : MonoBehaviour
{
    [HideInInspector] public BoundsInt Area;
    public BoundsInt PreviousArea { get; private set; }
    public BuildingData Data { get; private set; }

    private int _id;
    
    private bool _isPlaced = false;
    private bool _isFlipped = false;
    private bool _isPressed = false;
    
    private float _pressTime = 0f;
    private const float LONG_PRESS_THRESHOLD = 3f;
    
    private EditManager _editManager;
    private BuildingManager _buildingManager;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _editManager = App.GetManager<EditManager>();
        _buildingManager = App.GetManager<BuildingManager>();
    }

    public virtual void Initialize(BuildingData data)
    {
        Data = data;

        var size = new Vector3Int(data.SizeX, data.SizeY, 1);
        Area = new BoundsInt(Vector3Int.zero, size);

        var yPosition = (float)((data.SizeX - 1) * 0.5);
        _spriteRenderer.transform.localPosition = new Vector3(0, yPosition, 0);
    }
    
    public void Initialize(BuildingDTO buildingDto)
    {
        _id = buildingDto.Id;
        _isPlaced = true;
        
        var buildingData = App.GetData<TitleData>().Building[buildingDto.Name];
        Initialize(buildingData);
    }

    protected virtual void Update()
    {
        if (_isPressed && !_editManager.IsEditing.Value) 
        {
            _pressTime += Time.deltaTime;
            if (_pressTime >= LONG_PRESS_THRESHOLD)
            {
                _editManager.StartEdit(this);
                _isPressed = false;
            }
        }
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        _isPressed = false;
        _pressTime = 0f;

        if (!_editManager.IsEditing.Value)
        {
            OnClickWhenNotEditing();
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (!_editManager.IsEditing.Value)
        {
            Debug.Log("OnMouseDown");
            _isPressed = true;
            _pressTime = 0f;
        }
        else if(_editManager.IsEditing.Value && _editManager.CurrentEditObject == this)
        {
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            _editManager.MoveObject(touchPosition);
        }
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (_editManager.IsEditing.Value)
        {
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            _editManager.MoveObject(touchPosition);
        }
    }

    #region Edit Functions
    public void StartEdit()
    {
        SetTransparency(0.5f);
    }

    private void EndEdit()
    {
        SetTransparency(1f);
    }
    
    private void SetTransparency(float alpha)
    {
        var color = _spriteRenderer.color;
        color.a = alpha;
        _spriteRenderer.color = color;
    }

    public async UniTask<bool> Install()
    {
        if (_isPlaced)
        {
            return await UpdateBuilding();
        }
        else
        {
            return await CreateBuilding();
        }
    }

    private async UniTask<bool> CreateBuilding()
    {
        var newBuildingRequest = new BuildingDTO
        {
            Name = Data.ID,
            Position = new System.Numerics.Vector3(Area.position.x, Area.position.y, Area.position.x),
        };
        
        var newBuildingDto = await _buildingManager.CreateBuildingAsync(newBuildingRequest);
        if (newBuildingDto != null)
        {
            Debug.Log($"Building created: {newBuildingDto.Name} (ID: {newBuildingDto.Id})");

            _id = newBuildingDto.Id;
            _isPlaced = true;
            PreviousArea = Area;

            EndEdit();
            return true;
        }
        else
        {
            Debug.LogError("Failed to create building");
            return false;
        }
    }
    
    private async UniTask<bool> UpdateBuilding()
    {
        var updateBuildingParameter = new UpdateBuildingParameter
        {
            Id = _id,
            Position = new System.Numerics.Vector3(Area.position.x, Area.position.y, Area.position.x),
        };

        var updatedBuilding = await _buildingManager.UpdateBuildingAsync(updateBuildingParameter);
        if (updatedBuilding != null)
        {
            Debug.Log($"Building updated: {updatedBuilding.Name} (ID: {updatedBuilding.Id})");
            PreviousArea = Area;

            EndEdit();
            return true;
        }
        else
        {
            Debug.LogError("Failed to create building");
            return false;
        }
    }
  
    public void Cancel()
    {
        if (_isPlaced)
        {
            _editManager.MoveObject(PreviousArea);
            EndEdit();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Rotate()
    {
        transform.Rotate(0, _isFlipped ? -180 : 180, 0);
        _isFlipped = !_isFlipped;
    }
    #endregion

    protected virtual void OnClickWhenNotEditing() { }
}
