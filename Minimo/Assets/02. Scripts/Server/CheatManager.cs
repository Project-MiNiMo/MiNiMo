using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class CheatManager : ManagerBase
{
    private GameClient _gameClient;
    private AccountAssetManager _accountAssetManager;

    private const string CheatEndpoint = "api/cheat";

    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
        _accountAssetManager = App.GetManager<AccountAssetManager>();
    }
    
    public async UniTask UpdateCurrency(CurrencyDTO targetCurrency)
    {
        var endPoint = $"{CheatEndpoint}/currency";
        var result = await _gameClient.PostAsync<CurrencyDTO>(endPoint, targetCurrency);
        if(result.IsSuccess && result.Data is {} currency)
        {
            Debug.Log($"Currency updated: {currency}");
            _accountAssetManager.UpdateCurrency(currency);
            return;
        }
        else
        {
            Debug.LogError($"Failed to update currency: {result.Message}");
            return;
        }
    }
    
    public async UniTask UpdateBuildingInfo(BuildingInfoDTO targetBuildingInfo)
    {
        var endPoint = $"{CheatEndpoint}/buildinginfo";
        var result = await _gameClient.PostAsync<BuildingInfoDTO>(endPoint, targetBuildingInfo);
        if(result.IsSuccess && result.Data is {} buildingInfo)
        {
            Debug.Log($"Building info updated: {buildingInfo}");
            _accountAssetManager.UpdateBuildingInfo(buildingInfo);
            return;
        }
        else
        {
            Debug.LogError($"Failed to update building info: {result.Message}");
            return;
        }
    }

    public async UniTask UpdateItem(ItemDTO targetItem)
    {
        var endPoint = $"{CheatEndpoint}/item";
        var result = await _gameClient.PostAsync<ItemDTO>(endPoint, targetItem);
        if(result.IsSuccess && result.Data is {} item)
        {
            Debug.Log($"Item updated: {item}");
            _accountAssetManager.UpdateItem(item);
            return;
        }
        else
        {
            Debug.LogError($"Failed to update item: {result.Message}");
            return;
        }
    }

    public void SetServerTime(string targetTime)
    {
        App.GetManager<TimeManager>().SetCheatTime(targetTime);
    }
}
