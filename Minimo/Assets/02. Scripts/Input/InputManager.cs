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

    private Vector2 _startPos;
    private float _startTime;
    private bool _isDragging;

    private const float DragThreshold = 10f; 
    private const float LongPressThreshold = 1f; 
    
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CurrentState = InputState.None;
            _isDragging = false;
            return;
        }
        
        HandleTouchInput(); // Touch
        
        HandleMouseInput(); // Mouse
    }
    
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Click
        {
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
            else if (_isDragging) 
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
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began: // Click
                    _startPos = touch.position;
                    _startTime = Time.time;
                    _isDragging = false;
                    CurrentState = InputState.ClickDown; 
                    break;

                case TouchPhase.Moved: // Drag
                    if (Vector2.Distance(touch.position, _startPos) > DragThreshold)
                    {
                        CurrentState = InputState.Drag;
                        _isDragging = true;
                    }
                    break;

                case TouchPhase.Stationary: // LongPress
                    if (!_isDragging && Time.time - _startTime > LongPressThreshold) 
                    {
                        CurrentState = InputState.LongPress;
                    }
                    break;

                case TouchPhase.Ended:
                    if (CurrentState == InputState.ClickDown)
                    {
                        CurrentState = InputState.ClickUp;
                    }
                    else if (_isDragging) 
                    {
                        CurrentState = InputState.None;
                    }
                    
                    _isDragging = false;
                    break;
            }
        }
        else if (Input.touchCount > 1)
        {
            CurrentState = InputState.Zoom;
            _isDragging = false;
        }
        else
        {
            if (CurrentState != InputState.Drag && CurrentState != InputState.LongPress)
            {
                CurrentState = InputState.None;
            }
        }
    }
}
