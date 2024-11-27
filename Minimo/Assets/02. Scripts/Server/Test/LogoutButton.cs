using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    private void Awake()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() =>
        {
            App.GetManager<LoginManager>().Logout();
            SceneManager.LoadScene("96. Login");
        });
    }
}
