using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CommonData
{
    public string ID;
    public int Value;
}

[Serializable]
public class BuildingData
{
    public string ID;
    public int Type;
    public int SizeX;
    public int SizeY;
    public int UnlockLevel;
    public string Icon;
    public string Name;
    public string Description;
}

[Serializable]
public class ItemData
{
    public string ID;
    public int Type;
    public int Grade;
    public int MaxOverlap;
    public bool CanSell;
    public int BuyCost;
    public int SellCost;
    public int BuyCost_Cash;
    public string Icon;
    public string Name;
    public string Description;
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
    public Dictionary<string, CommonData> Common { get; private set; } = new();
    public Dictionary<string, BuildingData> Building { get; private set; } = new();
    public Dictionary<string, ItemData> Item { get; private set; } = new();

    private Dictionary<string, StringData> _string = new();

    private bool _isGameDataLoaded = false;

    #region Data Path
    private const string STRING_PATH = "Data/StringData";
    private const string COMMON_PATH = "Data/CommonData";
    private const string BUILDING_PATH = "Data/BuildingData";
    private const string ITEM_PATH = "Data/ItemData";
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
        Common.Clear();
        Building.Clear();
        Item.Clear();

        var stringDataRaw = DataLoader.LoadData<StringData>(STRING_PATH);
        var commonDataRaw = DataLoader.LoadData<CommonData>(COMMON_PATH);
        var buildingDataRaw = DataLoader.LoadData<BuildingData>(BUILDING_PATH);
        var itemDataRaw = DataLoader.LoadData<ItemData>(ITEM_PATH);

        foreach (var data in stringDataRaw)
        {
            _string.Add(data.ID, data);
        }

        foreach (var data in commonDataRaw)
        {
            Common.Add(data.ID, data);
        }

        foreach (var data in buildingDataRaw)
        {
            Building.Add(data.ID, data);
        }

        foreach (var data in itemDataRaw)
        {
            Item.Add(data.ID, data);
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
        return TryGetString(_code, App.GetData<SettingData>().Language, out str);
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
