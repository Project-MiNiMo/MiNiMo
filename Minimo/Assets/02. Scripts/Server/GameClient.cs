using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using MinimoShared;
using Newtonsoft.Json;

public class GameClient
{
    private readonly string _baseUrl;

    public GameClient(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    // Get all players (returns PlayerDTO list)
    public async UniTask<IEnumerable<PlayerDTO>> GetPlayersAsync()
    {
        string endpoint = $"{_baseUrl}/api/Players";
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
            var asyncOp = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }

            // Deserialize response to PlayerDTO array
            List<PlayerDTO> players = JsonConvert.DeserializeObject<List<PlayerDTO>>(request.downloadHandler.text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return players;
        }
    }

    // Get player by ID (returns PlayerDTO)
    public async UniTask<PlayerDTO> GetPlayerAsync(int id)
    {
        string endpoint = $"{_baseUrl}/api/Players/{id}";
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
            var asyncOp = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }

            return JsonUtility.FromJson<PlayerDTO>(request.downloadHandler.text);
        }
    }

    // Create a new player using PlayerDTO
    public async UniTask<PlayerDTO> CreatePlayerAsync(PlayerDTO playerDTO)
    {
        string endpoint = $"{_baseUrl}/api/Players";
        string jsonData = JsonUtility.ToJson(playerDTO);
        
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var asyncOp = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }

            return JsonUtility.FromJson<PlayerDTO>(request.downloadHandler.text);
        }
    }

    // Update an existing player using PlayerDTO
    public async UniTask<PlayerDTO> UpdatePlayerAsync(int id, PlayerDTO playerDTO)
    {
        string endpoint = $"{_baseUrl}/api/Players/{id}";
        string jsonData = JsonUtility.ToJson(playerDTO);

        using (UnityWebRequest request = new UnityWebRequest(endpoint, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var asyncOp = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }

            return JsonUtility.FromJson<PlayerDTO>(request.downloadHandler.text);
        }
    }

    // Delete a player
    public async UniTask<bool> DeletePlayerAsync(int id)
    {
        string endpoint = $"{_baseUrl}/api/Players/{id}";

        using (UnityWebRequest request = UnityWebRequest.Delete(endpoint))
        {
            var asyncOp = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                return false;
            }

            return true;
        }
    }
}