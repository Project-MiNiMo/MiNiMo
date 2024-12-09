using UniRx;

public class ProduceManager : ManagerBase
{
    public ReactiveProperty<bool> IsProducing { get; } = new(false);
    public ReactiveProperty<int> CurrentRemainTime { get; } = new(-1);
    public ProduceObject CurrentProduceObject { get; private set; }

    public void ActiveProduce(ProduceObject produceObject)
    {
        if (CurrentProduceObject && CurrentProduceObject != produceObject)
        {
            DeactiveProduce();
        }
        
        CurrentProduceObject = produceObject;
        IsProducing.Value = true;
    }
    
    public void DeactiveProduce()
    {
        CurrentProduceObject = null;
        IsProducing.Value = false;
        
        CurrentRemainTime.Value = -1;
    }

    public void SetRemainTime(int remainTime)
    {
        CurrentRemainTime.Value = remainTime;
    }
}
