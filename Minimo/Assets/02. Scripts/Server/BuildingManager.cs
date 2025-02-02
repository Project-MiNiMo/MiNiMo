using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class BuildingManager : ManagerBase
{
    private GameClient _gameClient;

    private const string BuildingEndpoint = "api/building";
    private const string BuildingInfoEndpoint = "api/buildinginfo";
    
    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
    }
    
    /// <summary>
    /// 서버에서 건물 목록을 가져옵니다.
    /// </summary>
    /// <returns>건물 DTO 목록</returns>
    public async UniTask<List<BuildingDTO>> GetBuildingsAsync()
    {
        var result = await _gameClient.GetAsync<List<BuildingDTO>>(BuildingEndpoint);
        if(result.IsSuccess && result.Data is {} buildings)
        {
            Debug.Log($"Retrieved {buildings.Count} buildings from the server.");
            return buildings;
        }
        else
        {
            Debug.LogError($"Failed to fetch buildings: {result.Message}");
            return new List<BuildingDTO>();
        }
    }

    /// <summary>
    /// 특정 ID의 건물 정보를 가져옵니다.
    /// </summary>
    /// <param name="buildingId">건물 ID</param>
    /// <returns>건물 DTO</returns>
    public async UniTask<BuildingDTO> GetBuildingAsync(int buildingId)
    {
        var endpoint = $"{BuildingEndpoint}/{buildingId}";
        var result = await _gameClient.GetAsync<BuildingDTO>(endpoint);
        if(result.IsSuccess && result.Data is {} building)
        {
            Debug.Log($"Retrieved building {building.BuildingType} (ID: {building.Id})");
            return building;
        }
        else
        {
            Debug.LogError($"Failed to fetch building with ID {buildingId}: {result.Message}");
            return null;
        }
    }

    /// <summary>
    /// 새 건물을 생성합니다.
    /// </summary>
    /// <param name="buildingDto">생성할 건물 정보</param>
    /// <returns>생성된 건물 DTO</returns>
    public async UniTask<BuildingDTO> CreateBuildingAsync(BuildingDTO buildingDto)
    {
        var result = await _gameClient.PostAsync<BuildingCreateResultDTO>(BuildingEndpoint, buildingDto);
        if(result.IsSuccess && result.Data is {} buildingCreateResult)
        {
            Debug.Log($"Created building {buildingCreateResult.CreatedBuilding.BuildingType} (ID: {buildingCreateResult.CreatedBuilding.Id})");
            App.GetManager<AccountInfoManager>().UpdateBuilding(buildingCreateResult.CreatedBuilding);
            App.GetManager<AccountInfoManager>().UpdateBuildingInfo(buildingCreateResult.BuildingInfoDto);
            App.GetManager<AccountInfoManager>().UpdateItems(buildingCreateResult.UpdatedItems);
            return buildingCreateResult.CreatedBuilding;
        }
        else
        {
            Debug.LogError($"Failed to create building: {result.Message}");
            return null;
        }
    }

    /// <summary>
    /// 건물 정보를 업데이트합니다.
    /// </summary>
    /// <param name="updateParameter">업데이트할 건물 정보</param>
    /// <returns>업데이트된 건물 DTO</returns>
    public async UniTask<BuildingDTO> UpdateBuildingAsync(UpdateBuildingParameter updateParameter)
    {
        var result = await _gameClient.PutAsync<BuildingDTO>(BuildingEndpoint, updateParameter);
        if(result.IsSuccess && result.Data is {} updatedBuildingDto)
        {
            Debug.Log($"Updated building with ID {updateParameter.Id}");
            App.GetManager<AccountInfoManager>().UpdateBuilding(updatedBuildingDto);
            return updatedBuildingDto;
        }
        else
        {
            Debug.LogError($"Failed to update building: {result.Message}");
            return null;
        }
    }

    /// <summary>
    /// 특정 ID의 건물을 삭제합니다.
    /// </summary>
    /// <param name="buildingId">건물 ID</param>
    /// <returns>삭제 성공 여부</returns>
    public async UniTask<bool> DeleteBuildingAsync(int buildingId)
    {
        var endpoint = $"{BuildingEndpoint}/{buildingId}";
        var result = await _gameClient.DeleteAsync(endpoint);
        if(result.IsSuccess)
        {
            App.GetManager<AccountInfoManager>().DeleteBuilding(buildingId);
            Debug.Log($"Deleted building with ID {buildingId}");
        }
        else
        {
            Debug.LogError($"Failed to delete building with ID {buildingId}: {result.Message}");
        }
        return result.IsSuccess;
    }

#region 건물 해금/업그레이드
    /// <summary>
    /// 건물 최초 해금을 요청합니다.
    /// </summary>
    /// <param name="buildingType"></param>
    /// <returns></returns>
        public async UniTask<BuildingInfoDTO> CreateBuildingInfo(string buildingType)
    {
        var result = await _gameClient.PostAsync<BuildingInfoUpgradeResultDTO>(BuildingInfoEndpoint, buildingType);
        if(result.IsSuccess && result.Data is {} createResult)
        {
            Debug.Log($"Created building info for {buildingType}");
            App.GetManager<AccountInfoManager>().UpdateBuildingInfo(createResult.BuildingInfo);
            App.GetManager<AccountInfoManager>().UpdateCurrency(createResult.UpdatedCurrency);
            return createResult.BuildingInfo;
        }
        else
        {
            Debug.LogError($"Failed to create building info: {result.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 해금된 건물의 업그레이드(개수 증가)를 요청합니다.
    /// </summary>
    /// <param name="buildingType"></param>
    /// <returns></returns>
    public async UniTask<BuildingInfoDTO> UpgradeBuildingInfoMaxCount(string buildingType)
    {
        var endpoint = $"{BuildingInfoEndpoint}/upgrademax";
        var result = await _gameClient.PostAsync<BuildingInfoUpgradeResultDTO>(endpoint, buildingType);
        if(result.IsSuccess && result.Data is {} upgradeResult)
        {
            Debug.Log($"Upgraded max count for building {buildingType}");
            // 자원 업데이트
            App.GetManager<AccountInfoManager>().UpdateBuildingInfo(upgradeResult.BuildingInfo);
            App.GetManager<AccountInfoManager>().UpdateCurrency(upgradeResult.UpdatedCurrency);
            return upgradeResult.BuildingInfo;
        }
        else
        {
            Debug.LogError($"Failed to upgrade max count: {result.Message}");
            return null;
        }
    }
#endregion

#region 건물 제조 슬롯
    /// <summary>
    /// 자원 제조를 시작합니다
    /// </summary>
    /// <param name="startProduceDto"></param>
    /// <returns></returns>
    public async UniTask<BuildingDTO> StartProduce(BuildingStartProduceDTO startProduceDto)
    {
        var result = await _gameClient.PostAsync<BuildingDTO>($"{BuildingEndpoint}/recipe", startProduceDto);
        if(result.IsSuccess && result.Data is {} producedBuilding)
        {
            Debug.Log($"Started producing building {producedBuilding.BuildingType}");
            App.GetManager<AccountInfoManager>().UpdateBuilding(producedBuilding);
            // TODO : 자원 업데이트
            return producedBuilding;
        }
        else
        {
            Debug.LogError($"Failed to start producing building: {result.Message}");
            return null;
        }
    }
    
    public async UniTask<bool> CompleteProduce(BuildingCompleteProduceDTO completeProduceDto)
    {
        var endpoint = $"{BuildingEndpoint}/recipe/complete";
        var result = await _gameClient.PostAsync<object>(endpoint, completeProduceDto); // 서버는 성공 메시지만 반환
        if (result.IsSuccess)
        {
            Debug.Log($"Successfully completed production for building ID {completeProduceDto.BuildingId}, Slot {completeProduceDto.SlotIndex}");
            return true;
        }
        else
        {
            Debug.LogError($"Failed to complete producing building: {result.Message}");
            return false;
        }
    }
    
    public async UniTask<BuildingHarvestProduceResultDTO> HarvestProduce(BuildingHarvestProduceDTO harvestProduceDto)
    {
        var endpoint = $"{BuildingEndpoint}/recipe/harvest";
        var result = await _gameClient.PostAsync<BuildingHarvestProduceResultDTO>(endpoint, harvestProduceDto);
        if (result.IsSuccess && result.Data is {} harvestResult)
        {
            Debug.Log($"Successfully harvested resources from building ID {harvestProduceDto.BuildingId}, Slot {harvestProduceDto.SlotIndex}");
            App.GetManager<AccountInfoManager>().UpdateBuilding(harvestResult.UpdatedBuilding);
            App.GetManager<AccountInfoManager>().UpdateItems(harvestResult.ProducedItems);
            App.GetManager<AccountInfoManager>().UpdateCurrency(harvestResult.UpdatedCurrency);
            if (harvestResult.ProducedItems != null)
            {
                foreach (var itemDto in harvestResult.ProducedItems)
                {
                    App.GetManager<AccountInfoManager>().UpdateItem(itemDto);
                }
            }
            return harvestResult;
        }
        else
        {
            Debug.LogError($"Failed to harvest resources: {result.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 현금을 지불하여 즉시 생산을 완료합니다.
    /// </summary>
    /// <param name="instantProduceDto">즉시 생산 요청 정보</param>
    /// <returns>업데이트된 건물 DTO</returns>
    public async UniTask<BuildingInstantProduceResultDTO> InstantProduceAsync(BuildingInstantProduceDTO instantProduceDto)
    {
        var endpoint = $"{BuildingEndpoint}/recipe/instant";
        var result = await _gameClient.PostAsync<BuildingInstantProduceResultDTO>(endpoint, instantProduceDto);
        if (result.IsSuccess && result.Data is {} instantProduceResult)
        {
            Debug.Log($"Instantly completed production for building {instantProduceResult.UpdatedBuilding.BuildingType}");
            App.GetManager<AccountInfoManager>().UpdateBuilding(instantProduceResult.UpdatedBuilding);
            App.GetManager<AccountInfoManager>().UpdateCurrency(instantProduceResult.UpdatedCurrency);
            return instantProduceResult;
        }
        else
        {
            Debug.LogError($"Failed to instantly complete production: {result.Message}");
            return null;
        }
    }
#endregion
}
