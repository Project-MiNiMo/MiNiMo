using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSetting", menuName = "Scriptable Object/Default Setting Data")]
public class SettingDefault : ScriptableObject
{
    [SerializeField] string path;

    [SerializeField] SoundData sound;
    [SerializeField] GraphicsData graphics;
    [SerializeField] AlertData alert;

    public string Path => path;

    public SoundData Sound => sound;
    public GraphicsData Graphics => graphics;
    public AlertData Alert => alert;
}