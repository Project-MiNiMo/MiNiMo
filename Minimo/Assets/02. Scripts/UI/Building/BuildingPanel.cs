using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : UIBase
{
    [SerializeField] private Button _closeBtn;

    public override void Initialize()
    {
        _closeBtn.onClick.AddListener(ClosePanel);
    }
}
