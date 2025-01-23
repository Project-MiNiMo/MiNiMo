using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Drag")]
    [SerializeField] private float _dragSpeed = 10f;

    [Header("Zoom")]
    [SerializeField] private float _zoomSpeed = 10f; 
    [SerializeField] private float _minZoom = 5f;  
    [SerializeField] private float _maxZoom = 10f; 

    private InputManager _input;
    
    private void Start()
    {
        _input = App.GetManager<InputManager>();
    }
    
    private void Update()
    {
        Debug.Log($"Current State: {_input.CurrentState}");
        
        if (_input.CurrentState == InputState.Drag)
        {
            Move();
        }
        
        Zoom();
    }

    private void Move()
    {
        if (Input.touchCount == 1) // Touch
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                var delta = touch.deltaPosition;
                var move = new Vector3(-delta.x * _dragSpeed, -delta.y * _dragSpeed, 0);
                Camera.main.transform.Translate(move * Time.deltaTime, Space.World);
            }
        }
        else if (Input.GetMouseButton(0)) // Mouse
        {
            var delta = new Vector3(-Input.GetAxis("Mouse X") * _dragSpeed, -Input.GetAxis("Mouse Y") * _dragSpeed, 0);
            Camera.main.transform.Translate(delta * Time.deltaTime, Space.World);
        }
    }

    private void Zoom()
    {
        if (Input.touchCount == 2) // Touch
        {
            var touch1 = Input.GetTouch(0);
            var touch2 = Input.GetTouch(1);
            
            var prevDistance = (touch1.position - touch1.deltaPosition).magnitude - (touch2.position - touch2.deltaPosition).magnitude;
            var currentDistance = (touch1.position - touch2.position).magnitude;

            var deltaDistance = currentDistance - prevDistance;

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - deltaDistance * _zoomSpeed * Time.deltaTime, _minZoom, _maxZoom);
        }
        
        var scroll = Input.GetAxis("Mouse ScrollWheel"); // Mouse
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * _zoomSpeed, _minZoom, _maxZoom);
        }
    }
}
