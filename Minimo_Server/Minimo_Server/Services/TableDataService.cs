using MinimoServer.Data.Class;

namespace MinimoServer.Services;


public class TableDataService
{
    public Dictionary<string, int> Common { get; private set; } = new();
    public Dictionary<string, BuildingData> Building { get; private set; } = new();
    public Dictionary<string, ItemData> Item { get; private set; } = new();
    public Dictionary<int, StarTreeData> StarTree { get; private set; } = new();
    public Dictionary<string, ConstructData> Construct { get; private set; } = new();
    public Dictionary<string, ProduceData> Produce { get; private set; } = new();

    public int MaxBuildingCount => 5;
    public int DefaultProduceSlotCount => 3;
    public int MaxProduceSlotCount => 5;

    private Dictionary<string, StringData> _string = new();

    private bool _isGameDataLoaded = false;

    #region Data Path
    private const string STRING_PATH = "Data/StringData.json";
    private const string COMMON_PATH = "Data/CommonData.json";
    private const string BUILDING_PATH = "Data/BuildingData.json";
    private const string ITEM_PATH = "Data/ItemData.json";
    private const string STARTREE_PATH = "Data/StarTreeData.json";
    private const string PRODUCE_PATH = "Data/ProduceData.json";
    private const string CONSTRUCT_PATH = "Data/ConstructData.json";
    #endregion

    public TableDataService()
    {
        LoadData();
    }

    /// <summary>
    /// 주어진 buildingType과 recipeId에 해당하는 ProduceOption을 반환한다.
    /// </summary>
    /// <param name="buildingType"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public ProduceOption? GetProduceRecipe(string buildingType, int recipeId)
    {
        return Produce[buildingType]?.ProduceOptions[recipeId - 1];
    }

    public int GetRandomQuestIndex()
    {
        return 0; // TODO 퀘스트 테이블이 없음
    }

    public int GetRandomItemIndex()
    {
        return Item.Count > 0 ? new Random().Next(0, Item.Count + 1) : 0; // TODO
    }

    public int GetRandomItemCount()
    {
        return new Random().Next(1, 10); // TODO: 더미 값
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

        _isGameDataLoaded = true;
    }
}

