using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;


public class TimeManager : ManagerBase
{
    private GameClient _gameClient;

    [SerializeField] private string targetTime = "";
    
    public DateTime Time => DateTime.UtcNow + _timeOffset + _timeZoneOffset;
    public bool IsProcessing { get; private set; } = false;
    
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

    public void Init(DateTime serverTime)
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
        SetTimeZoneOffset();
        SyncTime(serverTime);
        InvokeRepeating(nameof(SyncTime), (float)_syncInterval.TotalSeconds, (float)_syncInterval.TotalSeconds);
        InvokeRepeating(nameof(ValidateTime), (float)_validateInterval.TotalSeconds, (float)_validateInterval.TotalSeconds);
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
    private void SyncTime(DateTime serverTime)
    {
        var localTime = DateTime.UtcNow;
        _timeOffset = serverTime - localTime;
        _lastTime = Time;
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
        // space for other UI elements
        GUILayout.Space(500);
        
        // input field for target time
        targetTime = GUI.TextField(new Rect(10, 70, 200, 20), targetTime);
        
        // send put request to server
        if (GUI.Button(new Rect(10, 10, 100, 30), "Set Time"))
        {
            SetCheatTime(targetTime);
        }
        
        // Init Button
        if (GUI.Button(new Rect(10, 40, 100, 30), "Init"))
        {
            Init(DateTime.UtcNow);
        }
    }

    private async void SetCheatTime(string targetTime)
    {
        if (IsProcessing == false)
        {
            Debug.LogError("TimeManager is not processing");
            return;
        }
        
        try
        {
            if (DateTime.TryParse(targetTime, out var time))
            {
                await _gameClient.PutAsync("api/time", time);
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
