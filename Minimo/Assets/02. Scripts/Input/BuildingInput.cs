using UnityEngine;

public class BuildingInput : MonoBehaviour
{
    private InputManager _input;
    private Camera _mainCamera;
    
    private BuildingObject _currentBuilding;
    private EditManager _editManager;

    private int _layerMask;

    private void Start()
    {
        _input = App.GetManager<InputManager>();
        _mainCamera = Camera.main;
        _editManager = App.GetManager<EditManager>();
        
        _layerMask = LayerMask.GetMask("Building");
    }

    private void Update()
    {
        switch (_input.CurrentState)
        {
            case InputState.ClickDown:
                HandleClickDown();
                break;
            
            case InputState.ClickUp:
                HandleClickUp();
                break;
            
            case InputState.LongPress:
                HandleLongPress();
                break;
            
            case InputState.Drag:
                HandleDrag();
                break;
        }
    }

    private void HandleClickDown()
    {
        var hit = RaycastBuilding();
        _currentBuilding = hit;
    }

    private void HandleClickUp()
    {
        if (_currentBuilding == null)
        {
            if (!_editManager.IsEditing.Value) return;
            
            var screenPosition = Input.mousePosition;
            var worldPosition = _mainCamera.ScreenToWorldPoint(
                new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
            worldPosition.z = 0;
            _editManager.MoveObject(worldPosition);
        }
        else
        {
            _currentBuilding.OnClickUp();
            _currentBuilding = null;
        }
    }

    private void HandleLongPress()
    {
        if (_currentBuilding == null) return;
        
        _currentBuilding.OnLongPress();
        _currentBuilding = null;
    }
    
    private void HandleDrag()
    {
        if (_currentBuilding == null) return;
        
        _currentBuilding = null;
    }

    private BuildingObject RaycastBuilding()
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.OverlapPoint(worldPosition, _layerMask);
        if (hit != null && hit.TryGetComponent<BuildingObject>(out var component))
        {
            return component;
        }
        
        return null;
    }
}
