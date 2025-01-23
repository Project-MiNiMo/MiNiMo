using UnityEngine;
using UnityEngine.EventSystems;

public enum InputState
{
    None,      
    Drag,  
    ClickDown,
    ClickUp,   
    LongPress,
    Zoom
}

public class InputManager : ManagerBase
{
    public InputState CurrentState { get; private set; } = InputState.None;
    private InputState _previousState = InputState.None;

    private Vector2 _startPos;
    private float _startTime;
    private bool _isDragging;

    private const float DragThreshold = 10f; 
    private const float LongPressThreshold = 1f; 
    
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            ResetState();
            return;
        }

        HandleInput();
        
        if (_previousState == InputState.ClickUp)
        {
            CurrentState = InputState.None;
        }

        _previousState = CurrentState;
    }
    
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // Click
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                CurrentState = InputState.None;
                _isDragging = false;
                return;
            }
            
            _startPos = Input.mousePosition;
            _startTime = Time.time;
            _isDragging = false;
            CurrentState = InputState.ClickDown; 
        }

        if (Input.GetMouseButton(0)) 
        {
            if (!_isDragging && Vector2.Distance(Input.mousePosition, _startPos) > DragThreshold) // Drag
            {
                CurrentState = InputState.Drag;
                _isDragging = true;
            }
            else if (!_isDragging && Time.time - _startTime > LongPressThreshold) // LongPress
            {
                CurrentState = InputState.LongPress;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentState == InputState.ClickDown)
            {
                CurrentState = InputState.ClickUp;
            }
            else 
            {
                CurrentState = InputState.None;
            }

            _isDragging = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            CurrentState = InputState.Zoom;
            _isDragging = false;
        }

        if (Input.touchCount == 2) 
        {
            CurrentState = InputState.Zoom;
            _isDragging = false;
        }
    }
    
    private void ResetState()
    {
        CurrentState = InputState.None;
        _isDragging = false;
    }
}
