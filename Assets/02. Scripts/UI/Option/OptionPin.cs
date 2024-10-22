using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionPin : MonoBehaviour
{
    [SerializeField] private Button _pinBtn;
    [SerializeField] private TextMeshProUGUI _pinTMP;

    private string _userPin;

    private void Start()
    {
        _userPin = "1234567890";

        _pinTMP.text = App.GetData<TitleData>().GetFormatString("STR_OPTION_GAMESETTING_PIN", _userPin);
        _pinBtn.onClick.AddListener(OnClickPin);
    }

    private void OnEnable()
    {
        //TODO : Set User Pin;
    }

    private void OnClickPin()
    {
        GUIUtility.systemCopyBuffer = _userPin;
    }
}
