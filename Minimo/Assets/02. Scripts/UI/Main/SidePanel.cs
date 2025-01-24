using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SidePanel : UIBase
{
    [Header("Buttons")]
    [SerializeField] private Button _openBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private GameObject _alertIcon;

    private RectTransform _rect;

    private bool _isOpen = false;

    public override void Initialize()
    {
        _rect = GetComponent<RectTransform>();

        _openBtn.onClick.AddListener(Open);
        _closeBtn.onClick.AddListener(Close);

        UpdateOpenCloseButtons();
    }

    private void UpdateOpenCloseButtons()
    {
        _openBtn.gameObject.SetActive(!_isOpen);
        _closeBtn.gameObject.SetActive(_isOpen);
    }

    private void Open()
    {
        if (_isOpen)
        {
            return;
        }

        _rect.DOAnchorPosX(_rect.sizeDelta.x, 0.2f).OnComplete(() =>
        {
            _isOpen = true;
            UpdateOpenCloseButtons();
        });
    }

    private void Close()
    {
        if (!_isOpen)
        {
            return;
        }

        _rect.DOAnchorPosX(0f, 0.2f).OnComplete(() =>
        {
            _isOpen = false;
            UpdateOpenCloseButtons();
        });
    }

    public void SetAlert(bool isActive)
    {
        _alertIcon.SetActive(isActive);
    }
}
