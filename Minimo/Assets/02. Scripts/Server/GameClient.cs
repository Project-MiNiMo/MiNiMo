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
    
    public async UniTask<T> GetAsync<T>(string endpoint)
    {
        endpoint = $"{_baseUrl}/{endpoint}";
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
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
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
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
        using (UnityWebRequest request = UnityWebRequest.Delete(endpoint))
        {
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