using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarTreePanel : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _boostBtn;
    [SerializeField] private Button _rewardBtn;
    [SerializeField] private Button _descriptionBtn;

    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _timerTMP;

    private TimeManager _timeManager;
    private DateTime _lastSpawnTime;
    private float _checkTimer = 0f;
    private const float CHECK_INTERVAL = 1f;

    private bool _isShowTitle = true;
    private string _titleString;
    private string _descriptionString;

    #region Override
    public override void Initialize()
    {
        _timeManager = App.GetManager<TimeManager>();
        _lastSpawnTime = _timeManager.Time; // Reset last spawn time to server time //TODO: Save and retrieve the initialization time for each star on the server

        SetString();
        SetButtonEvent();
    }

    public override void OpenPanel()
    {
        base.OpenPanel();

        UpdateTimerText();
        SetTitleText(true);
    }
    #endregion

    #region Initialize
    private void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _titleString = titleData.GetString("STR_STARTREE_TITLE");
        _descriptionString = titleData.GetString("STR_STARTREE_DESC");

        _boostBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_STARTREE_BOOST");
        _rewardBtn.GetComponentInChildren<TextMeshProUGUI>().text = titleData.GetString("STR_STARTREE_REWARD");
    }

    private void SetButtonEvent()
    {
        _closeBtn.onClick.AddListener(ClosePanel);
        _boostBtn.onClick.AddListener(OnClickBoostBtn);
        _rewardBtn.onClick.AddListener(OnClickRewardBtn);
        _descriptionBtn.onClick.AddListener(OnClickDescriptionBtn);
    }

    private void OnClickBoostBtn()
    {

    }

    private void OnClickRewardBtn()
    {
        _lastSpawnTime = _timeManager.Time;
    }

    private void OnClickDescriptionBtn()
    {
        SetTitleText();
    }
    #endregion

    private void Update()
    {
        if (!gameObject.activeInHierarchy) 
        {
            return;
        }

        _checkTimer += Time.deltaTime;
        if (_checkTimer >= CHECK_INTERVAL)
        {
            UpdateTimerText();
            _checkTimer = 0f;
        }
    }

    private void UpdateTimerText()
    {
        DateTime currentServerTime = _timeManager.Time; // Get current server time
        TimeSpan timeDifference = currentServerTime - _lastSpawnTime;

        _timerTMP.text = $"{timeDifference.Hours:00}:{timeDifference.Minutes:00}:{timeDifference.Seconds:00}";
    }

    private void SetTitleText()
    {
        _isShowTitle = !_isShowTitle;

        _titleTMP.text = _isShowTitle ? _titleString : _descriptionString;
    }

    private void SetTitleText(bool isShowTitle)
    {
        if (_isShowTitle == isShowTitle) 
        {
            return;
        }

        _isShowTitle = isShowTitle;

        _titleTMP.text = _isShowTitle ? _titleString : _descriptionString;
    }
}
