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
    public int HPI;
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
    public int CashCost;
    public string Icon;
    public string Name;
    public string Description;
}

[Serializable]
public class ProduceData
{
    public string ID;
    public string Fst_MaterialCode1;
    public string Fst_MaterialAmount1;
    public string Fst_MaterialCode2;
    public string Fst_MaterialAmount2;
    public string Fst_ResultCode;
    public string Fst_ResultAmount;
    public string Fst_Time;
    public string Fst_EXP;
    public string Snd_MaterialCode1;
    public string Snd_MaterialAmount1;
    public string Snd_MaterialCode2;
    public string Snd_MaterialAmount2;
    public string Snd_ResultCode;
    public string Snd_ResultAmount;
    public string Snd_Time;
    public string Snd_EXP;
    public string Trd_MaterialCode1;
    public string Trd_MaterialAmount1;
    public string Trd_MaterialCode2;
    public string Trd_MaterialAmount2;
    public string Trd_ResultCode;
    public string Trd_ResultAmount;
    public string Trd_Time;
    public string Trd_EXP;
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
    public Dictionary<string, ProduceData> Produce { get; private set; } = new();

    private Dictionary<string, StringData> _string = new();

    private bool _isGameDataLoaded = false;

    #region Data Path
    private const string STRING_PATH = "Data/StringData";
    private const string COMMON_PATH = "Data/CommonData";
    private const string BUILDING_PATH = "Data/BuildingData";
    private const string ITEM_PATH = "Data/ItemData";
    private const string PRODUCE_PATH = "Data/ProduceData";
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
        Produce.Clear();

        var stringDataRaw = DataLoader.LoadData<StringData>(STRING_PATH);
        var commonDataRaw = DataLoader.LoadData<CommonData>(COMMON_PATH);
        var buildingDataRaw = DataLoader.LoadData<BuildingData>(BUILDING_PATH);
        var itemDataRaw = DataLoader.LoadData<ItemData>(ITEM_PATH);
        var produceDataRaw = DataLoader.LoadData<ProduceData>(PRODUCE_PATH);

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

        foreach (var data in produceDataRaw)
        {
            Produce.Add(data.ID, data);
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
