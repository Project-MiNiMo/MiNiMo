using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour, IEventListener
{
    private const float PRESS_DURATION = 1.0f;
    private float _pressStartTime = 0f;

    private bool _isEditing = false;
    private bool _isPressing = false;

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
                _pressedObject = hit.collider.gameObject;
                _pressStartTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isPressing = false;
            _pressedObject = null;
        }

        if (_isPressing && _pressedObject != null)
        {
            if (Time.time - _pressStartTime >= PRESS_DURATION)
            {
                var eventManager = App.GetManager<EventManager>();
                eventManager.PostEvent(EventCode.EditStart, this);

                var gridManager = App.GetManager<GridManager>();
                gridManager.SetObject(_pressedObject.GetComponent<GridObject>());

                _isPressing = false;
            }
        }
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
