using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public enum EventCode
{
    EditStart,
    EditEnd,
}

public interface IListener
{
    void OnEvent(EventCode code, Component sender, object param = null);
}

public class EventManager : ManagerBase
{
    private Dictionary<EventCode, List<IListener>> listeners = new();

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveNullListeners();
    }

    public void AddListener(EventCode code, IListener listener)
    {
        if (!listeners.TryGetValue(code, out var listenerList))
        {
            listenerList = new();
            listeners.Add(code, listenerList);
        }

        if (!listenerList.Contains(listener))
        {
            listenerList.Add(listener);
        }
    }

    public void RemoveListener(EventCode code, IListener listener)
    {
        if (listeners.TryGetValue(code, out var listenerList))
        {
            listenerList.Remove(listener);
            if (listenerList.Count == 0)
            {
                listeners.Remove(code);
            }
        }
    }

    public void PostEvent(EventCode code, Component sender, object param = null)
    {
        if (listeners.TryGetValue(code, out var listenerList))
        {
            foreach (var listener in listenerList)
            {
                if (listener != null)
                {
                    listener.OnEvent(code, sender, param);
                }
            }
        }
    }

    public void RemoveNullListeners()
    {
        foreach (var key in listeners.Keys)
        {
            listeners[key].RemoveAll(listener => listener == null);
        }
    }
}
