using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomPanel : UIBase
{
    [SerializeField] private Button _mainBtn;
    [SerializeField] private Button _friendBtn;
    [SerializeField] private Button _minimoBtn;
    [SerializeField] private Button _buildingBtn;
    [SerializeField] private Button _storageBtn;

    public override void Initialize()
    {
        SetString();
        SetButtonEvent();
    }

    private void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _friendBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_MAIN_BOTTOM_FRIEND");
        _minimoBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_MAIN_BOTTOM_MINIMO");
        _buildingBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_MAIN_BOTTOM_BUILDING");
        _storageBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_MAIN_BOTTOM_STORAGE");
    }

    private void SetButtonEvent()
    {
        _mainBtn.onClick.AddListener(OnClickMainBtn);
        _friendBtn.onClick.AddListener(OnClickFriendBtn);
        _minimoBtn.onClick.AddListener(OnClickMinimoBtn);
        _buildingBtn.onClick.AddListener(OnClickBuildingBtn);
        _storageBtn.onClick.AddListener(OnClickStorageBtn);
    }

    private void OnClickMainBtn()
    {

    }

    private void OnClickFriendBtn()
    {

    }

    private void OnClickMinimoBtn()
    {

    }

    private void OnClickBuildingBtn()
    {
        App.GetManager<UIManager>().GetPanel<BuildingPanel>().OpenPanel();
    }

    private void OnClickStorageBtn()
    {
        App.GetManager<UIManager>().GetPanel<StoragePanel>().OpenPanel();
    }
}
