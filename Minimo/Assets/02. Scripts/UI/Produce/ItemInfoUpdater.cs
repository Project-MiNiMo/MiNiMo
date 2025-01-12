using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _infoTMP;
    [SerializeField] private Image[] _infoImages;
    
    private TitleData _titleData;
    
    private ProduceOption _currentOption;
    
    private void Start()
    {
        _titleData = App.GetData<TitleData>();
    }

    public void SetTaskItem(ProduceTask produceTask)
    {
        _currentOption = produceTask.Data;
        
        SetInfo();
    }
    
    private void SetInfo()
    {
        var i = 0;
        
        for (; i < _currentOption.Results.Length; i++) 
        {
            if (!_titleData.Item.TryGetValue(_currentOption.Results[i].Code, out var itemData))
            {
                Debug.LogError($"Cannot find item data with code : {_currentOption.Results[i].Code}");
                return;
            }
            
            _infoTMP[i].text = $"X{_currentOption.Results[i].Amount}";
            _infoImages[i].sprite = Resources.Load<Sprite>($"Item/{itemData.ID}");
        }

        for (; i < _infoImages.Length; i++) 
        {
            _infoImages[i].gameObject.SetActive(false);
        }
    }

    public void SetItemEmpty()
    {
        _infoTMP[0].text = string.Empty;
        _infoImages[0].sprite = null;
    }
}
