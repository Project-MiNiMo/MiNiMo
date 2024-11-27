using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilePanel : UIBase
{
    [SerializeField] private Button _openBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private GameObject _profileBack;

    [SerializeField] private Image _playerImg;
    [SerializeField] private TextMeshProUGUI _levelTMP;

    [SerializeField] private TextMeshProUGUI _expTMP;
    [SerializeField] private Image _expProgressBarImg;

    [SerializeField] private Image _constellationImg;

    [SerializeField] private TextMeshProUGUI _followerTMP;
    [SerializeField] private TextMeshProUGUI _followerDescTMP;
    [SerializeField] private TextMeshProUGUI _visitorTMP;
    [SerializeField] private TextMeshProUGUI _visitorDescTMP;
    [SerializeField] private TextMeshProUGUI _friendTMP;
    [SerializeField] private TextMeshProUGUI _friendDescTMP;

    #region Temp - PlayerInfo
    private const int PLAYER_LEVEL = 5;
    private const int PLAYER_EXP = 33;
    private const int PLAYER_EXP_MAX = 100;
    private const int PLAYER_FOLLOWER = 5;
    private const int PLAYER_VISITOR = 10;
    private const int PLAYER_FRIEND = 15;
    #endregion

    #region Override
    public override void Initialize()
    {
        var titleData = App.GetData<TitleData>();

        _openBtn.onClick.AddListener(OpenPanel);
        _closeBtn.onClick.AddListener(ClosePanel);

        _levelTMP.text = titleData.GetFormatString("STR_PROFILE_LEVEL", PLAYER_LEVEL.ToString());
        _expTMP.text = titleData.GetFormatString("STR_PROFILE_EXP", PLAYER_EXP.ToString(), PLAYER_EXP_MAX.ToString());
        UpdateExpProgressBar();

        _followerDescTMP.text = titleData.GetString("STR_PROFILE_FOLLOWER");
        _visitorDescTMP.text = titleData.GetString("STR_PROFILE_VISITOR");
        _friendDescTMP.text = titleData.GetString("STR_PROFILE_FRIEND");

        _followerTMP.text = PLAYER_FOLLOWER.ToString();
        _visitorTMP.text = PLAYER_VISITOR.ToString();
        _friendTMP.text = PLAYER_FRIEND.ToString();
    }

    public override void OpenPanel()
    {
        if (IsAddUIStack && !_profileBack.activeSelf)
        {
            App.GetManager<UIManager>().PushUIState(UIState);
        }

        _profileBack.SetActive(true);
    }

    public override void ClosePanel()
    {
        if (IsAddUIStack && _profileBack.activeSelf)
        {
            App.GetManager<UIManager>().PopUIState(UIState);
        }

        _profileBack.SetActive(false);
    }
    #endregion

    private void UpdateExpProgressBar()
    {
        _expProgressBarImg.fillAmount = (float)PLAYER_EXP / PLAYER_EXP_MAX;
    }
}
