using UnityEngine;

public class BuildingInput : MonoBehaviour
{
    private InputManager _input;
    private Camera _mainCamera;
    
    private BuildingObject _currentBuilding;
    private EditManager _editManager;

    private void Start()
    {
        _input = App.GetManager<InputManager>();
        _mainCamera = Camera.main;
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
        }
    }

    private void HandleClickDown()
    {
        var hit = RaycastBuilding();
        if (hit != null)
        {
            _currentBuilding = hit;
        }
    }

    private void HandleClickUp()
    {
        if (_currentBuilding == null)
        {
            if (Input.touchCount == 1) 
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    var touchPosition = _mainCamera.ScreenToWorldPoint(
                        new Vector3(touch.position.x, touch.position.y, _mainCamera.nearClipPlane));
                    _editManager.MoveObject(touchPosition);
                }
            }
            else if (Input.GetMouseButton(0)) 
            {
                var screenPosition = Input.mousePosition;
                var worldPosition = _mainCamera.ScreenToWorldPoint(
                    new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
                _editManager.MoveObject(worldPosition);
            }
        }
        else
        {
            var hit = RaycastBuilding();
            if (_currentBuilding == hit)
            {
                _currentBuilding.OnClickUp();
                _currentBuilding = null;
            }
        }
    }

    private void HandleLongPress()
    {
        if (_currentBuilding == null) return;
        
        var hit = RaycastBuilding();
        if (_currentBuilding == hit)
        {
            _currentBuilding.OnLongPress();
            _currentBuilding = null;
        }
    }

    private BuildingObject RaycastBuilding()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            return hit.collider.GetComponent<BuildingObject>();
        }
        return null;
    }
}
