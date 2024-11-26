using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : UIBase
{
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _denyBtn;

    public override void Initialize() 
    {
        _confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        _denyBtn.onClick.AddListener(OnClickDenyBtn);
    }

    private void OnClickConfirmBtn()
    {
        ClosePanel();
    }

    private void OnClickDenyBtn()
    {
        ClosePanel();
    }

    public void ShowNewQuest()
    {
        OpenPanel();
    }
}
