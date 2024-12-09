using System;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    [HideInInspector] public BoundsInt Area;
    public BoundsInt PreviousArea { get; private set; }
    public BuildingData Data { get; private set; }

    private bool _isPlaced = false;
    private bool _isFlipped = false;
    private bool _isPressed = false;
    private const float LONG_PRESS_THRESHOLD = 3f;
    private float _pressTime = 0f;
    
    private EditManager _editManager;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _editManager = App.GetManager<EditManager>();
    }

    public virtual void Initialize(BuildingData data, Sprite sprite)
    {
        Data = data;

        var size = new Vector3Int(data.SizeX, data.SizeY, 1);
        Area = new BoundsInt(Vector3Int.zero, size);

        _spriteRenderer.sprite = sprite;

        var yPosition = (float)((data.SizeX - 1) * 0.5);
        _spriteRenderer.transform.localPosition = new Vector3(0, yPosition, 0);
    }

    private void Update()
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
        _isPressed = false;
        _pressTime = 0f;

        if (!_editManager.IsEditing.Value)
        {
            OnClickWhenNotEditing();
        }
    }

    private void OnMouseDown()
    {
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

    public void Install()
    {
        _isPlaced = true;
        PreviousArea = Area;

        EndEdit();
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
