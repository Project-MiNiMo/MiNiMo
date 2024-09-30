using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public enum EventCode
{
    EditStart,
    EditEnd,
}

public class EventManager : ManagerBase
{
    private Dictionary<EventCode, List<IEventListener>> _listeners = new();

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

    public void AddListener(EventCode code, IEventListener listener)
    {
        if (!_listeners.TryGetValue(code, out var listenerList))
        {
            listenerList = new();

            _listeners.Add(code, listenerList);
        }

        if (!listenerList.Contains(listener))
        {
            listenerList.Add(listener);
        }
    }

    public void RemoveListener(EventCode code, IEventListener listener)
    {
        if (_listeners.TryGetValue(code, out var listenerList))
        {
            listenerList.Remove(listener);

            if (listenerList.Count == 0)
            {
                _listeners.Remove(code);
            }
        }
    }

    public void PostEvent(EventCode code, Component sender, object param = null)
    {
        if (_listeners.TryGetValue(code, out var listenerList))
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
        foreach (var key in _listeners.Keys)
        {
            _listeners[key].RemoveAll(listener => listener == null);
        }
    }
}
