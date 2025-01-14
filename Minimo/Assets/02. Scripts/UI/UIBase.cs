using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected virtual GameObject Panel => gameObject;

    /// <summary>
    /// Initialize Panel.
    /// Called once on Awake.
    /// </summary>
    public abstract void Initialize();

    public virtual void OpenPanel()
    {
        Panel.SetActive(true);
    }

    public virtual void ClosePanel()
    {
        Panel.SetActive(false);
    }
}
