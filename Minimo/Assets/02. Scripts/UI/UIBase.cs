using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public virtual UIState UIState => UIState.Normal;

    /// <summary>
    /// Is Panel need to add ui stack?
    /// </summary>
    /// <returns></returns>
    public virtual bool IsAddUIStack => false;

    /// <summary>
    /// Initialize Panel.
    /// Called once on Awake.
    /// </summary>
    public abstract void Initialize();

    public virtual void OpenPanel()
    {
        if (IsAddUIStack && !gameObject.activeSelf)
        {
            App.GetManager<UIManager>().PushUIState(UIState);
        }

        gameObject.SetActive(true);
    }

    public virtual void ClosePanel()
    {
        if (IsAddUIStack && gameObject.activeSelf)
        {
            App.GetManager<UIManager>().PopUIState(UIState);
        }

        gameObject.SetActive(false);
    }
}
