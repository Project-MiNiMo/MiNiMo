using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;
using Random = System.Random;

public class ServerTest : MonoBehaviour
{
    private GameClient _gameClient;
    private readonly string _endpoint = "api/accounts";
    
    private const string testAcocntID = "test";
    private const string testPassword = "test";

    private async void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();

        // Fetch all accounts
        await FetchAccounts();
        // Login and building test
        await BuildingTest();
    }

    private async UniTask LoginTest()
    {
        var loginManager = App.GetManager<LoginManager>();
        var loginResult = await loginManager.LoginAsync(testAcocntID, testPassword);
        if (loginResult.Item1)
        {
            Debug.Log("Login Success");
        }
        else
        {
            Debug.Log($"Login Failed: {loginResult.Item2}");
            Debug.Log("Creating new account...");
            var createAccountResult = await loginManager.CreateAccountAsync(testAcocntID, testPassword, "TestAccount");
            if (createAccountResult.Item1)
            {
                Debug.Log("Account created successfully");
                // Try to login again
                loginResult = await loginManager.LoginAsync(testAcocntID, testPassword);
                if (loginResult.Item1)
                {
                    Debug.Log("Login Success");
                }
                else
                {
                    Debug.LogError($"Login Failed: {loginResult.Item2}");
                    return;
                }
            }
            else
            {
                Debug.LogError($"Failed to create account: {createAccountResult.Item2}");
                return;
            }
        }
    }

    private async UniTask BuildingTest()
    {
        await LoginTest();
        var buildingManager = App.GetManager<BuildingManager>();
        var buildings = await buildingManager.GetBuildingsAsync();
        foreach (var building in buildings)
        {
            Debug.Log($"Building: {building.Name} (ID: {building.Id}), Installed: {building.IsInstalled}, Position: {building.Position}");
        }
        BuildingDTO firstBuilding;
        if (buildings.Count == 0)
        {
            Debug.Log("No buildings found. Creating a new building...");
            var newBuilding = new BuildingDTO
            {
                Name = "TestBuilding",
            };
            firstBuilding = await buildingManager.CreateBuildingAsync(newBuilding);
            if (firstBuilding != null)
            {
                Debug.Log($"Building created: {firstBuilding.Name} (ID: {firstBuilding.Id})");
            }
            else
            {
                Debug.LogError("Failed to create building");
            }
        }
        else
        {
            firstBuilding = buildings[0];
        }

        // update cratedBuilding to install
        var randomPosition =
            new System.Numerics.Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        Debug.Log("Updating first building state...");
        var updateBuildingParameter = new UpdateBuildingParameter()
        {
            Id = firstBuilding.Id,
            IsInstalled = firstBuilding.IsInstalled.HasValue ? !firstBuilding.IsInstalled : true,
            Position = randomPosition
        };
        var updatedBuilding = await buildingManager.UpdateBuildingAsync(updateBuildingParameter);
        if (updatedBuilding != null)
        {
            Debug.Log($"Building updated: {updatedBuilding.Name} (ID: {updatedBuilding.Id}), Installed: {updatedBuilding.IsInstalled}, Position: {updatedBuilding.Position}");
        }
        else
        {
            Debug.LogError("Failed to update building");
        }
    }

    private async UniTask FetchAccounts()
    {
        try
        {
            // Get all accounts
            var accounts = await _gameClient.GetAsync<List<AccountDTO>>(_endpoint);
            foreach (var account in accounts)
            {
                Debug.Log($"account: {account.Nickname}, Level: {account.Level}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch accounts: {ex.Message}");
        }
    }
}
