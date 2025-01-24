using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomBtn : MonoBehaviour
{
    [SerializeField] private GameObject _alertIcon;
    [SerializeField] private Transform _openParent;
    
    private RectTransform _rect;

    private float _defaultY;
    private Transform _defaultParent;
    
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _defaultY = _rect.anchoredPosition.y;
        _defaultParent = _rect.parent;
    }
    
    public void MoveBtn(bool isActive)
    {
        _rect.DOKill();
        
        var targetY = isActive ? 0 : _defaultY;
        _rect.DOAnchorPosY(targetY, 0.5f);

        if (isActive)
        {
            SetAlert(false);
            _rect.SetParent(_openParent);
        }
        else
        {
            _rect.SetParent(_defaultParent);
        }
    }
    
    public void SetAlert(bool isActive)
    {
        _alertIcon.SetActive(isActive);
    }
}
