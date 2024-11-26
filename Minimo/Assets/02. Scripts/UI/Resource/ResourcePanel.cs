using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : UIBase
{
    [SerializeField] private Button _closeBtn;

    public override void Initialize() 
    {
        _closeBtn.onClick.AddListener(ClosePanel);
    }

    public void ShowGetResource()
    {
        OpenPanel();
    }
}
