
using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;


public class TimeManager : ManagerBase
{
    private GameClient _gameClient;

    [SerializeField] private string targetTime = "";
    
    public DateTime Time => DateTime.UtcNow + _timeOffset + _timeZoneOffset;
    
    // 서버 시간
    private TimeSpan _timeOffset;
    private TimeSpan _timeZoneOffset;
    private readonly TimeSpan _syncInterval = TimeSpan.FromSeconds(10);
    private bool _isSyncing;
    
    // 클라이언트 시간 검증
    private DateTime _lastTime;
    private readonly TimeSpan _maxTimeDifference = TimeSpan.FromSeconds(5);
    private readonly TimeSpan _validateInterval = TimeSpan.FromSeconds(1);
    private bool _isValidating;

    public void Init()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
        SetTimeZoneOffset();
        InvokeRepeating(nameof(SyncTime), 0, (float)_syncInterval.TotalSeconds);
        InvokeRepeating(nameof(ValidateTime), 0, (float)_validateInterval.TotalSeconds);
    }

    private void SetTimeZoneOffset()
    {
        _timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
    }
    
    private async UniTask SyncTime()
    {
        if (_isSyncing)
        {
            return;
        }

        _isSyncing = true;
        try
        {
            var serverTime = await _gameClient.GetAsync<DateTime> ("api/time");
            Debug.Log($"Server time: {serverTime}");
            var localTime = DateTime.UtcNow;
            _timeOffset = serverTime - localTime;
            _lastTime = Time;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to sync time: {ex.Message}");
        }
        finally
        {
            _isSyncing = false;
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
        try
        {
            if (timeDifference.Duration() > _maxTimeDifference || timeDifference < TimeSpan.Zero)
            {
                Debug.LogWarning($"Validation failed: current time: {currentTime}, last time: {_lastTime}");
                SyncTime().GetAwaiter().OnCompleted(() =>
                {
                    _isValidating = false;
                });
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to validate time: {ex.Message}");
        }
        finally
        {
            _lastTime = currentTime;
            _isValidating = false;
        }
    }
    
#if UNITY_EDITOR
    private void OnGUI()
    {
        // send put request to server
        if (GUI.Button(new Rect(10, 10, 100, 50), "Set Time"))
        {
            SetCheatTime();
        }
    }

    private async void SetCheatTime()
    {
        try
        {
            if (DateTime.TryParse(targetTime, out var time))
            {
                await _gameClient.PutAsync<DateTime>("api/time", time);
                await SyncTime();
            }
            else
            {
                Debug.LogError("Invalid time format");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to set cheat time: {ex.Message}");
        }
    }
#endif
}
