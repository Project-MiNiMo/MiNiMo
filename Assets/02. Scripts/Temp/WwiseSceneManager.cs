using UnityEngine;
using UnityEngine.UI;

public class WwiseSceneManager : MonoBehaviour
{
    [SerializeField] private Button _testBtn;
    [SerializeField] private GameObject _playImg;
    [SerializeField] private GameObject _notPlayImg;
    [SerializeField] private Sprite[] _stateSprites;

    [Header("Hey Donghun Here!!")]
    [SerializeField] private string _soundCode;

    private void Start()
    {
        _testBtn.onClick.AddListener(OnClickTest);
    }

    private void OnClickTest()
    {
        _testBtn.interactable = false;
        _playImg.SetActive(true);
        _notPlayImg.SetActive(false);

        AkSoundEngine.PostEvent(_soundCode, gameObject, (uint)AkCallbackType.AK_EndOfEvent, PlaySoundCallback, null);
    }

    private void PlaySoundCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            _testBtn.interactable = true;
            _playImg.SetActive(false);
            _notPlayImg.SetActive(true);
        }
    }
}
