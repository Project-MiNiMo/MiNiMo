using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TaskState
{
    Empty,
    Pending,
    Produce,
    Complete,
}

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
        
        var titleData = App.GetData<TitleData>();
        _emptyString = titleData.GetString("STR_PRODUCE_SLOTSTATE_EMPTY");
        _pendingString = titleData.GetString("STR_PRODUCE_SLOTSTATE_PENDING");
        _completeString = titleData.GetString("STR_PRODUCE_SLOTSTATE_COMPLETE");
    }
    
    public void SetRemainTime(int remainTime, int fullTime)
    {
        _remainTimeTMP.text = FormatTime(remainTime);
        _remainTimeImg.fillAmount = 1 - ((float)remainTime / fullTime);
    }
    
    private string FormatTime(int time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public void SetFillAmount(int fillAmount)
    {
        _remainTimeImg.fillAmount = fillAmount;
    }
    
    public void SetRemainText(TaskState state)
    {
        switch (state)
        {
            case TaskState.Empty:
                _remainTimeTMP.text = _emptyString;
                break;
            
            case TaskState.Pending:
                _remainTimeTMP.text = _pendingString;
                break;
            
            case TaskState.Produce:
                break;
            
            case TaskState.Complete:
                _remainTimeTMP.text = _completeString;
                break;
        }
    }
}
