using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingStarType
{
    Green,
    Blue,
    Yellow,
    Rainbow,
}

public class ShootingStarCtrl : MonoBehaviour
{
    private readonly List<ShootingStar> _activeStars = new();
    private readonly DateTime _resetTime = new(1, 1, 1, 6, 0, 0);  // ∏≈¿œ 06:00 KST

    private int _maxStarCount;

    private void Start()
    {
        _maxStarCount = App.GetData<TitleData>().Common["MaxStarLimit"].Value;
        var starObj = Resources.Load<GameObject>("Star");

        for (int i = 0; i < _maxStarCount; i++)
        {
            var star = Instantiate(starObj).GetComponent<ShootingStar>();

            _activeStars.Add(star);
        }
    }

    private void Update()
    {
        
    }
}