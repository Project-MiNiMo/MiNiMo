using UnityEngine;
using UnityEngine.UI;

public class EditCirclePanel : UIBase
{
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private Button _moveBtn;

    private GridManager _gridManager;

    private Camera _mainCamera;
    private RectTransform _rect;

    private Transform _target;

    public override void Initialize()
    {
        _gridManager = App.GetManager<GridManager>();
        _confirmBtn.onClick.AddListener(() => _gridManager.ConfirmObject());
        _cancelBtn.onClick.AddListener(() => _gridManager.CancelObject());
        _moveBtn.onClick.AddListener(() => _gridManager.CancelObject(true));

        _mainCamera = Camera.main;
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_target == null || !gameObject.activeInHierarchy)
        {
            return;
        }

        SetPosition();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void SetPosition()
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y += 0.5f;

        Vector3 screenPos = _mainCamera.WorldToScreenPoint(targetPosition);
        _rect.position = screenPos;
    }
}
