using UnityEngine;
using TMPro;

public class PlayerInfoPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI _nicknameTMP;
    [SerializeField] private TextMeshProUGUI _levelTMP;

    private const string PLAYER_NAME = "¥Ÿ¿∫";
    private const int PLAYER_LEVEL = 5;

    public override void Initialize()
    {
        var titleData = App.GetData<TitleData>();

        _nicknameTMP.text = PLAYER_NAME;
        _levelTMP.text = titleData.GetFormatString("STR_PROFILE_LEVEL", PLAYER_LEVEL.ToString());
    }
}
