
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using UnityEngine;

public class MeteorManager : ManagerBase
{
    private GameClient _gameClient;

    private const string MeteorEndpoint = "api/meteor";
    private const string MeteorCreateEndpoint = "api/meteor/create";
    private const string MeteorResultEndpoint = "api/meteor/result";

    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
    }
    
    /// <summary>
    /// 유성 정보를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public async UniTask<ApiResult<List<MeteorDTO>>> GetMeteors()
    {
        var result = await _gameClient.GetAsync<List<MeteorDTO>>(MeteorEndpoint);
        if (result.IsSuccess && result.Data is {} meteorList)
        {
            // 유성 정보를 AccountInfoManager에 업데이트
            Debug.Log($"Retrieved {meteorList.Count} meteors from server.");
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to retrieve meteors: {result.Message}");
            return result;
        }
    }
    
    /// <summary>
    /// 유성을 생성합니다(시간이 충분히 지난 경우).
    /// </summary>
    /// <returns></returns>
    public async UniTask<ApiResult<MeteorCreateDTO>> CreateMeteor()
    {
        var result = await _gameClient.GetAsync<MeteorCreateDTO>(MeteorCreateEndpoint);
        if (result.IsSuccess && result.Data is {} meteorCreateDto)
        {
            App.GetManager<AccountInfoManager>().AddMeteors(meteorCreateDto.CreatedMeteors);
            App.GetManager<AccountInfoManager>().UpdateLastMeteorCreatedAt(meteorCreateDto.LastMeteorCreatedAt);
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to create meteor: {result.Message}");
            return result;
        }
    }
    
    /// <summary>
    /// 유성의 결과를 가져옵니다.(유성은 사라집니다)
    /// </summary>
    /// <param name="meteorId"></param>
    /// <returns></returns>
    public async UniTask<ApiResult<MeteorResultDTO>> GetMeteorResult(int meteorId)
    {
        var result = await _gameClient.GetAsync<MeteorResultDTO>($"{MeteorResultEndpoint}/{meteorId}");
        if (result.IsSuccess && result.Data is {} meteorResult)
        {
            // TODO : 유성 결과 처리 
            if (meteorResult.ResultItem != null)
            {
                Debug.Log($"Meteor result: {meteorResult.ResultItem}");
                App.GetManager<AccountInfoManager>().UpdateItem(meteorResult.ResultItem);
            }
            if (meteorResult.ResultQuest != null)
            {
                Debug.Log($"Meteor result: {meteorResult.ResultQuest}");
                App.GetManager<AccountInfoManager>().AddQuest(meteorResult.ResultQuest);   
            }
            App.GetManager<AccountInfoManager>().RemoveMeteor(meteorId);
            App.GetManager<AccountInfoManager>().UpdateLastMeteorCreatedAt(meteorResult.LastMeteorCreatedAt);
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to get meteor result: {result.Message}");
            return result;
        }
    }
}
