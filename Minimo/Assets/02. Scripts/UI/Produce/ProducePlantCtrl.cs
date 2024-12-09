using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProducePlantCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private LayerMask _targetLayerMask;
    
    private RectTransform _rect;
    private Image _image;
    private Canvas _canvas;
    
    private Vector3 _startPosition;

    private ProduceOption _currentOption;
    
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _canvas = GetComponentInParent<Canvas>();
        
        _startPosition = _rect.anchoredPosition;
    }

    public void Initialize(ProduceOption option)
    {
        _currentOption = option;
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
            component.StartProduce(_currentOption);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        _rect.anchoredPosition = _startPosition;
    }
}
