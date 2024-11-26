using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

public class GameClient
{
    private readonly string _baseUrl;

    public GameClient(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    private void AddAuthorizationHeader(UnityWebRequest request)
    {
        var jwtToken = App.GetManager<LoginManager>().JwtToken;
        if (!string.IsNullOrEmpty(jwtToken))
        {
            request.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
        }
    }
    
    public async UniTask<T> GetAsync<T>(string endpoint)
    {
        endpoint = $"{_baseUrl}/{endpoint}";
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
            AddAuthorizationHeader(request);
            var asyncOp = await request.SendWebRequest().ToUniTask();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }
            return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        }
    }
    
    public async UniTask PutAsync(string endpoint, object data)
    {
        endpoint = $"{_baseUrl}/{endpoint}";
        var jsonData = data == null ? null : JsonConvert.SerializeObject(data);
        using (UnityWebRequest request = UnityWebRequest.Put(endpoint, jsonData))
        {
            AddAuthorizationHeader(request);
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest().ToUniTask();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }
        }
    }
    
    public async Task<T> PostAsync<T>(string endpoint, object data)
    {
        endpoint = $"{_baseUrl}/{endpoint}";
        var jsonData = data == null ? null : JsonConvert.SerializeObject(data);
        Debug.Log($"PostAsync: {endpoint}, {jsonData}");
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            AddAuthorizationHeader(request);
            byte[] jsonToSend = jsonData is not null ? new UTF8Encoding().GetBytes(jsonData) : null;
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest().ToUniTask();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                throw new Exception(request.error);
            }
            return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        }
    }
    
    public async UniTask<bool> DeleteAsync(string endpoint)
    {
        endpoint = $"{_baseUrl}/{endpoint}";
        using (UnityWebRequest request = UnityWebRequest.Delete(endpoint))
        {
            AddAuthorizationHeader(request);
            await request.SendWebRequest().ToUniTask();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
                return false;
            }
            return true;
        }
    }
}