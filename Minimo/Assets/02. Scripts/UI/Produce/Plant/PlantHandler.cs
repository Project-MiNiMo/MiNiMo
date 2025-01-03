using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlantHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private LayerMask _targetLayerMask;
    
    private RectTransform _rect;
    private Image _image;
    private Canvas _canvas;
    
    private Vector3 _startPosition;

    private ProduceOption _currentOption;
    private ProduceManager _produceManager;
    
    private void Start()
    {
        _targetLayerMask = LayerMask.GetMask("Building");
        _produceManager = App.GetManager<ProduceManager>();
        
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
        List<RaycastResult> raycastResults = new();
        GraphicRaycaster raycaster = _canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(eventData, raycastResults);
        foreach (var result in raycastResults)
        {
            if (result.gameObject.CompareTag("ProduceTaskBtn"))
            {
                Debug.Log($"1 : {_currentOption.Results[0].Code}");
                Debug.Log($"2 : {_produceManager.CurrentProduceObject.name}");
                Debug.Log($"2-1 : Current Option Valid: {_produceManager.CurrentProduceObject.ProduceData.ProduceOptions.Contains(_currentOption)}");
                
                var currentObject = _produceManager.CurrentProduceObject;
                
                Debug.Log(currentObject.name);
                Debug.Log(currentObject.StartProduce(_currentOption).ToString());
                //currentObject.StartProduce(_currentOption);
                
                Debug.Log("6 : Produce Start");
                break; 
            }
        }
        
        _image.raycastTarget = true;
        _rect.anchoredPosition = _startPosition;
    }
}
