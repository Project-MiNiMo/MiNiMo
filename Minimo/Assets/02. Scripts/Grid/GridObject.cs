using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridObject : MonoBehaviour
{
    [HideInInspector] public BoundsInt Area;
    public BoundsInt PreviousArea { get; private set; }
    public BuildingData Data { get; private set; }

    private bool _isPlaced = false;
    private bool _isFlipped = false;
    private bool _isPressed = false;
    private bool _isDragUI = false;
    private const float LONG_PRESS_THRESHOLD = 3f;
    private SpriteRenderer _spriteRenderer;
    
    private EditManager _editManager;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Start()
    {
        _editManager = App.GetManager<EditManager>();
        
        // 마우스 눌렀을 때 스트림
        var mouseDownStream = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0));

        // 마우스 뗐을 때 스트림
        var mouseUpStream = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0));

        // 마우스가 눌린 상태 스트림
        var mouseHoldStream = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0));

        // 편집 모드일 때: 터치 시
        mouseDownStream
            .Where(_ => _editManager.IsEditing.Value)
            .Subscribe(_ =>
            {
                _isDragUI = EventSystem.current.IsPointerOverGameObject();
                _editManager.StartEdit(this);
            })
            .AddTo(gameObject);

        // 편집 모드일 때: 드래그 시
        mouseHoldStream
            .Where(_ => _editManager.IsEditing.Value)
            .Where(_ => !_isDragUI) 
            .Subscribe(_ =>
            {
                var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0;
                _editManager.MoveObject(touchPosition);
            })
            .AddTo(gameObject);

        // 편집 모드가 아닐 때: 터치 시
        mouseDownStream
            .Where(_ => !_editManager.IsEditing.Value)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject())
            .Subscribe(_ =>
            {
                _isPressed = true;
                
                Observable.Timer(System.TimeSpan.FromSeconds(LONG_PRESS_THRESHOLD))
                    .TakeUntil(mouseUpStream)
                    .Subscribe(__ =>
                    {
                        if (_isPressed)
                        {
                            _editManager.StartEdit(this);
                        }
                    })
                    .AddTo(gameObject);
            })
            .AddTo(gameObject);
        
        mouseUpStream
            .Subscribe(_ =>
            {
                _isDragUI = false;
                _isPressed = false;
            })
            .AddTo(gameObject);
    }

    public void Initialize(BuildingData data, Sprite sprite)
    {
        Data = data;

        var size = new Vector3Int(data.SizeX, data.SizeY, 1);
        Area = new BoundsInt(Vector3Int.zero, size);

        _spriteRenderer.sprite = sprite;

        var yPosition = (float)((data.SizeX - 1) * 0.5);
        _spriteRenderer.transform.localPosition = new Vector3(0, yPosition, 0);
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

    public void Place()
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
}
