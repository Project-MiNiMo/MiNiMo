using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour, IEventListener
{
    private const float PRESS_DURATION = 1.0f;
    private float _pressStartTime = 0f;

    private bool _isEditing = false;
    private bool _isPressing = false;
    private bool _longPressTriggered = false;

    private GameObject _pressedObject = null;

    private void Start()
    {
        var eventManager = App.GetManager<EventManager>();

        eventManager.AddListener(EventCode.EditStart, this);
        eventManager.AddListener(EventCode.EditEnd, this);
    }

    private void Update()
    {
        if (_isEditing)
        {
            return;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("GridObject"))
            {
                _isPressing = true;
                _longPressTriggered = false;

                _pressedObject = hit.collider.gameObject;
                _pressStartTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0) && _isPressing)
        {
            _isPressing = false;

            if (!_longPressTriggered && _pressedObject != null)
            {
                HandleSingleClick();
            }

            _pressedObject = null;
        }

        if (_isPressing && _pressedObject != null && !_longPressTriggered)
        {
            if (Time.time - _pressStartTime >= PRESS_DURATION)
            {
                HandleLongPress();

                _isPressing = false;
                _longPressTriggered = true;
            }
        }
    }

    /// <summary>
    /// Pop up building production and management UI 
    /// </summary>
    /// <param name="clickedObject"></param>
    private void HandleSingleClick()
    {
        Debug.Log($"Single click : {_pressedObject.name}");

        _pressedObject.GetComponentInParent<GridObject>().OnClick();
    }

    /// <summary>
    /// Start edit mode with pressedObject
    /// </summary>
    private void HandleLongPress()
    {
        Debug.Log($"Long press : {_pressedObject.name}");

        App.GetManager<EventManager>().PostEvent(EventCode.EditStart, this);
        App.GetManager<GridManager>().SetObject(_pressedObject.GetComponentInParent<GridObject>());
    }

    public void OnEvent(EventCode code, Component sender, object param = null)
    {
        switch (code)
        {
            case EventCode.EditStart:
                _isEditing = true;
                break;

            case EventCode.EditEnd:
                _isEditing = false;
                break;
        }
    }
}
