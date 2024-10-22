using UnityEngine;
using TMPro;

public class OptionScreen : OptionBase
{
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private ToggleOption _frame;
    [SerializeField] private ToggleOption _graphics;

    #region Override
    protected override void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _titleTMP.text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN");
        _frame.Title.text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN_FRAME");
        _graphics.Title.text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN_GRAPHICS");

        _frame.LeftToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN_FRAME_60");
        _frame.RightToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN_FRAME_30");

        _graphics.LeftToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN_GRAPHICS_DEFAULT");
        _graphics.RightToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_SCREEN_GRAPHICS_LOW");
    }

    protected override void SetEvent()
    {
        _frame.LeftToggle.onValueChanged.AddListener((bool isOn) => OnFrameChange(isOn));
        _graphics.LeftToggle.onValueChanged.AddListener((bool isOn) => OnGraphicsChange(isOn));
    }

    protected override void SetValueFromData()
    {
        _frame.LeftToggle.SetIsOnWithoutNotify(_setting.Screen.Frame);
        OnFrameChange(_setting.Screen.Frame, false);

        _graphics.LeftToggle.SetIsOnWithoutNotify(_setting.Screen.Graphics);
        OnGraphicsChange(_setting.Screen.Graphics, false);
    }

    public override void SaveOption()
    {
        _setting.Screen = new()
        {
            Frame = _frame.LeftToggle.isOn,
            Graphics = _graphics.LeftToggle.isOn
        };
    }
    #endregion

    private void OnFrameChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            Application.targetFrameRate = isOn ? 60 : 30;
        }

        _frame.LeftToggle.interactable = !isOn;
        _frame.RightToggle.interactable = isOn;
    }

    private void OnGraphicsChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            //TODO : Set Graphics Settings (ex. On/Off Shaders, Lights)
        }

        _graphics.LeftToggle.interactable = !isOn;
        _graphics.RightToggle.interactable = isOn;
    }
}
