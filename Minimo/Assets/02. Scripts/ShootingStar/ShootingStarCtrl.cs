using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class ShootingStarCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _shootingStarPrefab;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private InstallChecker _installChecker;

    private List<ShootingStar> _shootingStars;
    
    private TimeManager _timeManager;
    private DateTime _lastSpawnTime;
    
    private float _checkTimer = 0f;
    private const float CHECK_INTERVAL = 1f;

    private void Start()
    {
        var maxStarLimit = App.GetData<TitleData>().Common["MaxStarLimit"];
        
        _shootingStars = new List<ShootingStar>();
        for (var i = 0; i < maxStarLimit; i++)
        {
            _shootingStars.Add(Instantiate(_shootingStarPrefab, Vector3.zero, Quaternion.identity).GetComponent<ShootingStar>());
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
        return _shootingStars.Any(star => !star.IsLanded);
    }

    private void SpawnShootingStar()
    {
        var spawnPositions = _installChecker.GetInstallablePositions();
        var spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
        
        var shootingStar = _shootingStars.FirstOrDefault(star => star.IsLanded);
        shootingStar?.Land(spawnPosition);
    }
}