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
    [SerializeField] private Image blackBlur;

    private Dictionary<Type, UIBase> uiDictionary;
    private Stack<UIState> uiStack;

    public UIState CurrentState => uiStack.Count > 0 ? uiStack.Peek() : UIState.Normal;

    protected override void Awake()
    {
        base.Awake();

        var uiPanels = GetComponentsInChildren<UIBase>(true);

        uiDictionary = new(uiPanels.Length);
        uiStack = new();

        foreach (var panel in uiPanels)
        {
            uiDictionary.Add(panel.GetType(), panel);
        }
    }

    private void Start()
    {
        foreach (var UI in uiDictionary.Values)
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
        if (uiDictionary.TryGetValue(typeof(T), out UIBase panel))
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
        uiStack.Push(state);
    }

    public void PopUIState(UIState state)
    {
        if (CurrentState == state)
        {
            uiStack.Pop();
        }
    }
    #endregion

    #region Fade In / Out
    public void FadeIn(Action onComplete = null)
    {
        blackBlur.gameObject.SetActive(true);

        blackBlur.DOKill();
        blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void FadeOut(Action onComplete = null)
    {
        blackBlur.DOKill();
        blackBlur.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            blackBlur.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void FadeInOut(Action midAction = null)
    {
        blackBlur.gameObject.SetActive(true);

        blackBlur.DOKill();
        blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            midAction?.Invoke();
            FadeOut();
        });
    }
    #endregion
}
