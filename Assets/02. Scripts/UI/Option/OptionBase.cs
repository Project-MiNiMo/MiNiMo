using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class OptionBase : MonoBehaviour
{
    [Serializable]
    protected struct ToggleOption
    {
        public TextMeshProUGUI Title;
        public Toggle LeftToggle;
        public Toggle RightToggle;
    }

    protected SettingData _setting;

    protected virtual void Awake()
    {
        _setting = App.GetData<SettingData>();
    }

    protected virtual void Start()
    {
        SetString();
        SetEvent();
    }

    protected virtual void OnEnable()
    {
        SetValueFromData();
    }

    protected abstract void SetString();

    protected abstract void SetEvent();

    protected abstract void SetValueFromData();

    public abstract void SaveOption();
}
