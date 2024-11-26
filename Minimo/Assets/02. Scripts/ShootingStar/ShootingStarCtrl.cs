using UnityEngine;
using System;

public class ShootingStarCtrl : MonoBehaviour
{
    public GameObject shootingStarPrefab;

    public float spawnInterval = 10f;
    private int currentStarCount = 0;
    private DateTime lastSpawnTime;

    private int maxStarLimit;

    private float checkTimer = 0f;
    private const float CHECK_INTERVAL = 1f;

    private void Start()
    {
        maxStarLimit = App.GetData<TitleData>().Common["MaxStarLimit"];

        lastSpawnTime = App.GetManager<TimeManager>().Time; // Reset last spawn time to server time //TODO: Save and retrieve the initialization time for each star on the server
        CheckShootingStars();
    }

    private void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= CHECK_INTERVAL)
        {
            CheckShootingStars();
            checkTimer = 0f;
        }
    }

    private void CheckShootingStars()
    {
        DateTime currentServerTime = App.GetManager<TimeManager>().Time; // Get current server time
        TimeSpan timeDifference = currentServerTime - lastSpawnTime;

        // If the spawn time has passed, a meteor will be created
        while (currentStarCount < maxStarLimit && timeDifference.TotalSeconds >= spawnInterval)
        {
            SpawnShootingStar();
            timeDifference = timeDifference.Subtract(TimeSpan.FromSeconds(spawnInterval));
            lastSpawnTime = lastSpawnTime.AddSeconds(spawnInterval);
        }
    }

    private void SpawnShootingStar()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(shootingStarPrefab, spawnPosition, Quaternion.identity).GetComponent<ShootingStar>().Initialize(this);
        currentStarCount++;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // Returns a random location on the map
        // TODO: Consider locations that do not overlap with objects
        float x = UnityEngine.Random.Range(-10f, 10f); // TODO: Needs to be adjusted to map size
        float y = UnityEngine.Random.Range(-5f, 5f);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Treatment during oil harvesting
    /// </summary>
    /// <param name="star"></param>
    public void OnHarvestShootingStar(GameObject star)
    {
        currentStarCount--;
        Destroy(star);
    }
}