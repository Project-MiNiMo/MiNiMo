using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

[DefaultExecutionOrder(-30)]
public class BuildingManager : ManagerBase
{
    private GameClient _gameClient;

    private const string BuildingEndpoint = "api/building";

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
        var result = await _gameClient.PostAsync<BuildingDTO>(BuildingEndpoint, buildingDto);
        if(result.IsSuccess && result.Data is {} createdBuildingDto)
        {
            Debug.Log($"Created building {createdBuildingDto.BuildingType} (ID: {createdBuildingDto.Position})");
            return createdBuildingDto;
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
            Debug.Log($"Deleted building with ID {buildingId}");
        }
        else
        {
            Debug.LogError($"Failed to delete building with ID {buildingId}: {result.Message}");
        }
        return result.IsSuccess;
    }
}
