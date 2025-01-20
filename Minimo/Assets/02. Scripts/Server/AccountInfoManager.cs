using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class AccountInfoManager : ManagerBase
{
    public string NickName => _gameClient.AccountInfo.Nickname;
    public int Level => _gameClient.AccountInfo.Level;
    public int Star => _gameClient.AccountInfo.Currency.Star;
    public int BlueStar => _gameClient.AccountInfo.Currency.BlueStar;
    
    private GameClient _gameClient;
    
    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
    }
    
    public void UpdateAccountInfo(AccountDTO account)
    {
        _gameClient.AccountInfo = account;
    }
    
    public void UpdateCurrency(CurrencyDTO currency)
    {
        _gameClient.AccountInfo.Currency = currency;
    }

    public void UpdateBuilding(BuildingDTO buildingDto)
    {
        var building = _gameClient.AccountInfo.Buildings.Find(b => b.Id == buildingDto.Id);
        if(building != null)
        {
            building.CopyFrom(buildingDto);
        }
        else
        {
            _gameClient.AccountInfo.Buildings.Add(buildingDto);
        }
    }
    
    public void DeleteBuilding(int buildingId)
    {
        var building = _gameClient.AccountInfo.Buildings.Find(b => b.Id == buildingId);
        if(building != null)
        {
            _gameClient.AccountInfo.Buildings.Remove(building);
        }
    }
    
    public void UpdateBuildingInfo(BuildingInfoDTO buildingInfoDto)
    {
        var buildingInfo = _gameClient.AccountInfo.BuildingInfos.Find(b => b.BuildingType == buildingInfoDto.BuildingType);
        if(buildingInfo != null)
        {
            buildingInfo.CopyFrom(buildingInfoDto);
        }
        else
        {
            _gameClient.AccountInfo.BuildingInfos.Add(buildingInfoDto);
        }
    }
    
    public void UpdateItem(ItemDTO itemDto)
    {
        var item = _gameClient.AccountInfo.Items.Find(i => i.ItemType == itemDto.ItemType);
        if(item != null)
        {
            item.CopyFrom(itemDto);
        }
        else
        {
            _gameClient.AccountInfo.Items.Add(itemDto);
        }
    }
    
    public void UpdateItems(List<ItemDTO> items)
    {
        foreach(var itemDto in items)
        {
            UpdateItem(itemDto);
        }
    }
    
    public void UpdateAssetInfo(AssetUpdateDTO assetUpdate)
    {
        if(assetUpdate.CurrencyUpdate != null)
        {
            UpdateCurrency(assetUpdate.CurrencyUpdate.CurrentCurrency);
        }
        if(assetUpdate.BuildingsUpdate != null)
        {
            foreach(var buildingUpdateInfo in assetUpdate.BuildingsUpdate)
            {
                UpdateBuildingInfo(buildingUpdateInfo.CurrentBuildingInfo);
            }
        }
        if(assetUpdate.ItemsUpdate != null)
        {
            foreach(var itemUpdate in assetUpdate.ItemsUpdate)
            {
                UpdateItem(itemUpdate.CurrentItem);
            }
        }
    }
    
    public void UpdateLastMeteorCreatedAt(DateTime lastMeteorCreatedAt)
    {
        _gameClient.AccountInfo.LastMeteorCreatedAt = lastMeteorCreatedAt;
    }
    
    public void AddMeteor(MeteorDTO meteorDto)
    {
        _gameClient.AccountInfo.Meteors.Add(meteorDto);
    }
    
    public void AddMeteors(List<MeteorDTO> meteorDtos)
    {
        _gameClient.AccountInfo.Meteors.AddRange(meteorDtos);
    }

    public void RemoveMeteor(int meteorId)
    {
        var meteor = _gameClient.AccountInfo.Meteors.Find(m => m.Id == meteorId);
        _gameClient.AccountInfo.Meteors.Remove(meteor);
    }
    
    public void AddQuest(QuestDTO questDto)
    {
        _gameClient.AccountInfo.Quests.Add(questDto);
    }
    
    public void UpdateLastStarTreeCreatedAt(DateTime lastStarTreeCreatedAt)
    {
        _gameClient.AccountInfo.LastStarTreeCreatedAt = lastStarTreeCreatedAt;
    }
    
    public void UpdateLastWishedAt(DateTime lastWishedAt)
    {
        _gameClient.AccountInfo.LastWishedAt = lastWishedAt;
    }
}
