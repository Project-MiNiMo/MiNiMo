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

    public void InitItemButtons(GridObject gridObject)
    {
        if (!App.GetData<TitleData>().Produce.TryGetValue(gridObject.Data.ID, out var produceData))
        {
            Debug.LogError($"Can't find ProduceData with ID : {gridObject.Data.ID}");
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            var itemID = i switch
            {
                0 => produceData.Fst_ResultCode,
                1 => produceData.Snd_ResultCode,
                2 => produceData.Trd_ResultCode,
                _ => string.Empty,
            };

            if (string.IsNullOrEmpty(itemID))
            {
                _itemBtns[i].gameObject.SetActive(false);
                continue;
            }

            _itemBtns[i].gameObject.SetActive(true);
            _itemBtns[i].Initialize(gridObject, itemID);
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
