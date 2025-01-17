using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class MeteorCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _meteorPrefab;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private InstallChecker _installChecker;

    private List<Meteor> _meteors;
    
    private TimeManager _timeManager;
    private DateTime _lastSpawnTime;
    
    private float _checkTimer = 0f;
    private const float CHECK_INTERVAL = 1f;

    private void Start()
    {
        var maxStarLimit = App.GetData<TitleData>().Common["MeteorLimit"];
        
        _meteors = new List<Meteor>();
        for (var i = 0; i < maxStarLimit; i++)
        {
            _meteors.Add(Instantiate(_meteorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Meteor>());
        }
        
        _timeManager = App.GetManager<TimeManager>();
        _lastSpawnTime = _timeManager.Time; //TODO: Save and retrieve the initialization time for each star on the server
        CheckShootingStars();
    }

    private void Update()
    {
        _checkTimer += Time.deltaTime;
        if (_checkTimer >= CHECK_INTERVAL)
        {
            if (!CheckRemainingShootingStars()) return;
            
            CheckShootingStars();
            _checkTimer = 0f;
        }
    }

    private void CheckShootingStars()
    {
        var timeDifference =  _timeManager.Time - _lastSpawnTime;
        
        while (timeDifference.TotalSeconds >= _spawnInterval)
        {
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
        var spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
        
        var shootingStar = _meteors.FirstOrDefault(star => star.IsLanded);
        shootingStar?.Land(spawnPosition);
    }
}