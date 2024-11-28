using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceCircleCtrl : MonoBehaviour
{
    [SerializeField] Button _infoBtn;
    [SerializeField] Button _harvestBtn;
    [SerializeField] GameObject _remainTimeObj;
    [SerializeField] TextMeshProUGUI _remainTimeTMP;

    private Camera _mainCamera;
    private RectTransform _rect;

    private int _remainTime = 0;
    private float _lastUpdateTime; 

    public void Initialize()
    {
        //_infoBtn.onClick.AddListener(() => App.GetManager<GridManager>().ConfirmObject());
        //_harvestBtn.onClick.AddListener(() => App.GetManager<GridManager>().RotateObject());

        _mainCamera = Camera.main;
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //if (_remainTimeObj.activeSelf && _remainTime > 0)
        //{
        //    if (Time.time - _lastUpdateTime >= 1f)
        //    {
        //        _remainTime--;
        //        _lastUpdateTime = Time.time;
//
         //       _remainTimeTMP.text = FormatTime(_remainTime);
//
          //      if (_remainTime <= 0)
          //      {
          //          _remainTimeObj.SetActive(false);
          //          _remainTime = 0;
          //      }
          //  }
        //}
    }

    private string FormatTime(int timeInSeconds)
    {
        int minutes = timeInSeconds / 60;
        int seconds = timeInSeconds % 60;
        return $"{minutes:00}:{seconds:00}"; 
    }

    public void SetPosition(Transform _target)
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y += 3f;

        Vector3 screenPos = _mainCamera.WorldToScreenPoint(targetPosition);
        _rect.position = screenPos;
    }

    public void SetRemainTime(bool isActive, int remainTime = 0)
    {
        _remainTime = remainTime;

        _remainTimeObj.SetActive(isActive);
    }
}
