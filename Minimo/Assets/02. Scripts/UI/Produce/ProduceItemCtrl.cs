using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProduceItemCtrl : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _openBtn;
    [SerializeField] private Button _closeBtn;

    private ProduceItemBtn[] _itemBtns;
    private RectTransform _rect;

    private bool _isOpen = true;

    private void OnEnable()
    {
        _isOpen = true;
        UpdateOpenCloseButtons();
    }

    public void Initialize()
    {
        _rect = GetComponent<RectTransform>();
        _itemBtns = GetComponentsInChildren<ProduceItemBtn>(true);

        _openBtn.onClick.AddListener(Open);
        _closeBtn.onClick.AddListener(Close);

        UpdateOpenCloseButtons();
    }

    public void InitItemButtons(string dataID)
    {
        int index = 0;

        if (!App.GetData<TitleData>().Produce.TryGetValue(dataID, out var produceData))
        {
            return;
        }

        
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

        _rect.DOAnchorPosY(0f, 0.5f).OnComplete(() =>
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

        _rect.DOAnchorPosY(-_rect.sizeDelta.y, 0.2f).OnComplete(() =>
        {
            _isOpen = false;
            UpdateOpenCloseButtons();
        });
    }
}
