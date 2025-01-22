using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class MeteorCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _meteorPrefab;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private InstallChecker _installChecker;
    [SerializeField] private TileStateModifier _tileStateModifier;
 
    private List<Meteor> _meteors;
    
    private TimeManager _timeManager;
    private DateTime _lastSpawnTime;
    private DateTime _lastUpdateTime;

    private void Start()
    {
        var maxStarLimit = App.GetData<TitleData>().Common["MeteorLimit"];
        
        _meteors = new List<Meteor>(maxStarLimit);
        for (var i = 0; i < maxStarLimit; i++)
        {
            _meteors.Add(Instantiate(_meteorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Meteor>());
        }
        
        _timeManager = App.GetManager<TimeManager>();
        _lastUpdateTime = _timeManager.Time;
        _lastSpawnTime = _timeManager.Time; //TODO: Save and retrieve the initialization time for each star on the server
        CheckShootingStars();
    }

    private void Update()
    {
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds < 1f) return;
        
        _lastUpdateTime = _timeManager.Time;

        if (!CheckRemainingShootingStars())
        {
            _lastSpawnTime = _timeManager.Time;
            return;
        }
        
        CheckShootingStars();
    }

    private void CheckShootingStars()
    {
        var timeDifference =  _timeManager.Time - _lastSpawnTime;
        
        while (timeDifference.TotalSeconds >= _spawnInterval)
        {
            if (!CheckRemainingShootingStars()) return;
            
            SpawnShootingStar();
            timeDifference = timeDifference.Subtract(TimeSpan.FromSeconds(_spawnInterval));
            _lastSpawnTime = _lastSpawnTime.AddSeconds(_spawnInterval);
        }
    }
    
    private bool CheckRemainingShootingStars()
    {
        return _meteors.Any(star => !star.IsLanded);
    }

    private void SpawnShootingStar()
    {
        var spawnPositions = _installChecker.GetInstallablePositions();
        if (spawnPositions.Count == 0) return;
        var spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
        _tileStateModifier.ModifyTileState(spawnPosition, TileState.Installed);
        
        var shootingStar = _meteors.FirstOrDefault(star => !star.IsLanded);
        shootingStar?.Land(spawnPosition);
    }
}