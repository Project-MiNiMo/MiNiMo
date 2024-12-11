using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class ProducePrimary : ProduceObject
{
    [SerializeField] private SpriteRenderer _cropSpriteRenderer;
    
    protected List<Sprite[]> _cropSprites;
   
    private Sprite[] _currentCropSprites;
    private int _currentSpriteIndex;
  
    protected override void Update()
    {
        base.Update();
        
        if (_currentCropSprites == null) 
        {
            return;
        }

        if (CompleteTasks.Count > 0)
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
    
    public override bool StartProduce(ProduceOption option)
    {
        if (!base.StartProduce(option)) return false;
        
        _currentSpriteIndex = 0;

        var cropCode = ActiveTask.Data.Results[0].Code;
        _currentCropSprites = _cropSprites[GetCropType(cropCode)];
        _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];

        return true;
    }

    protected abstract int GetCropType(string cropCode);

    protected override void SetCompleteTask()
    {
        base.SetCompleteTask();
        
        _currentSpriteIndex = 2;
        _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
    }
}
