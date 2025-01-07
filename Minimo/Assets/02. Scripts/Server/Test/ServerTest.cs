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
        if (loginResult.IsSuccess)
        {
            Debug.Log("Login Success");
        }
        else
        {
            Debug.Log($"Login Failed: {loginResult.Message}");
            Debug.Log("Creating new account...");
            var createAccountResult = await loginManager.CreateAccountAsync(testAcocntID, testPassword, "TestAccount");
            if (createAccountResult.IsSuccess)
            {
                Debug.Log("Account created successfully");
                // Try to login again
                loginResult = await loginManager.LoginAsync(testAcocntID, testPassword);
                if (loginResult.IsSuccess)
                {
                    Debug.Log("Login Success");
                }
                else
                {
                    Debug.LogError($"Login Failed: {loginResult.Message}");
                    return;
                }
            }
            else
            {
                Debug.LogError($"Failed to create account: {createAccountResult.Message}");
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
            Debug.Log($"Building: {building.BuildingType} (ID: {building.Id}), Position: {building.Position}");
        }

        var randomPosition = new int[] {UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10)};
        var newBuildingRequest = new BuildingDTO
        {
            BuildingType = "TestBuilding",
            Position = randomPosition
        };
        var newBuildingDto = await buildingManager.CreateBuildingAsync(newBuildingRequest);
        if (newBuildingDto != null)
        {
            Debug.Log($"Building created: {newBuildingDto.BuildingType} (ID: {newBuildingDto.Id}), Position: ({newBuildingDto.Position[0]}, {newBuildingDto.Position[1]}, {newBuildingDto.Position[2]})");
        }
        else
        {
            Debug.LogError("Failed to create building");
        }

        // update cratedBuilding to install
        randomPosition = new int[] {UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10)};
        Debug.Log("Updating just created building state...");
        var updateBuildingParameter = new UpdateBuildingParameter()
        {
            Id = newBuildingDto.Id,
            Position = randomPosition
        };
        var updatedBuilding = await buildingManager.UpdateBuildingAsync(updateBuildingParameter);
        if (updatedBuilding != null)
        {
            Debug.Log($"Building updated: {updatedBuilding.BuildingType} (ID: {updatedBuilding.Id}), Position: ({updatedBuilding.Position[0]}, {updatedBuilding.Position[1]}, {updatedBuilding.Position[2]})");
        }
        else
        {
            Debug.LogError("Failed to update building");
        }
    }

    private async UniTask FetchAccounts()
    {
        var result = await _gameClient.GetAsync<List<AccountDTO>>(_endpoint);
        if(result.IsSuccess && result.Data is {} accounts)
        {
            foreach (var account in accounts)
            {
                Debug.Log($"account: {account.Nickname}, Level: {account.Level}");
            }
        }
        else
        {
            Debug.LogError($"Failed to fetch accounts: {result.Message}");
        }
    }
}
