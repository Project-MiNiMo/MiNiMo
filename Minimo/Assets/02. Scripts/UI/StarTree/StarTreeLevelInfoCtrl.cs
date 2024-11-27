using UnityEngine;
using TMPro;

public class StarTreeLevelInfoCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _limitTimeTMP;
    [SerializeField] private TextMeshProUGUI _starCoinTMP;
    [SerializeField] private TextMeshProUGUI _expTMP;
    [SerializeField] private TextMeshProUGUI _hpiTMP;
    [SerializeField] private TextMeshProUGUI _visitMinimoLimitTMP;

    public void Initialize(int level)
    {
        if (!App.GetData<TitleData>().StarTree.TryGetValue(level, out var data))
        {
            Debug.LogError($"Can't Find StarTreeData with ID : {level}");
            return;
        }

        _limitTimeTMP.text = ConvertSecondsToTime(data.LimitTime);
        _starCoinTMP.text = App.GetData<TitleData>().GetFormatString("STR_STARTREE_INFO_PERHOUR", data.StarCoin.ToString());
        _expTMP.text = App.GetData<TitleData>().GetFormatString("STR_STARTREE_INFO_PERHOUR", data.EXP.ToString());
        _hpiTMP.text = data.HPI.ToString();
        _visitMinimoLimitTMP.text = data.VisitMinimoLimit.ToString();
    }

    private string ConvertSecondsToTime(int time)
    {
        int hours = time / 3600;
        int minutes = (time % 3600) / 60;
        int seconds = time % 60;

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}
