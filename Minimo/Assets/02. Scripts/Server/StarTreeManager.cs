using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
 
public class StarTreeManager : ManagerBase
{
    private GameClient _gameClient;

    private const string StarTreeEndpoint = "api/startree";
    private const string StarTreeWishEndpoint = "api/startree/wish";
    private const string StarTreeLevelUpEndpoint = "api/satrtree/levelup";

    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
    }
    
    /// <summary>
    /// 현재까지 누적된 별나무의 보상을 획득합니다.
    /// </summary>
    /// <returns></returns>
    public async UniTask<ApiResult<StarTreeResultDTO>> GetStarTreeResult()
    {
        var result = await _gameClient.GetAsync<StarTreeResultDTO>(StarTreeEndpoint);
        if (result.IsSuccess && result.Data is {} starTreeResultDto)
        {
            App.GetManager<AccountInfoManager>().UpdateCurrency(starTreeResultDto.UpdatedCurrency);
            App.GetManager<AccountInfoManager>().UpdateLastStarTreeCreatedAt(starTreeResultDto.LastDateTime);
            return result;
        }
        else
        {
            Debug.LogError($"Failed to get star tree result. {result.Message}");
            return result;
        }
    }

    /// <summary>
    /// 별나무에 소원을 빌고 최대 보상을 획득합니다.
    /// </summary>
    /// <returns></returns>
    public async UniTask<ApiResult<StarTreeResultDTO>> GetWishResult()
    {
        var result = await _gameClient.GetAsync<StarTreeResultDTO>(StarTreeWishEndpoint);
        if (result.IsSuccess && result.Data is {} starTreeResultDto)
        {
            App.GetManager<AccountInfoManager>().UpdateCurrency(starTreeResultDto.UpdatedCurrency);
            App.GetManager<AccountInfoManager>().UpdateLastWishedAt(starTreeResultDto.LastDateTime);
            return result;
        }
        else
        {
            Debug.LogError($"Failed to get wish result. {result.Message}");
            return result;
        }
    }
    
    /// <summary>
    /// 별나무의 레벨을 올립니다.
    /// </summary>
    /// <returns></returns>
    public async UniTask<ApiResult<int>> GetLevelUpResult()
    {
        var result = await _gameClient.GetAsync<int>(StarTreeLevelUpEndpoint);
        if (result.IsSuccess && result.Data is {} starTreeResultDto)
        {
            _gameClient.AccountInfo.StarTreeLevel = starTreeResultDto;
            return result;
        }
        else
        {
            Debug.LogError($"Failed to get level up result. {result.Message}");
            return result;
        }
    }
}
