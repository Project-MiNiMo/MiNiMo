using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageBtn : MonoBehaviour
{
    public Item Item { get; private set; }
    [SerializeField] private Button _infoBtn;
    
    [SerializeField] private Image _iconImg;
    [SerializeField] private TextMeshProUGUI _countTMP;

    private Item _item;
    private StorageInfoPanel _infoPanel;

    private void Start()
    {
        _infoPanel= App.GetManager<UIManager>().GetPanel<StorageInfoPanel>();
        
        _infoBtn.onClick.AddListener(OnClickInfoBtn);
    }

    public void Initialize(Item item)
    {
        _item = item;

        string spritePath = $"Item/Icon/{item.Icon}";
        _iconImg.sprite = Resources.Load<Sprite>(spritePath);
        _countTMP.text = 1.ToString();
    }
    
    private void OnClickInfoBtn()
    {
        _infoPanel.OpenPanel(_item);
    }
}
