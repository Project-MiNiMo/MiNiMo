using UnityEngine;
using UnityEngine.UI;

public class EditCirclePanel : UIBase
{
    [SerializeField] Button confirmBtn;
    [SerializeField] Button rotateBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] Button keepBtn;

    private Camera mainCamera;
    private RectTransform rect;

    public override void Initialize()
    {
        confirmBtn.onClick.AddListener(() => App.Instance.GetManager<GridManager>().ConfirmObject());
        rotateBtn.onClick.AddListener(() => App.Instance.GetManager<GridManager>().RotateObject());
        cancelBtn.onClick.AddListener(() => App.Instance.GetManager<GridManager>().CancelObject());
        keepBtn.onClick.AddListener(() => App.Instance.GetManager<GridManager>().CancelObject(true));

        mainCamera = Camera.main;
        rect = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void SetPosition(Transform _target)
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y += 0.5f;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPosition);
        rect.position = screenPos;
    }
}
