using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ProducePrimary : ProduceObject
{
    public override bool IsPrimary => true;
    
    [SerializeField] private SpriteRenderer _cropSpriteRenderer;
    
    protected List<Sprite[]> _cropSprites;
   
    private Sprite[] _currentCropSprites;
    private int _currentSpriteIndex;
  
    protected override void Update()
    {
        base.Update();

        if (AllTasks.Count == 0) 
        {
            return;
        }
        
        if (_currentCropSprites == null) 
        {
            return;
        }

        if (AllTasks.Any(task => task.RemainTime <= 0))
        {
            return;
        }

        SetCropSprite();
    }

    private void SetCropSprite()
    {
        var remainPercent = (float)ActiveTask.RemainTime / ActiveTask.Data.Time;

        var newSpriteIndex = remainPercent switch
        {
            >= 0.5f => 0,
            >= 0.01f => 1,
            _ => 2
        };

        if (newSpriteIndex != _currentSpriteIndex)
        {
            _currentSpriteIndex = newSpriteIndex;
            _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
        }
    }
    
    protected override void CompleteActiveTask()
    {
        base.CompleteActiveTask();
        
        _currentSpriteIndex = 2;
        _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
    }
    
    protected override void OnPlant(ProduceTask task)
    {
        if (AllTasks.Count > 0)
        {
            return;
        }
        
        base.OnPlant(task);
        
        _currentSpriteIndex = 0;

        var cropCode = ActiveTask.Data.Results[0].Code;
        _currentCropSprites = _cropSprites[GetCropType(cropCode)];
        _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
    }
    
    public override void StartHarvest()
    {
        base.StartHarvest();

        if (ActiveTask == null)
        {
            _cropSpriteRenderer.sprite = null;
        }
        else
        {
            SetCropSprite();
        }
    }

    protected abstract int GetCropType(string cropCode);
}
