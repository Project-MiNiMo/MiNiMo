using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceTaskBtn : MonoBehaviour
{
    [SerializeField] private Button _taskBtn;
    [SerializeField] private TextMeshProUGUI _taskText;
    
    private ProduceTask _produceTask;
    
    [SerializeField] private Image _remainTimeImg;
    [SerializeField] private TextMeshProUGUI _remainTimeTMP;

    private int _currentRemainTime;
    
    public void Initialize(ProduceTask produceTask)
    {
        if (produceTask == null)
        {
            return;
        }
        _produceTask = produceTask;
        SetRemainTime(produceTask.RemainTime);
    }
    
    private void Update()
    {
        if (_produceTask == null) 
        {
            return;
        }
        
        if (_currentRemainTime != _produceTask.RemainTime)
        {
            _currentRemainTime = _produceTask.RemainTime;
            SetRemainTime(_currentRemainTime);
        }
    }

    private void SetRemainTime(int remainTime)
    {
        if (remainTime <= 0)
        {
            gameObject.SetActive(false);
        }

        _remainTimeTMP.text = FormatTime(remainTime);
        _remainTimeImg.fillAmount = 1 - ((float)remainTime / _produceTask.Data.Time);
    }
    
    private string FormatTime(int time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
}
