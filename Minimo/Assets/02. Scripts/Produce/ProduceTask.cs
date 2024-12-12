using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceTask
{
    public ProduceOption Data { get; private set; }
    public int RemainTime { get; private set; }

    public ProduceTask(ProduceOption produceOption)
    {
        Data = produceOption;
        
        RemainTime = produceOption.Time;
    }
    
    public void UpdateHarvestTime()
    { 
        RemainTime = Mathf.Max(0, RemainTime - 1);
    }
    
    public void HarvestEarly()
    {
        RemainTime = 0;
    }
}
