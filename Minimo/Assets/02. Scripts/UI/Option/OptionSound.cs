using UnityEngine;
using TMPro;

//TODO : Connect with Wwise
public class OptionSound : OptionBase
{
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private ToggleOption _bgm;
    [SerializeField] private ToggleOption _sfx;

    #region Override
    protected override void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _titleTMP.text = titleData.GetString("STR_OPTION_GAMESETTING_SOUND");
        _bgm.Title.text = titleData.GetString("STR_OPTION_GAMESETTING_SOUND_BGM");
        _sfx.Title.text = titleData.GetString("STR_OPTION_GAMESETTING_SOUND_SFX");

        _bgm.LeftToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_ON");
        _bgm.RightToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_OFF");

        _sfx.LeftToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_ON");
        _sfx.RightToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_OFF");
    }

    protected override void SetEvent()
    {
        _bgm.LeftToggle.onValueChanged.AddListener((bool isOn) => OnBGMChange(isOn));
        _sfx.LeftToggle.onValueChanged.AddListener((bool isOn) => OnSFXChange(isOn));
    }

    protected override void SetValueFromData()
    {
        _bgm.LeftToggle.SetIsOnWithoutNotify(_setting.Sound.BGM);
        OnBGMChange(_setting.Sound.BGM, false);

        _sfx.LeftToggle.SetIsOnWithoutNotify(_setting.Sound.SFX);
        OnSFXChange(_setting.Sound.SFX, false);
    }

    public override void SaveOption()
    {
        _setting.Sound = new()
        {
            BGM = _bgm.LeftToggle.isOn,
            SFX = _sfx.LeftToggle.isOn
        };
    }
    #endregion

    private void OnBGMChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            //TODO : Set BGM On/Off
        }

        _bgm.LeftToggle.interactable = !isOn;
        _bgm.RightToggle.interactable = isOn; 
    }

    private void OnSFXChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            //TODO : Set SFX On/Off
        }

        _sfx.LeftToggle.interactable = !isOn;
        _sfx.RightToggle.interactable = isOn; 
    }
}
