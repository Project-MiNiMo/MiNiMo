using UnityEngine;

public enum InputState
{
    None,      
    Drag,   
    Click,   
    LongPress   
}

public class InputManager : ManagerBase
{
    public InputState CurrentState { get; private set; } = InputState.None;

    private Vector2 _startPos;
    private float _startTime;
    private bool _isDragging;

    private void Update()
    {
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
            CurrentState = InputState.Click; 
        }

        if (Input.GetMouseButton(0)) // Drag
        {
            if (Vector2.Distance(Input.mousePosition, _startPos) > 10f) 
            {
                CurrentState = InputState.Drag;
                _isDragging = true;
            }
            else if (!_isDragging && Time.time - _startTime > 0.5f) // LongPress
            {
                CurrentState = InputState.LongPress;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_isDragging)
            {
                CurrentState = InputState.Click;
            }
            else
            {
                CurrentState = InputState.None;
            }

            _isDragging = false;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began: // Click
                    _startPos = touch.position;
                    _startTime = Time.time;
                    _isDragging = false;
                    CurrentState = InputState.Click; 
                    break;

                case TouchPhase.Moved: // Drag
                    if (Vector2.Distance(touch.position, _startPos) > 10f)
                    {
                        CurrentState = InputState.Drag;
                        _isDragging = true;
                    }
                    break;

                case TouchPhase.Stationary: // LongPress
                    if (!_isDragging && Time.time - _startTime > 0.5f) 
                    {
                        CurrentState = InputState.LongPress;
                    }
                    break;

                case TouchPhase.Ended:
                    if (!_isDragging)
                    {
                        CurrentState = InputState.Click;
                    }
                    else
                    {
                        CurrentState = InputState.None;
                    }
                    _isDragging = false;
                    break;
            }
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
