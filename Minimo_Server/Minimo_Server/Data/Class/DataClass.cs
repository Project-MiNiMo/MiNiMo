using System.Net.Mime;
using Newtonsoft.Json;

namespace MinimoServer.Data.Class;

public class DataLoader
{
    public static T[] LoadData<T>(string dataPath)
    {
        if (!File.Exists(dataPath))
            throw new FileNotFoundException($"File not found at path: {dataPath}");
        
        // read json file from dataPath
        
        var json = File.ReadAllText(dataPath);

        if ((!string.IsNullOrEmpty(json)))
        {
            return JsonUtilityHelper.FromJson<T>(json);
        }

        return Array.Empty<T>();
    }
}

public class JsonUtilityHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = json.TrimStart();

        if (newJson.StartsWith("{"))
        {
            return JsonConvert.DeserializeObject<Wrapper<T>>(newJson)?.array ?? Array.Empty<T>();
        }
        else
        {
            newJson = "{ \"array\": " + json + "}";
            return JsonConvert.DeserializeObject<Wrapper<T>>(newJson)?.array ?? Array.Empty<T>();
        }
    }

    public static string ToJson<T>(T[] array)
    {
        var wrapper = new Wrapper<T> { array = array };

        return JsonConvert.SerializeObject(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

public class DataGrouper
{
    public static ProduceData[] GroupData(string jsonPath)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException($"File not found at path: {jsonPath}");
        
        var json = File.ReadAllText(jsonPath);
        var rawData = JsonConvert.DeserializeObject<List<FlatProduceData>>(json);

        // Group by ID and map to ProduceData structure
        var groupedData = rawData
            .GroupBy(entry => entry.ID) // Group by Building ID
            .Select(group => new ProduceData
            {
                ID = group.Key,
                ProduceOptions = group.Select(option => new ProduceOption
                {
                    Materials = ParseMaterials(option.Materials),
                    Results = ParseResults(option.Results),
                    Time = option.Time,
                    EXP = option.EXP
                }).ToArray()
            }).ToArray();

        return groupedData;
    }
    
    private static ProduceMaterial[] ParseMaterials(string materialsRaw)
    {
        if (string.IsNullOrEmpty(materialsRaw)) return Array.Empty<ProduceMaterial>();

        return materialsRaw.Split(',').Select(mat =>
        {
            var parts = mat.Split(':').Select(p => p.Trim()).ToArray();
            return new ProduceMaterial
            {
                Code = parts[0],
                Amount = int.Parse(parts[1])
            };
        }).ToArray();
    }

    private static ProduceResult[] ParseResults(string resultsRaw)
    {
        if (string.IsNullOrEmpty(resultsRaw)) return Array.Empty<ProduceResult>();

        return resultsRaw.Split(',').Select(res =>
        {
            var parts = res.Split(':').Select(p => p.Trim()).ToArray();
            return new ProduceResult
            {
                Code = parts[0],
                Amount = int.Parse(parts[1])
            };
        }).ToArray();
    }
}

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

public class ItemSO
{
    public List<Item> items;
    
    public Item GetItem(string code)
    {
        return items.Find(item => item.Code == code);
    }
}

public class Item
{
    public string Code;
    public int Count;

    public ItemData Data { get; private set; }
    //public Sprite Icon { get; private set; }

    public void SetData(ItemData _data)
    {
        Data = _data;
        Count = 0;

        //Icon = Resources.Load<Sprite>($"Item/{_data.ID}");
    }
}