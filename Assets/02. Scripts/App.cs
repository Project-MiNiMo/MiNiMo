using System;
using System.Collections.Generic;

using UnityEngine;
using DG.Tweening;

public enum SceneName
{
    Developer,
    Title,
    Game
}

public class App : Singleton<App>
{
    private Dictionary<Type, MonoBehaviour> _managers = new();
    private Dictionary<Type, MonoBehaviour> _datas = new();

    protected override void Awake()
    {
        base.Awake();

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;

        DOTween.safeModeLogBehaviour = DG.Tweening.Core.Enums.SafeModeLogBehaviour.Error;
    }

    private void Register(MonoBehaviour obj, Dictionary<Type, MonoBehaviour> dictionary)
    {
        var type = obj.GetType();

        if (!dictionary.ContainsKey(type))
        {
            dictionary.Add(type, obj);
        }
        else
        {
            dictionary[type] = obj;
        }
    }

    public void RegisterManager(MonoBehaviour manager)
    {
        Register(manager, _managers);
    }

    public void RegisterData(MonoBehaviour data)
    {
        Register(data, _datas);
    }

    public T GetManager<T>() where T : MonoBehaviour
    {
        var type = typeof(T);

        if (_managers.TryGetValue(type, out MonoBehaviour manager))
        {
            return manager as T;
        }

        Debug.LogWarning($"Manager of type {type.Name} not found.");
        return null;
    }

    public T GetData<T>() where T : MonoBehaviour
    {
        var type = typeof(T);

        if (_datas.TryGetValue(type, out MonoBehaviour data))
        {
            return data as T;
        }

        Debug.LogWarning($"Data of type {type.Name} not found.");
        return null;
    }

    public static void LoadScene(SceneName sceneName)
    {
        DOTween.KillAll();

        UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneName);
    }
}