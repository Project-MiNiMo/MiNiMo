using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimoInput : MonoBehaviour
{
    private Camera _mainCamera;
    
    private InputManager _input;
    private EditManager _editManager;

    private Minimo _currentObject;
    
    private int _layerMask;

    private void Start()
    {
        _mainCamera = Camera.main;
        
        _input = App.GetManager<InputManager>();
        
        _layerMask = LayerMask.GetMask("Minimo");
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
        var hit = GetRaycastObject();
        _currentObject = hit;
    }

    private void HandleClickUp()
    {
        _currentObject = null;
    }

    private void HandleLongPress()
    {
        //if(_currentObject.FSM.CurrentState is )
        _currentObject = null;
    }
    
    private void HandleDrag()
    {
        _currentObject = null;
    }

    private Minimo GetRaycastObject()
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.OverlapPoint(worldPosition, _layerMask);
        if (hit != null && hit.TryGetComponent<Minimo>(out var component))
        {
            return component;
        }
        
        return null;
    }
}
