using UnityEngine;
using UnityEngine.UI;

public class HpiPanel : UIBase
{
    [SerializeField] private Image _fillImg;

    public override void Initialize()
    {
        _fillImg.fillAmount = 0.3f;
    }
}
