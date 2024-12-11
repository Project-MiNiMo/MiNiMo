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
    public int MaxOverlap;
    public bool CanSell;
    public int BuyCost;
    public int SellCost;
    public int CashCost;
    public string Name;
    public string Description;
}

[Serializable]
public class StarTreeData
{
    public int ID;
    public int LimitTime;
    public int StarCoin;
    public int HPI;
    public int EXP;
    public int VisitMinimoLimit;
}

[Serializable]
public class FlatProduceData
{
    public string ID;
    public string Materials;
    public string Results;
    public int Time;
    public int EXP;
}

[Serializable]
public class ProduceData
{
    public string ID;
    public ProduceOption[] ProduceOptions;
}

[Serializable]
public class ProduceOption
{
    public ProduceMaterial[] Materials;
    public ProduceResult[] Results;
    public int Time;
    public int EXP;
}

[Serializable]
public class ProduceMaterial
{
    public string Code;
    public int Amount;
}

[Serializable]
public class ProduceResult
{
    public string Code;
    public int Amount;
}

[Serializable]
public class ConstructData
{
    public string ID;
    public string MatCode1;
    public int MatAmount1;
    public string MatCode2;
    public int MatAmount2;
    public int Duration;
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
    public ItemSO ItemSO;
    public Dictionary<string, int> Common { get; private set; } = new();
    public Dictionary<string, BuildingData> Building { get; private set; } = new();
    public Dictionary<string, ItemData> Item { get; private set; } = new();
    public Dictionary<int, StarTreeData> StarTree { get; private set; } = new();
    public Dictionary<string, ConstructData> Construct { get; private set; } = new();
    public Dictionary<string, ProduceData> Produce { get; private set; } = new();

    private Dictionary<string, StringData> _string = new();

    private bool _isGameDataLoaded = false;

    #region Data Path
    private const string STRING_PATH = "Data/StringData";
    private const string COMMON_PATH = "Data/CommonData";
    private const string BUILDING_PATH = "Data/BuildingData";
    private const string ITEM_PATH = "Data/ItemData";
    private const string STARTREE_PATH = "Data/StarTreeData";
    private const string PRODUCE_PATH = "Data/ProduceData";
    private const string CONSTRUCT_PATH = "Data/ConstructData";
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
        StarTree.Clear();
        Produce.Clear();
        Construct.Clear();

        var stringDataRaw = DataLoader.LoadData<StringData>(STRING_PATH);
        var commonDataRaw = DataLoader.LoadData<CommonData>(COMMON_PATH);
        var buildingDataRaw = DataLoader.LoadData<BuildingData>(BUILDING_PATH);
        var itemDataRaw = DataLoader.LoadData<ItemData>(ITEM_PATH);
        var starTreeDataRaw = DataLoader.LoadData<StarTreeData>(STARTREE_PATH);
        var produceDataRaw = DataGrouper.GroupData(PRODUCE_PATH);
        var constructDataRaw = DataLoader.LoadData<ConstructData>(CONSTRUCT_PATH);

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

        foreach (var data in starTreeDataRaw)
        {
            StarTree.Add(data.ID, data);
        }

        foreach (var data in produceDataRaw)
        {
            Produce.Add(data.ID, data);       
        }

        foreach (var data in constructDataRaw)
        {
            Construct.Add(data.ID, data);
        }

        foreach (var item in ItemSO.items) //TEMP
        {
            item.SetData(Item[item.Code]);
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
