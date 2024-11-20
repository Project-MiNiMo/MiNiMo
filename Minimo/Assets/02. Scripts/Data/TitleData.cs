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
public class ProduceSingleData
{
    public string ID;
    public string Fst_MaterialCode;
    public int Fst_MaterialAmount;
    public string Fst_ResultCode;
    public int Fst_ResultAmount;
    public int Fst_Time;
    public int Fst_EXP;
    public string Snd_MaterialCode;
    public int Snd_MaterialAmount;
    public string Snd_ResultCode;
    public int Snd_ResultAmount;
    public int Snd_Time;
    public int Snd_EXP;
    public string Trd_MaterialCode;
    public int Trd_MaterialAmount;
    public string Trd_ResultCode;
    public int Trd_ResultAmount;
    public int Trd_Time;
    public int Trd_EXP;
    public string Fourth_MaterialCode;
    public int Fourth_MaterialAmount;
    public string Fourth_ResultCode;
    public int Fourth_ResultAmount;
    public int Fourth_Time;
    public int Fourth_EXP;
    public string Fifth_MaterialCode;
    public int Fifth_MaterialAmount;
    public string Fifth_ResultCode;
    public int Fifth_ResultAmount;
    public int Fifth_Time;
    public int Fifth_EXP;
}

[Serializable]
public class ProduceMultipleData
{
    public string ID;
    public string Fst_MaterialCode1;
    public int Fst_MaterialAmount1;
    public string Fst_MaterialCode2;
    public int Fst_MaterialAmount2;
    public string Fst_ResultCode;
    public int Fst_ResultAmount;
    public int Fst_Time;
    public int Fst_EXP;
    public string Snd_MaterialCode1;
    public int Snd_MaterialAmount1;
    public string Snd_MaterialCode2;
    public int Snd_MaterialAmount2;
    public string Snd_ResultCode;
    public int Snd_ResultAmount;
    public int Snd_Time;
    public int Snd_EXP;
    public string Trd_MaterialCode1;
    public int Trd_MaterialAmount1;
    public string Trd_MaterialCode2;
    public int Trd_MaterialAmount2;
    public string Trd_ResultCode;
    public int Trd_ResultAmount;
    public int Trd_Time;
    public int Trd_EXP;
    public string Fourth_MaterialCode1;
    public int Fourth_MaterialAmount1;
    public string Fourth_MaterialCode2;
    public int Fourth_MaterialAmount2;
    public string Fourth_ResultCode;
    public int Fourth_ResultAmount;
    public int Fourth_Time;
    public int Fourth_EXP;
    public string Fifth_MaterialCode1;
    public int Fifth_MaterialAmount1;
    public string Fifth_MaterialCode2;
    public int Fifth_MaterialAmount2;
    public string Fifth_ResultCode;
    public int Fifth_ResultAmount;
    public int Fifth_Time;
    public int Fifth_EXP;
}

[Serializable]
public class StringData
{
    public string ID;
    public string Korean;
    public string English;
    public string Chinese;
    public string Japanese;
}

public class TitleData : DataBase
{
    public Dictionary<string, int> Common { get; private set; } = new();
    public Dictionary<string, BuildingData> Building { get; private set; } = new();
    public Dictionary<string, ItemData> Item { get; private set; } = new();
    public Dictionary<string, ProduceSingleData> ProduceSingle { get; private set; } = new();
    public Dictionary<string, ProduceMultipleData> ProduceMultiple { get; private set; } = new();

    private Dictionary<string, StringData> _string = new();

    private bool _isGameDataLoaded = false;

    #region Data Path
    private const string STRING_PATH = "Data/StringData";
    private const string COMMON_PATH = "Data/CommonData";
    private const string BUILDING_PATH = "Data/BuildingData";
    private const string ITEM_PATH = "Data/ItemData";
    private const string PRODUCESINGLE_PATH = "Data/ProduceSingleData";
    private const string PRODUCEMULTIPLE_PATH = "Data/ProduceMultipleData";
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
        ProduceSingle.Clear();
        ProduceMultiple.Clear();

        var stringDataRaw = DataLoader.LoadData<StringData>(STRING_PATH);
        var commonDataRaw = DataLoader.LoadData<CommonData>(COMMON_PATH);
        var buildingDataRaw = DataLoader.LoadData<BuildingData>(BUILDING_PATH);
        var itemDataRaw = DataLoader.LoadData<ItemData>(ITEM_PATH);
        var produceSingleDataRaw = DataLoader.LoadData<ProduceSingleData>(PRODUCESINGLE_PATH);
        var produceMultipleDataRaw = DataLoader.LoadData<ProduceMultipleData>(PRODUCEMULTIPLE_PATH);

        foreach (var data in stringDataRaw)
        {
            _string.Add(data.ID, data);
        }

        foreach (var data in commonDataRaw)
        {
            Common.Add(data.ID, data.Value);
        }

        foreach (var data in buildingDataRaw)
        {
            Building.Add(data.ID, data);
        }

        foreach (var data in itemDataRaw)
        {
            Item.Add(data.ID, data);
        }

        foreach (var data in produceSingleDataRaw)
        {
            ProduceSingle.Add(data.ID, data);
        }

        foreach (var data in produceMultipleDataRaw)
        {
            ProduceMultiple.Add(data.ID, data);
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
