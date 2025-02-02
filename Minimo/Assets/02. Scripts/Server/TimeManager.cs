﻿using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UnityEngine;


public class TimeManager : ManagerBase
{
    private GameClient _gameClient;

    [SerializeField] private string targetTime = "";

    public DateTime Time => DateTime.UtcNow + _timeOffset;
    public DateTime LocalTime => DateTime.UtcNow + _timeOffset + _timeZoneOffset;
    public TimeSpan TimeZoneOffset => _timeZoneOffset;
    public ReactiveProperty<DateTime> TimeReactiveProperty { get; } = new ReactiveProperty<DateTime>();
    public bool IsProcessing { get; private set; } = false;
    
    // 서버 시간
    private TimeSpan _timeOffset;
    private TimeSpan _timeZoneOffset;
    private readonly TimeSpan _syncInterval = TimeSpan.FromSeconds(10);
    
    // 클라이언트 시간 검증
    private DateTime _lastTime;
    private readonly TimeSpan _maxTimeDifference = TimeSpan.FromSeconds(5);
    private readonly TimeSpan _validateInterval = TimeSpan.FromSeconds(1);
    private bool _isValidating;

    public void Init(DateTime serverTime)
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
        SetTimeZoneOffset();
        SyncTime(serverTime);
        InvokeRepeating(nameof(SyncTime), (float)_syncInterval.TotalSeconds, (float)_syncInterval.TotalSeconds);
        InvokeRepeating(nameof(ValidateTime), (float)_validateInterval.TotalSeconds, (float)_validateInterval.TotalSeconds);
        InvokeRepeating(nameof(TimePulse), 0, 1f);
        IsProcessing = true;
    }
    
    public void Stop()
    {
        CancelInvoke();
        IsProcessing = false;
    }

    private void SetTimeZoneOffset()
    {
        _timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
    }
    
    // 첫 로그인시 서버 시간을 받아옴
    public void SyncTime(DateTime serverTime)
    {
        var localTime = DateTime.UtcNow;
        _timeOffset = serverTime - localTime;
        _lastTime = Time;
    }
    
    private async UniTask SyncTime()
    {
        if (_isValidating)
        {
            return;
        }

        var result = await _gameClient.GetAsync<DateTime> ("api/time");
        if(result.IsSuccess && result.Data is {} serverTime)
        {
            SyncTime(serverTime);
        }
        else
        {
            Debug.LogError($"Failed to sync time: {result.Message}");
        }
    }

    private void ValidateTime()
    {
        if (_isValidating)
        {
            return;
        }

        _isValidating = true;
        
        var currentTime = Time;
        var timeDifference = currentTime - _lastTime;
        bool isTimeValid = timeDifference.Duration() <= _maxTimeDifference && timeDifference >= TimeSpan.Zero;

        if (isTimeValid)
        {
            _lastTime = currentTime;
            _isValidating = false;
        }
        else
        {
            try
            {
                Debug.LogWarning($"Validation failed: current time: {currentTime}, last time: {_lastTime}");
                SyncTime().GetAwaiter().OnCompleted(() => { _isValidating = false; });
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to validate time: {ex.Message}");
                _isValidating = false;
            }
        }
    }

    private void TimePulse()
    {
        if (_isValidating)
        {
            return;
        }
        TimeReactiveProperty.Value = Time;
    }
}
