
using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MinimoShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

[DefaultExecutionOrder(-30)]
public class LoginManager : ManagerBase
{
    private GameClient _gameClient;
    
    [SerializeField] private string _userName = "";
    [SerializeField] private string _password = "";
    [SerializeField] private string _nickname = "";
    
    private readonly string _endpoint = "api/login";
    
    public string JwtToken { get; private set; }
    private bool IsLoggedIn { get; set; } = false;
    
    private void Start()
    {
        _gameClient = App.Services.GetRequiredService<GameClient>();
        JwtToken = PlayerPrefs.GetString("JwtToken");
    }
    
    public void Logout()
    {
        JwtToken = "";
        PlayerPrefs.DeleteKey("JwtToken");
        PlayerPrefs.Save();
        IsLoggedIn = false;
    }
    
    public async UniTask<(bool, string)> LoginAsync(string username, string password)
    {
        if(IsLoggedIn)
        {
            return (false, "Already Logged In");
        }
        
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
            IsLoggedIn = true;
            
            return (true, $"Login Success: Token: {token}, Time: {time}, NickName: {_nickname}");
            
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Login Failed: {ex.Message}");
            return (false, $"Login Failed.");
        }   
    }

    public async UniTask<(bool, string)> LoginWithTokenAsync()
    {
        if(IsLoggedIn)
        {
            return (false, "Already Logged In");
        }
        
        var endpoint = _endpoint + "/token";
        try
        {
            var response = await _gameClient.PostAsync<JObject>(endpoint, null);
            
            var time = response["time"].Value<DateTime>();
            var accountDTO = response["account"].ToObject<AccountDTO>();
            
            App.GetManager<TimeManager>().Init(time);
            _nickname = accountDTO.Nickname;
            IsLoggedIn = true;
            
            Debug.Log($"Login With Token Success: Token: {JwtToken} Time: {time} NickName: {_nickname}");
            
            return (true, $"Login Success: Time: {time}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Login Failed: {ex.Message}");
            return (false, $"Login Failed.");
        }
    }
    
    public async UniTask<(bool, string)> CreateAccountAsync(string username, string password, string nickname)
    {
        var createAccountDto = new CreateAccountDTO { Username = username, Password = password, Nickname = nickname };
        try
        {
            var createdAccount = await _gameClient.PostAsync<AccountDTO>(_endpoint, createAccountDto);
            Debug.Log($"Created player: {createdAccount.Nickname} with ID: {createdAccount.ID}");
            return (true, $"Successfully registered.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to create account: {ex.Message}");
            return (false, $"Failed to register the account.");
        }
    }
}
