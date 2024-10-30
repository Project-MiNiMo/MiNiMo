using UnityEngine;
using UnityEngine.UI;

public class ProduceCircleCtrl : MonoBehaviour
{
    [SerializeField] Button _infoBtn;
    [SerializeField] Button _harvestBtn;

    private Camera _mainCamera;
    private RectTransform _rect;

    public void Initialize()
    {
        //_infoBtn.onClick.AddListener(() => App.GetManager<GridManager>().ConfirmObject());
        //_harvestBtn.onClick.AddListener(() => App.GetManager<GridManager>().RotateObject());

        _mainCamera = Camera.main;
        _rect = GetComponent<RectTransform>();
    }

    public void SetPosition(Transform _target)
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y += 3f;

        Vector3 screenPos = _mainCamera.WorldToScreenPoint(targetPosition);
        _rect.position = screenPos;
    }
}
