using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProduceHarvestCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ProduceManager _produceManager;
    
    private LayerMask _targetLayerMask;

    [SerializeField] private RectTransform _parentRect;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    private Canvas _canvas;
    
    private Vector3 _startPosition;

    private void Start()
    {
        _targetLayerMask = LayerMask.GetMask("Building");
  
        _canvas = GetComponentInParent<Canvas>();
        
        _startPosition = _rect.anchoredPosition;
        
        _produceManager = App.GetManager<ProduceManager>();
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                if (isProducing)
                {
                    var isHarvest = _produceManager.CurrentProduceObject.CurrentState == ProduceState.Harvest;
                    gameObject.SetActive(isHarvest);
                    SetPosition();
                }
            }).AddTo(gameObject);
        
        _produceManager.CurrentRemainTime
            .Subscribe((remainTime) =>
            {
                if (_produceManager.CurrentProduceObject?.CurrentState == ProduceState.Produce 
                    && remainTime <= 0)
                {
                    gameObject.SetActive(true);
                }
            })
            .AddTo(gameObject);
    }
    
    private void SetPosition()
    {
        var position = _produceManager.CurrentProduceObject.transform.position;
        var screenPos = Camera.main.WorldToScreenPoint(position);
        _parentRect.position = screenPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPosition, _targetLayerMask);
        if (hit != null && hit.TryGetComponent<ProduceObject>(out var component))
        {
            component.StartHarvest();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        _rect.anchoredPosition = _startPosition;
    }
}

