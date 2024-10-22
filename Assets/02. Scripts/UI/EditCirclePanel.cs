using UnityEngine;
using UnityEngine.UI;

public class EditCirclePanel : UIBase
{
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _rotateBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private Button _keepBtn;

    private Camera _mainCamera;
    private RectTransform _rect;

    public override void Initialize()
    {
        _confirmBtn.onClick.AddListener(() => App.GetManager<GridManager>().ConfirmObject());
        _rotateBtn.onClick.AddListener(() => App.GetManager<GridManager>().RotateObject());
        _cancelBtn.onClick.AddListener(() => App.GetManager<GridManager>().CancelObject());
        _keepBtn.onClick.AddListener(() => App.GetManager<GridManager>().CancelObject(true));

        _mainCamera = Camera.main;
        _rect = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    public void SetPosition(Transform _target)
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y += 0.5f;

        Vector3 screenPos = _mainCamera.WorldToScreenPoint(targetPosition);
        _rect.position = screenPos;
    }
}
