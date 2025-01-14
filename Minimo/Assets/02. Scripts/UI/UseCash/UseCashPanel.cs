using System;
using System.Collections.Generic;

using UnityEngine;

public enum UseCashType
{
    Produce,
    ProduceExpand,
    ProduceMaterial,
}

public class UseCashPanel : UIBase
{
    [SerializeField] private UseCashBack _useBack;
    [SerializeField] private UseCashMaterialBack _useMaterialBack;
    
    [SerializeField] private ChargeCashBack _chargeBack;
    
    private PlayerData _playerData;
    private TitleData _titleData;
    private string _diamondCountString;
    
    private Action _useAction;
    private int _useCount;
    
    public override void Initialize()
    {
        _playerData = App.GetData<PlayerData>();
        _titleData = App.GetData<TitleData>();

        _useBack.Initialize(_playerData, _titleData, ClosePanel, OpenCharge);
        _useMaterialBack.Initialize(_playerData, _titleData, ClosePanel, OpenCharge);
        _chargeBack.Initialize(_titleData, ClosePanel);
        
        ClosePanel();
    }

    public void OpenPanel(UseCashType type, int price, Action useAction)
    {
        base.OpenPanel();

        ActiveBacks(isActiveUse: true);
        
        _useBack.gameObject.SetActive(true);
        _useBack.Setup(type, price, useAction);
    }

    public void OpenPanel(List<(Item, int)> lackItems, Action useAction)
    {
        base.OpenPanel();
        
        ActiveBacks(isActiveUse: true);
        
        _useMaterialBack.gameObject.SetActive(true);
        _useMaterialBack.Setup(lackItems, useAction);
    }
    
    private void OpenCharge()
    {
        ActiveBacks(isActiveUse: false);
    }
    
    private void ActiveBacks(bool isActiveUse)
    {
        _useBack.gameObject.SetActive(false);
        _useMaterialBack.gameObject.SetActive(false);
        _chargeBack.gameObject.SetActive(!isActiveUse);
    }
}
