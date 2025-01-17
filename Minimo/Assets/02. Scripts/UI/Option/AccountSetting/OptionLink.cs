using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionLink : OptionBase
{
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private Button _linkBtn;
    
    protected override void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _titleTMP.text = titleData.GetString("STR_OPTION_ACCOUNTSETTING_LINK");
        _linkBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_ACCOUNTSETTING_LINK_DESC");
    }
    
    protected override void SetEvent()
    {
        _linkBtn.onClick.AddListener(OnClickLink);
    }

    protected override void SetValueFromData() { }

    public override void SaveOption() { }

    private void OnClickLink() { }
}
