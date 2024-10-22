using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionLanguage : OptionBase
{
    [SerializeField] private ToggleOption _language;

    [Header("Pop Up")]
    [SerializeField] private GameObject _popUpBack;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _contentTMP;
    [SerializeField] private Button _yesBtn;
    [SerializeField] private Button _noBtn;

    #region Override
    protected override void Start()
    {
        base.Start();

        _popUpBack.SetActive(false);
    }

    protected override void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _language.LeftToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_LANGUAGE_KOREAN");
        _language.RightToggle.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_LANGUAGE_ENGLISH");

        _titleTMP.text = titleData.GetString("STR_OPTION_GAMESETTING_LANGUAGE_POPUP");
        _contentTMP.text = titleData.GetString("STR_OPTION_GAMESETTING_LANGUAGE_POPUP_CONTENT");

        _yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_LANGUAGE_POPUP_YES");
        _noBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_OPTION_GAMESETTING_LANGUAGE_POPUP_NO");
    }

    protected override void SetEvent()
    {
        _language.LeftToggle.onValueChanged.AddListener((bool isOn) => OnLanguageChange(isOn));

        _yesBtn.onClick.AddListener(OnClickYes);
        _noBtn.onClick.AddListener(() => _popUpBack.SetActive(false));
    }

    protected override void SetValueFromData()
    {
        _language.LeftToggle.SetIsOnWithoutNotify(_setting.Language == SystemLanguage.Korean);
        OnLanguageChange(_setting.Language == SystemLanguage.Korean, false);
    }

    public override void SaveOption() { }
    #endregion

    private void OnLanguageChange(bool isOn, bool withNotify = true)
    {
        if (withNotify)
        {
            _language.LeftToggle.SetIsOnWithoutNotify(!isOn);
            _language.RightToggle.SetIsOnWithoutNotify(isOn);

            _popUpBack.SetActive(true);
        }
        else
        {
            _language.LeftToggle.interactable = !isOn;
            _language.RightToggle.interactable = isOn;
        }
    }

    private void OnClickYes()
    {
        _setting.Language = _language.LeftToggle.isOn ? SystemLanguage.English : SystemLanguage.Korean;

        App.LoadScene(SceneName.Developer);
    }
}