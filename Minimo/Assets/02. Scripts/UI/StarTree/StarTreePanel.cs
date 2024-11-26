using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarTreePanel : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _boostBtn;
    [SerializeField] private Button _rewardBtn;
    [SerializeField] private Button _descriptionBtn;

    public override void Initialize()
    {
        _closeBtn.onClick.AddListener(ClosePanel);
        _boostBtn.onClick.AddListener(OnClickBoostBtn);
        _rewardBtn.onClick.AddListener(OnClickRewardBtn);
        _descriptionBtn.onClick.AddListener(OnClickDescriptionBtn);
    }

    private void OnClickBoostBtn()
    {

    }

    private void OnClickRewardBtn()
    {

    }

    private void OnClickDescriptionBtn()
    {

    }

    public override void OpenPanel()
    {
        base.OpenPanel();


    }
}
