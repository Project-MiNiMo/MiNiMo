using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantOptionSlot : MonoBehaviour
{
    [Serializable]
    private struct PlantInfo
    {
        public GameObject _gameObject;
        public Image _image;
        public TextMeshProUGUI _text;
    }
    
    [SerializeField] private TextMeshProUGUI _resultNameTMP;
    [SerializeField] private PlantInfo _result;
    [SerializeField] private PlantInfo[] _materials;
    [SerializeField] private TextMeshProUGUI _timeTMP;
    [SerializeField] private TextMeshProUGUI _storageTMP;
    
    private TitleData _titleData;
    private AccountInfoManager _accountInfo;
    [SerializeField] private PlantHandler _plantHandler;

    private void Awake()
    {
        _titleData = App.GetData<TitleData>();
        _plantHandler = GetComponentInChildren<PlantHandler>(true);
        _accountInfo = App.GetManager<AccountInfoManager>();
    }

    public void SetOption(ProduceOption optionData)
    {
        _plantHandler.SetOption(optionData);

        SetResultInfo(optionData.Results[0]);
        SetMaterialInfo(optionData.Materials);
        
        _timeTMP.text = optionData.Time.ToString();
        _storageTMP.text = _accountInfo.GetItem(optionData.Results[0].Code).Count.ToString();
    }

    private void SetMaterialInfo(ProduceMaterial[] materials)
    {
        var i = 0;
        
        for (; i < materials.Length; i++) 
        {
            _materials[i]._gameObject.SetActive(true);
            _materials[i]._image.sprite = Resources.Load<Sprite>($"Item/{materials[i].Code}");
            _materials[i]._text.text = materials[i].Amount.ToString();
        }
        
        for (; i < _materials.Length; i++) 
        {
            _materials[i]._gameObject.SetActive(false);
        }
    }

    private void SetResultInfo(ProduceResult result)
    {
        if (!_titleData.Item.TryGetValue(result.Code, out var itemData))
        {
            Debug.LogError($"Cannot find item data with code : {result.Code}");
            return;
        }
        
        _resultNameTMP.text = _titleData.GetString(itemData.Name);
        _result._image.sprite = Resources.Load<Sprite>($"Item/{result.Code}");
        _result._text.text = $"X{result.Amount}";
    }
}

