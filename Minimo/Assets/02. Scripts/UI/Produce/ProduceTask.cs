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
        --RemainTime;
    }
    
    public void HarvestEarly()
    {
        RemainTime = 0;
    }
}
