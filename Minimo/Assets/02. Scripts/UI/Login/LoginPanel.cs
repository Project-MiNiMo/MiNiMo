using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _pwInputField;
    [SerializeField] private Button _loginBtn;
    [SerializeField] private Button _registerBtn;
    [SerializeField] private TextMeshProUGUI _resultText;
    
    private LoginManager _loginManager;
    //private bool isLogin;
    
    private async void Start()
    {
        _loginBtn.onClick.AddListener(OnLogin);
        _registerBtn.onClick.AddListener(OnRegister);
        
        _loginManager = App.GetManager<LoginManager>();
        if (!String.IsNullOrEmpty(_loginManager.JwtToken))
        {
            var result = await _loginManager.LoginWithTokenAsync();
            if (result.Item1)
            {
                App.LoadScene(SceneName.Game);
            }
            else
            {
                _resultText.text = result.Item2;
            }
        }
    }
    
    private async void OnLogin()
    {
        var id = ThrowHelper.IfNullOrWhitespace(_idInputField.text);
        var pw = ThrowHelper.IfNullOrWhitespace(_pwInputField.text);

        // 로그인 요청
        var result = await _loginManager.LoginAsync(id, pw);
        if(result.Item1)
        {
            App.LoadScene(SceneName.Game);
        }
        else
        {
            _resultText.text = result.Item2;
        }
    }
    
    private async void OnRegister()
    {
        var id = ThrowHelper.IfNullOrWhitespace(_idInputField.text);
        var pw = ThrowHelper.IfNullOrWhitespace(_pwInputField.text);
        var randomNickname = "User" + UnityEngine.Random.Range(0, 1000);
        
        // 회원가입 요청
        var result = await _loginManager.CreateAccountAsync(id, pw, randomNickname);
        _resultText.text = result.Item2;
    }
}

public static class ThrowHelper
{
    public static string IfNullOrWhitespace(string argument)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException("Argument is whitespace");
        }

        return argument;
    }
}