using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private GameClient _gameClient;

    private const string BuildingEndpoint = "api/buildings";

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
        try
        {
            var buildings = await _gameClient.GetAsync<List<BuildingDTO>>(BuildingEndpoint);
            Debug.Log($"Retrieved {buildings.Count} buildings from the server.");
            return buildings;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to fetch buildings: {ex.Message}");
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
        try
        {
            var endpoint = $"{BuildingEndpoint}/{buildingId}";
            var building = await _gameClient.GetAsync<BuildingDTO>(endpoint);
            Debug.Log($"Retrieved building {building.Name} (ID: {building.Id})");
            return building;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to fetch building with ID {buildingId}: {ex.Message}");
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
        try
        {
            var createdBuilding = await _gameClient.PostAsync<BuildingDTO>(BuildingEndpoint, buildingDto);
            Debug.Log($"Created building {createdBuilding.Name} (ID: {createdBuilding.Id})");
            return createdBuilding;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to create building: {ex.Message}");
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
        try
        {
            await _gameClient.PutAsync(BuildingEndpoint, updateParameter);
            Debug.Log($"Updated building with ID {updateParameter.Id}");
            return await GetBuildingAsync(updateParameter.Id);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to update building: {ex.Message}");
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
        try
        {
            var endpoint = $"{BuildingEndpoint}/{buildingId}";
            var success = await _gameClient.DeleteAsync(endpoint);
            if (success)
            {
                Debug.Log($"Deleted building with ID {buildingId}");
            }
            else
            {
                Debug.LogError($"Failed to delete building with ID {buildingId}");
            }
            return success;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to delete building: {ex.Message}");
            return false;
        }
    }
}
