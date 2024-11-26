
using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : ManagerBase
{
    private GameClient _gameClient;
    
    [SerializeField] private string _userName = "";
    [SerializeField] private string _password = "";
    [SerializeField] private string _nickname = "";
    
    private readonly string _endpoint = "api/login";
    
    public string JwtToken { get; private set; }
    
    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
        JwtToken = PlayerPrefs.GetString("JwtToken");
    }
    
    public async UniTask LoginAsync(string username, string password)
    {
        LoginDTO loginDto = new LoginDTO { Username = username, Password = password };
        try
        {
            var response = await _gameClient.PostAsync<JObject>(_endpoint, loginDto);
            
            var token = response["token"].Value<string>();
            var time = response["time"].Value<DateTime>();
            var accountDTO = response["account"].ToObject<AccountDTO>();
            
            App.GetManager<TimeManager>().Init(time);
            _nickname = accountDTO.Nickname;
            JwtToken = token;
            PlayerPrefs.SetString("JwtToken", JwtToken);
            PlayerPrefs.Save();
            
            Debug.Log($"Login Success: Token: {token}, Time: {time}");
            
            // test code
            var user = await _gameClient.GetAsync<AccountDTO>("api/accounts/1");
            Debug.Log($"User: {user.Nickname}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Login Failed: {ex.Message}");
        }   
    }

    public async UniTask LoginAsyncWithToken()
    {
        var endpoint = _endpoint + "/token";
        try
        {
            var response = await _gameClient.PostAsync<JObject>(endpoint, null);
            
            var time = response["time"].Value<DateTime>();
            var accountDTO = response["account"].ToObject<AccountDTO>();
            
            App.GetManager<TimeManager>().Init(time);
            _nickname = accountDTO.Nickname;
            
            Debug.Log($"Login With Token Success: Time: {time}");
            
            // test code
            var user = await _gameClient.GetAsync<AccountDTO>("api/accounts/1");
            Debug.Log($"User: {user.Nickname}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Login Failed: {ex.Message}");
        }
    }
    
    public async UniTask<AccountDTO> CreateAccountAsync(string username, string password, string nickname)
    {
        var createAccountDto = new CreateAccountDTO { Username = username, Password = password, Nickname = nickname };
        try
        {
            var createdAccount = await _gameClient.PostAsync<AccountDTO>(_endpoint, createAccountDto);
            Debug.Log($"Created player: {createdAccount.Nickname} with ID: {createdAccount.ID}");
            return createdAccount;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to create account: {ex.Message}");
            return null;
        }
    }
        
#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 100, 300, 100));
        _userName = GUILayout.TextField(_userName, 20);
        _password = GUILayout.TextField(_password, 20);
        
        if (GUILayout.Button("Login"))
        {
            if (string.IsNullOrEmpty(JwtToken))
            {
                LoginAsync(_userName, _password).Forget();
            }
            else
            {
                LoginAsyncWithToken().Forget();
            }
        }
        
        if (GUILayout.Button("Create Account"))
        {
            CreateAccountAsync(_userName, _password, _nickname).Forget();
        }
        
        GUILayout.EndArea();
    }
#endif
}
