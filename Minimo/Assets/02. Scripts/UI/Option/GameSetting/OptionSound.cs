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
        _bgm.LeftToggle.onValueChanged.AddListener((isOn) => OnBGMChange(isOn));
        _sfx.LeftToggle.onValueChanged.AddListener((isOn) => OnSFXChange(isOn));
    }

    protected override void SetValueFromData()
    {
        OnBGMChange(_setting.Sound.BGM, false);
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
        
        _bgm.LeftToggle.SetIsOnWithoutNotify(isOn);
        _bgm.RightToggle.SetIsOnWithoutNotify(!isOn);

        _bgm.LeftToggle.interactable = !isOn;
        _bgm.RightToggle.interactable = isOn; 
    }

    private void OnSFXChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            //TODO : Set SFX On/Off
        }
        
        _sfx.LeftToggle.SetIsOnWithoutNotify(isOn);
        _sfx.RightToggle.SetIsOnWithoutNotify(!isOn);

        _sfx.LeftToggle.interactable = !isOn;
        _sfx.RightToggle.interactable = isOn; 
    }
}
