using UnityEngine;
using TMPro;

public abstract class OptionBase : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _titleTMP;

    protected SettingData _setting;

    protected virtual void Awake()
    {
        _setting = App.Instance.GetData<SettingData>();
    }

    protected virtual void Start()
    {
        SetString();
        SetButtonEvent();
    }

    protected virtual void OnEnable()
    {
        SetValueFromData();
    }

    protected abstract void SetString();

    protected abstract void SetButtonEvent();

    protected abstract void SetValueFromData();

    public abstract void SaveOption();
}
