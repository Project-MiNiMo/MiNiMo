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

public class TitleData : DataBase
{
    public Dictionary<string, GridObjectData> GridObject { get; private set; } = new();

    private bool _isGameDataLoaded = false;

    #region Data Path
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

        GridObject.Clear();

        var valueDataRaw = DataLoader.LoadData<GridObjectData>(GRID_PATH);

        foreach (var data in valueDataRaw)
        {
            GridObject.Add(data.Code, data);
        }

        _isGameDataLoaded = true;
    }
}
