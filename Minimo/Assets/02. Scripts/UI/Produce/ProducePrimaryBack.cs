using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProducePrimaryBack : MonoBehaviour
{
    [SerializeField] private ProduceOptionCtrl _optionCtrl;
    [SerializeField] private ProduceHarvestCtrl _harvestCtrl;
    [SerializeField] private ProduceInfoCtrl _infoCtrl;
    private ProduceManager _produceManager;

    private void Start()
    {
        _produceManager = App.GetManager<ProduceManager>();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
        {
            var currentObject = _produceManager.CurrentProduceObject;

            if (currentObject.CompleteTasks.Count > 0)
            {
                ShowUI(_harvestCtrl);
            }
            else if (currentObject.ActiveTask != null)
            {
                ShowUI(_infoCtrl);
            }
            else
            {
                ShowUI(_optionCtrl);
            }
        }
    }

    private void ShowUI(MonoBehaviour targetUI)
    {
        _harvestCtrl.SetActive(targetUI == _harvestCtrl);
        _optionCtrl.SetActive(targetUI == _optionCtrl);
        _infoCtrl.SetActive(targetUI == _infoCtrl);
    }
}
