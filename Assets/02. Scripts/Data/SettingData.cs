using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public bool BGM;
    public bool SFX;
}

[Serializable]
public struct GraphicsData
{
    public float Frame;
    public bool Graphics;
}

[Serializable]
public struct AlertData
{
    public bool Push;
}

public class SettingData : DataBase
{
    private struct GameData
    {
        public string UDID;
        public string Version;
        public SystemLanguage Language;

        public SoundData Sound;
        public GraphicsData Graphics;
        public AlertData Alert;
    }

    [SerializeField] private SettingDefault _default;

    public SoundData DefaultSound => _default.Sound;
    public GraphicsData DefaultGraphics => _default.Graphics;
    public AlertData DefaultAlert => _default.Alert;

    private GameData _data;

    private string _dataPath;
    private bool _isDataMessy;

    public bool IsDataLoaded { get; private set; }
    public bool IsDataSaved { get; private set; }

    #region Getter Setter
    public SystemLanguage Language
    {
        get => _data.Language;
        set
        {
            _data.Language = value;
            IsDataSaved = false;
            _isDataMessy = true;
            SaveToLocal();
        }
    }

    public SoundData Sound
    {
        get => _data.Sound;
        set
        {
            _data.Sound = value;
            IsDataSaved = false;
            _isDataMessy = true;
        }
    }

    public GraphicsData Graphics
    {
        get => _data.Graphics;
        set
        {
            _data.Graphics = value;
            IsDataSaved = false;
            _isDataMessy = true;
        }
    }

    public AlertData Alert
    {
        get => _data.Alert;
        set
        {
            _data.Alert = value;
            IsDataSaved = false;
            _isDataMessy = true;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        IsDataLoaded = false;
        IsDataSaved = false;
        _isDataMessy = true;
        _dataPath = Application.persistentDataPath + '/' + _default.Path;

        LoadFromLocal();
    }

    private void OnApplicationQuit()
    {
        SaveToLocal();
    }

    private void CreateDefault()
    {
        _data = new()
        {
            UDID = SystemInfo.deviceUniqueIdentifier,
            Version = Application.version,
            Language = SystemLanguage.Korean,

            Sound = DefaultSound,
            Graphics = DefaultGraphics,
            Alert = DefaultAlert,
        };

        _data.Language = Application.systemLanguage switch
        {
            SystemLanguage.Korean => SystemLanguage.Korean,
            SystemLanguage.Chinese => SystemLanguage.Chinese,
            SystemLanguage.Japanese => SystemLanguage.Japanese,
            _ => SystemLanguage.English
        };

        SaveToLocal();

        IsDataLoaded = true;
        _isDataMessy = false;
    }

    private void LoadFromLocal()
    {
        try
        {
            using var fstream = new FileStream(_dataPath, FileMode.Open);
            LoadDataInternal(fstream);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Failed to load save file from " + _dataPath +
                ". Default settings will be applied.\n" + exception);
        }

        if (IsDataLoaded)
        {
            Debug.Log("Data loaded successfully from " + _dataPath);
        }
        else
        {
            CreateDefault();
        }

        _isDataMessy = false;
    }

    private void LoadDataInternal(FileStream fstream) // loading save file should not be async function
    {
        using var streamReader = new StreamReader(fstream);
        string jsonStr = streamReader.ReadToEnd();

        _data = JsonUtility.FromJson<GameData>(jsonStr);

        if (!_data.UDID.Equals(SystemInfo.deviceUniqueIdentifier))
        {
            // TODO: just reset screen related data..?
            throw new InvalidDataException("New device detected");
        }

        if (!_data.Version.Equals(Application.version))
        {
            // TODO: notice user of version mismatch
            _data.Version = Application.version;
        }

        IsDataLoaded = true;
    }

    public async void SaveToLocal(bool force = false)
    {
        // if the data hasn't changed, we don't need to save it again
        if (!_isDataMessy && !force) return;

        using (var fstream = new FileStream(_dataPath, FileMode.Create))
        {
            do
            {
                _isDataMessy = false;

                try
                {
                    await SaveDataInternal(fstream);
                }
                catch (Exception exception)
                {
                    Debug.LogError("ERROR: Failed to save file.\n" + exception);
                    return;
                }
            }
            while (_isDataMessy);
        }

        IsDataSaved = true;
        Debug.Log("Data saved successfully to " + _dataPath);
    }

    public async Task SaveDataInternal(FileStream fstream)
    {
        string jsonStr = JsonUtility.ToJson(_data);

        using var streamWriter = new StreamWriter(fstream);
        await streamWriter.WriteAsync(jsonStr);
    }
}
