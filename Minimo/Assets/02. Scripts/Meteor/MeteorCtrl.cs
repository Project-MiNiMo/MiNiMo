using System;
using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MinimoShared;
using UnityEngine;

public class MeteorCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _meteorPrefab;
    [SerializeField] private InstallChecker _installChecker;
    [SerializeField] private TileStateModifier _tileStateModifier;
 
    private List<Meteor> _meteors;
    
    private TimeManager _timeManager;
    private MeteorManager _meteorManager;
    private DateTime _lastSpawnTime;
    private DateTime _lastUpdateTime;
    
    private bool _isSpawning = false;
    private float _spawnInterval;

    private void Start()
    {
        var commonData = App.GetData<TitleData>().Common;
        var maxStarLimit = commonData["MeteorLimit"];
        _spawnInterval = commonData["MeteorSpawnTime"];
        
        _meteors = new List<Meteor>(maxStarLimit);
        for (var i = 0; i < maxStarLimit; i++)
        {
            var meteor = Instantiate(_meteorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Meteor>();
            meteor.gameObject.SetActive(false);
            _meteors.Add(meteor);
            
        }
        
        _timeManager = App.GetManager<TimeManager>();
        _meteorManager = App.GetManager<MeteorManager>();
        _lastSpawnTime = _timeManager.Time; //TODO: Save and retrieve the initialization time for each star on the server
        _lastUpdateTime = _timeManager.Time;

        SetExistingMeteors().Forget();
    }

    private void Update()
    {
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds < 1f) return;
        
        _lastUpdateTime = _timeManager.Time;

        if (!CheckRemainingMeteor())
        {
            _lastSpawnTime = _timeManager.Time;
            return;
        }

        if (_isSpawning) return;
        
        CheckSpawnInterval().Forget();
    }
    
    private async UniTask SetExistingMeteors()
    {
        var result = await _meteorManager.GetMeteors();
        if (!result.IsSuccess) return;

        var spawnPositions = _installChecker.GetInstallablePositions();
        if (spawnPositions.Count == 0) return;
        
        foreach (var existMeteor in result.Data)
        {
            var meteor = _meteors.FirstOrDefault(star => !star.IsLanded);
            if (meteor == null) break;
            
            var meteorId = existMeteor.Id;
            var spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
            _tileStateModifier.ModifyTileState(spawnPosition, TileState.Installed);
            spawnPositions.Remove(spawnPosition);
            
            meteor.Land(meteorId, spawnPosition);
        }
    }

    private async UniTask CheckSpawnInterval()
    {
        _isSpawning = true;
        
        var timeDifference =  _timeManager.Time - _lastSpawnTime;
        
        if (timeDifference.TotalSeconds >= _spawnInterval)
        {
            await SpawnMeteor();
            _lastSpawnTime = _lastSpawnTime.AddSeconds(_spawnInterval);
        }
        
        _isSpawning = false;
    }
    
    private bool CheckRemainingMeteor()
    {
        return _meteors.Any(star => !star.IsLanded);
    }

    private async UniTask SpawnMeteor()
    {
        return;
        
        if (!CheckRemainingMeteor()) return;
        
        var spawnPositions = _installChecker.GetInstallablePositions();
        if (spawnPositions.Count == 0) return;
        
        var result = await _meteorManager.CreateMeteor();
        if (!result.IsSuccess) return;

        foreach (var createMeteor in result.Data.CreatedMeteors)
        {
            var meteor = _meteors.FirstOrDefault(star => !star.IsLanded);
            if (meteor == null) break;
            
            var meteorId = createMeteor.Id;
            var spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
            _tileStateModifier.ModifyTileState(spawnPosition, TileState.Installed);
            spawnPositions.Remove(spawnPosition);
            
            meteor.Land(meteorId, spawnPosition);
        } 
    }
}