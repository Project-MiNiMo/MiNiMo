using System;

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CycleCtrl : MonoBehaviour
{
    [Serializable]
    public struct CycleMark
    {
        public float timeRatio; // 0.0 ~ 1.0 (00:00 ~ 24:00)
        public Color color;
        public float intensity;
    }

    [SerializeField] private CycleMark[] marks;
    [SerializeField] private Light2D globalLight;

    private const float cycleLength = 24f;
    private float lastUpdateTime = 0f;

    private int currentIndex = 0;

    private float time;

    private void Start()
    {
        Array.Sort(marks, (a, b) => a.timeRatio.CompareTo(b.timeRatio));

        UpdateTime();
        FindCurrentIndex();
        UpdateLight(0);
    }

    private void Update()
    {
        if (Time.time - lastUpdateTime >= 60f)
        {
            lastUpdateTime = Time.time;

            UpdateTime();
            FindCurrentIndex();
            UpdateLight(CalculateTransitionFactor());
        }
    }

    private void UpdateTime()
    {
        DateTime now = DateTime.Now;

        time = (now.Hour + now.Minute / 60f) / cycleLength;
    }

    private void FindCurrentIndex()
    {
        for (int i = 0; i < marks.Length; i++)
        {
            if (time < marks[i].timeRatio)
            {
                currentIndex = (i - 1 + marks.Length) % marks.Length;
                return;
            }
        }

        currentIndex = marks.Length - 1;
    }

    private float CalculateTransitionFactor()
    {
        CycleMark currentMark = marks[currentIndex];
        CycleMark nextMark = marks[(currentIndex + 1) % marks.Length];

        float currentTime = currentMark.timeRatio;
        float nextTime = nextMark.timeRatio;

        if (nextTime < currentTime)
            nextTime += 1f;

        float timeSinceCurrent = time - currentTime;
        if (timeSinceCurrent < 0)
            timeSinceCurrent += 1f;

        float duration = nextTime - currentTime;
        return Mathf.Clamp01(timeSinceCurrent / duration);
    }

    private void UpdateLight(float t)
    {
        CycleMark currentMark = marks[currentIndex];
        CycleMark nextMark = marks[(currentIndex + 1) % marks.Length];

        globalLight.color = Color.Lerp(currentMark.color, nextMark.color, t);
        globalLight.intensity = Mathf.Lerp(currentMark.intensity, nextMark.intensity, t);
    }
}
