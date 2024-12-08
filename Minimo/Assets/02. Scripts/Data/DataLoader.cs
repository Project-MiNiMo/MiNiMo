using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

using UnityEngine;

public class DataLoader
{
    public static T[] LoadData<T>(string dataPath)
    {
        var json = Resources.Load<TextAsset>(dataPath);

        if (json)
        {
            var stringDataList = JsonUtilityHelper.FromJson<T>(json.ToString());

            return stringDataList;
        }

        return null;
    }
}

public class JsonUtilityHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson;

        if (json[0] == '{')
        {
            newJson = json;
        }
        else
        {
            newJson = "{ \"array\": " + json + "}";
        }

        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new();
        wrapper.array = array;

        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

public class DataGrouper
{
    public static ProduceData[] GroupData(string path)
    {
        var json = Resources.Load<TextAsset>(path).text;
        
        // Deserialize JSON array into a flat list of FlatData
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

