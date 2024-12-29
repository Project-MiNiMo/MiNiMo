using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemainTimeUpdater : MonoBehaviour
{
    [SerializeField] private Image _remainTimeImg;
    [SerializeField] private TextMeshProUGUI _remainTimeTMP;
    
    private string _emptyString;
    private string _pendingString;
    private string _completeString;
    
    private void Start()
    {
        _remainTimeTMP.text = string.Empty;
        
        _emptyString = App.GetData<TitleData>().GetString("STR_FACILITY_SLOTSTATE_DESC1");
        _pendingString = App.GetData<TitleData>().GetString("STR_FACILITY_PRODSTATE_DESC2");
        _completeString = App.GetData<TitleData>().GetString("STR_FACILITY_PRODSTATE_DESC3");
    }
    
    public void SetRemainTime(int remainTime, int fullTime)
    {
        if (remainTime <= 0)
        {
            gameObject.SetActive(false);
        }

        _remainTimeTMP.text = FormatTime(remainTime);
        _remainTimeImg.fillAmount = 1 - ((float)remainTime / fullTime);
    }
    
    private string FormatTime(int time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public void SetEmpty()
    {
        _remainTimeTMP.text = _emptyString;
        _remainTimeImg.fillAmount = 0;
    }
    
    public void SetPending()
    {
        _remainTimeTMP.text = _pendingString;
        _remainTimeImg.fillAmount = 0;
    }
    
    public void SetComplete()
    {
        _remainTimeTMP.text = _completeString;
        _remainTimeImg.fillAmount = 1;
    }
}
