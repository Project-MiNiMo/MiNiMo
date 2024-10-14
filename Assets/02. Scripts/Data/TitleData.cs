using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class GridObjectData
{
    public int Index;
    public string Code;
    public int Type;
    public string Name;
    public string Description;
    public string Sprite;
    public string Prefab;
}

[Serializable]
public class StringData
{
    public string ID;
    public string Korean;
    public string English;
    public string Japanese;
    public string Chinese;
}

public class TitleData : DataBase
{
    public Dictionary<string, GridObjectData> GridObject { get; private set; } = new();

    private Dictionary<string, StringData> _string = new Dictionary<string, StringData>();

    private bool _isGameDataLoaded = false;

    #region Data Path
    private const string STRING_PATH = "Data/StringData";
    private const string GRID_PATH = "Data/GridObjectData";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        LoadData();
    }

    public void LoadData()
    {
        if (_isGameDataLoaded)
        {
            return;
        }

        _string.Clear();
        GridObject.Clear();

        var stringDataRaw = DataLoader.LoadData<StringData>(STRING_PATH);
        var gridObjectDataRaw = DataLoader.LoadData<GridObjectData>(GRID_PATH);

        foreach (var data in stringDataRaw)
        {
            _string.Add(data.ID, data);
        }

        foreach (var data in gridObjectDataRaw)
        {
            GridObject.Add(data.Code, data);
        }

        _isGameDataLoaded = true;
    }

    public string GetString(string _code)
    {
        TryGetString(_code, out var str);
        return str;
    }

    public string GetFormatString(string _code, params string[] _args)
    {
        TryGetString(_code, out var str);

        try { return string.Format(str, _args); }
        catch (Exception error)
        { Debug.LogError($"Failed to format string {_code}. {error}"); }

        return _code;
    }

    private bool TryGetString(string _code, out string str)
    {
        return TryGetString(_code, App.Instance.GetData<SettingData>().Language, out str);
    }

    private bool TryGetString(string _code, SystemLanguage _language, out string str)
    {
        if (_code != null && _string.TryGetValue(_code, out var data))
        {
            str = _language switch
            {
                SystemLanguage.Korean => data.Korean,
                SystemLanguage.Chinese => data.Chinese,
                SystemLanguage.Japanese => data.Japanese,
                _ => data.English
            };

            return true;
        }

        str = _code;
        return false;
    }
}
