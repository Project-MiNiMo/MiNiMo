using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageBtn : MonoBehaviour
{
    public Item Item { get; private set; }
    public Vector2 AnchoredPosition { get; private set; }
    
    [SerializeField] private Button _infoBtn;
    
    [SerializeField] private Image _iconImg;
    [SerializeField] private TextMeshProUGUI _countTMP;

    private StorageInfoPanel _infoPanel;

    private void Start()
    {
        _infoPanel= App.GetManager<UIManager>().GetPanel<StorageInfoPanel>();
        
        _infoBtn.onClick.AddListener(OnClickInfoBtn);
    }

    private void OnEnable()
    {
        if (Item?.Count <= 0)
        {
            gameObject.SetActive(false);
        }

        SetCountText();
    }

    public void Initialize(Item item)
    {
        Item = item;
        AnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        
        _iconImg.sprite = item.Icon;
    }
    
    private void OnClickInfoBtn()
    {
        _infoPanel.OpenPanel(this);
    }

    private void SetCountText()
    {
        _countTMP.text = Item?.Count.ToString();
    }
}
