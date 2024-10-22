using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSetting", menuName = "Scriptable Object/Default Setting Data")]
public class SettingDefault : ScriptableObject
{
    [SerializeField] private string _path;

    [SerializeField] private SoundData _sound;
    [SerializeField] private ScreenData _screen;
    [SerializeField] private AlertData _alert;

    public string Path => _path;

    public SoundData Sound => _sound;
    public ScreenData Screen => _screen;
    public AlertData Alert => _alert;
}