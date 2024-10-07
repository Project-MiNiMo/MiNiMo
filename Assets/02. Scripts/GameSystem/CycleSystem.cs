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

    private const float CYCLE_LENGTH = 24f;
    private float _lastUpdateTime = 0f;
    private float _time;

    private int _currentIndex = 0;

    private void Start()
    {
        Array.Sort(_marks, (a, b) => a.timeRatio.CompareTo(b.timeRatio));

        UpdateTime();
        FindCurrentIndex();
        UpdateLight(0);
    }

    private void Update()
    {
        if (Time.time - _lastUpdateTime >= 60f)
        {
            _lastUpdateTime = Time.time;

            UpdateTime();
            FindCurrentIndex();
            UpdateLight(CalculateTransitionFactor());
        }
    }

    private void UpdateTime()
    {
        DateTime now = DateTime.Now;

        _time = (now.Hour + now.Minute / 60f) / CYCLE_LENGTH;
    }

    private void FindCurrentIndex()
    {
        for (int i = 0; i < _marks.Length; i++)
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
        CycleMark currentMark = _marks[_currentIndex];
        CycleMark nextMark = _marks[(_currentIndex + 1) % _marks.Length];

        float currentTime = currentMark.timeRatio;
        float nextTime = nextMark.timeRatio;

        if (nextTime < currentTime)
        {
            nextTime += 1f;
        }

        float timeSinceCurrent = _time - currentTime;
        if (timeSinceCurrent < 0)
        {
            timeSinceCurrent += 1f;
        }

        float duration = nextTime - currentTime;
        return Mathf.Clamp01(timeSinceCurrent / duration);
    }

    private void UpdateLight(float updateTime)
    {
        CycleMark currentMark = _marks[_currentIndex];
        CycleMark nextMark = _marks[(_currentIndex + 1) % _marks.Length];

        _globalLight.color = Color.Lerp(currentMark.color, nextMark.color, updateTime);
        _globalLight.intensity = Mathf.Lerp(currentMark.intensity, nextMark.intensity, updateTime);
    }
}
