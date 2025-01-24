using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SidePanel : UIBase
{
    [Header("Buttons")]
    [SerializeField] private Button _openBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _closeBtn2;
    [SerializeField] private GameObject _alertIcon;

    private RectTransform _rect;

    public bool IsOpen { get; private set; }

    public override void Initialize()
    {
        _rect = GetComponent<RectTransform>();

        _openBtn.onClick.AddListener(Open);
        _closeBtn.onClick.AddListener(Close);
        _closeBtn2.onClick.AddListener(Close);

        UpdateOpenCloseButtons();
    }

    private void UpdateOpenCloseButtons()
    {
        _openBtn.gameObject.SetActive(!IsOpen);
        _closeBtn.gameObject.SetActive(IsOpen);
    }

    private void Open()
    {
        if (IsOpen)
        {
            return;
        }

        SetAlert(false);
        
        _rect.DOAnchorPosX(_rect.sizeDelta.x, 0.2f).OnComplete(() =>
        {
            IsOpen = true;
            UpdateOpenCloseButtons();
        });
    }

    public void Close()
    {
        if (!IsOpen)
        {
            return;
        }

        SetAlert(false);
        
        _rect.DOAnchorPosX(0f, 0.2f).OnComplete(() =>
        {
            IsOpen = false;
            UpdateOpenCloseButtons();
        });
    }

    public void SetAlert(bool isActive)
    {
        if (IsOpen) return;
        
        _alertIcon.SetActive(isActive);
    }
}
