using System;

using UnityEngine;

public class ShootingStarCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _shootingStarPrefab;
    [SerializeField] private float _spawnInterval = 10f;

    private TimeManager _timeManager;
    private DateTime _lastSpawnTime;

    private int _currentStarCount = 0;
    private int _maxStarLimit;

    private float _checkTimer = 0f;
    private const float CHECK_INTERVAL = 1f;

    private void Start()
    {
        _maxStarLimit = App.GetData<TitleData>().Common["MaxStarLimit"];

        _timeManager = App.GetManager<TimeManager>();
        _lastSpawnTime = _timeManager.Time; // Reset last spawn time to server time //TODO: Save and retrieve the initialization time for each star on the server
        CheckShootingStars();
    }

    private void Update()
    {
        _checkTimer += Time.deltaTime;
        if (_checkTimer >= CHECK_INTERVAL)
        {
            CheckShootingStars();
            _checkTimer = 0f;
        }
    }

    private void CheckShootingStars()
    {
        DateTime currentServerTime = _timeManager.Time; // Get current server time
        TimeSpan timeDifference = currentServerTime - _lastSpawnTime;

        // If the spawn time has passed, a meteor will be created
        while (_currentStarCount < _maxStarLimit && timeDifference.TotalSeconds >= _spawnInterval)
        {
            SpawnShootingStar();
            timeDifference = timeDifference.Subtract(TimeSpan.FromSeconds(_spawnInterval));
            _lastSpawnTime = _lastSpawnTime.AddSeconds(_spawnInterval);
        }
    }

    private void SpawnShootingStar()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(_shootingStarPrefab, spawnPosition, Quaternion.identity).GetComponent<ShootingStar>().Initialize(this);
        _currentStarCount++;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // Returns a random location on the map
        // TODO: Consider locations that do not overlap with objects
        float x = UnityEngine.Random.Range(-5f, 5f); // TODO: Needs to be adjusted to map size
        float y = UnityEngine.Random.Range(-5f, 5f);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Treatment during oil harvesting
    /// </summary>
    /// <param name="star"></param>
    public void OnHarvestShootingStar(GameObject star)
    {
        _currentStarCount--;
        Destroy(star);
    }
}