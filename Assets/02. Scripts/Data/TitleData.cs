using System;
using System.Collections.Generic;

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

    private bool isGameDataLoaded = false;

    #region Data Path
    private const string gridObjectDataPath = "Data/GridObjectData";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        LoadData();
    }

    public void LoadData()
    {
        if (isGameDataLoaded)
        {
            return;
        }

        GridObject.Clear();

        var valueDataRaw = DataLoader.LoadData<GridObjectData>(gridObjectDataPath);

        foreach (var data in valueDataRaw)
        {
            GridObject.Add(data.Code, data);
        }

        isGameDataLoaded = true;
    }
}
