using System;

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CycleSystem : MonoBehaviour
{
    [Serializable]
    public struct CycleMark
    {
        public float timeRatio; // 0.0 ~ 1.0 (00:00 ~ 24:00)
        public Color color;
        public float intensity;
    }

    [SerializeField] private CycleMark[] _marks;
    [SerializeField] private Light2D _globalLight;

    private TimeManager _timeManager;
    
    private const float CYCLE_LENGTH = 24f;
    private DateTime _lastUpdateTime;
    private float _time;

    private int _currentIndex = 0;

    private void Start()
    {
        _timeManager = App.GetManager<TimeManager>();
        
        Array.Sort(_marks, (a, b) => a.timeRatio.CompareTo(b.timeRatio));

        _lastUpdateTime = _timeManager.Time;
        
        UpdateTime();
        FindCurrentIndex();
        UpdateLight(CalculateTransitionFactor());
    }

    private void Update()
    {
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds < 1f) return;
        
        _lastUpdateTime = _timeManager.Time;

        UpdateTime();
        FindCurrentIndex();
        UpdateLight(CalculateTransitionFactor());
    }

    private void UpdateTime()
    {
        var now = _timeManager.Time;

        _time = (now.Hour + now.Minute / 60f) / CYCLE_LENGTH;
    }

    private void FindCurrentIndex()
    {
        for (var i = 0; i < _marks.Length; i++)
        {
            if (_time < _marks[i].timeRatio)
            {
                _currentIndex = (i - 1 + _marks.Length) % _marks.Length;
                return;
            }
        }

        _currentIndex = _marks.Length - 1;
    }

    private float CalculateTransitionFactor()
    {
        var currentMark = _marks[_currentIndex];
        var nextMark = _marks[(_currentIndex + 1) % _marks.Length];

        var currentTime = currentMark.timeRatio;
        var nextTime = nextMark.timeRatio;

        if (nextTime < currentTime)
        {
            nextTime += 1f;
        }

        var timeSinceCurrent = _time - currentTime;
        if (timeSinceCurrent < 0)
        {
            timeSinceCurrent += 1f;
        }

        var duration = nextTime - currentTime;
        return Mathf.Clamp01(timeSinceCurrent / duration);
    }

    private void UpdateLight(float updateTime)
    {
        var currentMark = _marks[_currentIndex];
        var nextMark = _marks[(_currentIndex + 1) % _marks.Length];

        _globalLight.color = Color.Lerp(currentMark.color, nextMark.color, updateTime);
        _globalLight.intensity = Mathf.Lerp(currentMark.intensity, nextMark.intensity, updateTime);
    }
}
