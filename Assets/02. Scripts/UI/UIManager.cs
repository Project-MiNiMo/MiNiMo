using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum UIState
{
    Normal,
}

public class UIManager : ManagerBase
{
    [SerializeField] private Image _blackBlur;

    private Dictionary<Type, UIBase> _uiDictionary;
    private Stack<UIState> _uiStack;

    public UIState CurrentState => _uiStack.Count > 0 ? _uiStack.Peek() : UIState.Normal;

    protected override void Awake()
    {
        base.Awake();

        var uiPanels = GetComponentsInChildren<UIBase>(true);

        _uiDictionary = new(uiPanels.Length);
        _uiStack = new();

        foreach (var panel in uiPanels)
        {
            _uiDictionary.Add(panel.GetType(), panel);
        }
    }

    private void Start()
    {
        foreach (var UI in _uiDictionary.Values)
        {
            if (!UI.gameObject.activeSelf) //wake up panels
            {
                UI.gameObject.SetActive(true);
                UI.gameObject.SetActive(false);
            }

            try { UI.Initialize(); }
            catch (Exception error)
            { Debug.LogError($"ERROR: {error.Message}\n{error.StackTrace}"); }
        }
    }

    #region Get Panel
    public T GetPanel<T>() where T : UIBase
    {
        if (_uiDictionary.TryGetValue(typeof(T), out UIBase panel))
        {
            return panel as T;
        }

        Debug.LogError($"Panel of type {typeof(T)} not found.");
        return null;
    }
    #endregion

    #region UI Stack Management
    public void PushUIState(UIState state)
    {
        _uiStack.Push(state);
    }

    public void PopUIState(UIState state)
    {
        if (CurrentState == state)
        {
            _uiStack.Pop();
        }
    }
    #endregion

    #region Fade In / Out
    public void FadeIn(Action onComplete = null)
    {
        _blackBlur.gameObject.SetActive(true);

        _blackBlur.DOKill();
        _blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void FadeOut(Action onComplete = null)
    {
        _blackBlur.DOKill();
        _blackBlur.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _blackBlur.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void FadeInOut(Action midAction = null)
    {
        _blackBlur.gameObject.SetActive(true);

        _blackBlur.DOKill();
        _blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            midAction?.Invoke();
            FadeOut();
        });
    }
    #endregion
}
