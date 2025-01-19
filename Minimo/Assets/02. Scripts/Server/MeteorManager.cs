
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
    /// 유성을 생성합니다(시간이 충분히 지난 경우).
    /// </summary>
    /// <returns></returns>
    public async UniTask<ApiResult<List<MeteorDTO>>> CreateMeteor()
    {
        var result = await _gameClient.GetAsync<List<MeteorDTO>>(MeteorCreateEndpoint);
        if (result.IsSuccess && result.Data is {} createdMeteors)
        {
            AddMeteors(createdMeteors);
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
            RemoveMeteor(meteorId);
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to get meteor result: {result.Message}");
            return result;
        }
    }

    private void AddMeteor(MeteorDTO meteorDto)
    {
        _gameClient.AccountInfo.Meteors.Add(meteorDto);
    }
    
    private void AddMeteors(List<MeteorDTO> meteorDtos)
    {
        _gameClient.AccountInfo.Meteors.AddRange(meteorDtos);
    }

    private void RemoveMeteor(int meteorId)
    {
        var meteor = _gameClient.AccountInfo.Meteors.Find(m => m.Id == meteorId);
        _gameClient.AccountInfo.Meteors.Remove(meteor);
    }
}
