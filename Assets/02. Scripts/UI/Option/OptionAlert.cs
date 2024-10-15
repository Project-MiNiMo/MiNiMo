using UnityEngine;
using TMPro;

public class OptionAlert : OptionBase
{
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private ToggleOption _push;

    #region Override
    protected override void SetString()
    {
        var titleData = App.Instance.GetData<TitleData>();

        _titleTMP.text = titleData.GetString("STR_OPTION_GAMESETTING_ALERT");
        _push.Title.text = titleData.GetString("STR_OPTION_GAMESETTING_ALERT_PUSH");

        _push.LeftToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_ON");
        _push.RightToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_OFF");
    }

    protected override void SetEvent()
    {
        _push.LeftToggle.onValueChanged.AddListener((bool isOn) => OnPushChange(isOn));
    }

    protected override void SetValueFromData()
    {
        _push.LeftToggle.SetIsOnWithoutNotify(_setting.Alert.Push);
        OnPushChange(_setting.Alert.Push, false);
    }

    public override void SaveOption()
    {
        _setting.Alert = new()
        {
            Push = _push.LeftToggle.isOn
        };
    }
    #endregion

    private void OnPushChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            //TODO : Set PushAlert On/Off
        }

        _push.LeftToggle.interactable = !isOn;
        _push.RightToggle.interactable = isOn;
    }
}
