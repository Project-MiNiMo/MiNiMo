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
            App.GetManager<AccountAssetManager>().UpdateBuilding(buildingCreateResult.CreatedBuilding);
            App.GetManager<AccountAssetManager>().UpdateBuildingInfo(buildingCreateResult.BuildingInfoDto);
            App.GetManager<AccountAssetManager>().UpdateItems(buildingCreateResult.UpdatedItems);
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
            App.GetManager<AccountAssetManager>().UpdateBuilding(updatedBuildingDto);
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
            App.GetManager<AccountAssetManager>().DeleteBuilding(buildingId);
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
            App.GetManager<AccountAssetManager>().UpdateBuildingInfo(createResult.BuildingInfo);
            App.GetManager<AccountAssetManager>().UpdateCurrency(createResult.UpdatedCurrency);
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
            App.GetManager<AccountAssetManager>().UpdateBuildingInfo(upgradeResult.BuildingInfo);
            App.GetManager<AccountAssetManager>().UpdateCurrency(upgradeResult.UpdatedCurrency);
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
            App.GetManager<AccountAssetManager>().UpdateBuilding(producedBuilding);
            // TODO : 자원 업데이트
            return producedBuilding;
        }
        else
        {
            Debug.LogError($"Failed to start producing building: {result.Message}");
            return null;
        }
    }
    
    public async UniTask<BuildingCompleteProduceResultDTO> CompleteProduce(BuildingCompleteProduceDTO completeProduceDto)
    {
        var endpoint = $"{BuildingEndpoint}/recipe/complete";
        var result = await _gameClient.PostAsync<BuildingCompleteProduceResultDTO>(endpoint, completeProduceDto);
        if(result.IsSuccess && result.Data is {} completeResult)
        {
            Debug.Log($"Completed producing building {completeResult.UpdatedBuilding.BuildingType}");
            App.GetManager<AccountAssetManager>().UpdateBuilding(completeResult.UpdatedBuilding);
            App.GetManager<AccountAssetManager>().UpdateItems(completeResult.ProducedItems);
            App.GetManager<AccountAssetManager>().UpdateCurrency(completeResult.UpdatedCurrency);
            if (completeResult.ProducedItems != null)
            {
                foreach (var itemDto in completeResult.ProducedItems)
                {
                    App.GetManager<AccountAssetManager>().UpdateItem(itemDto);
                }
            }
            return completeResult;
        }
        else
        {
            Debug.LogError($"Failed to complete producing building: {result.Message}");
            return null;
        }
    }
#endregion
}
