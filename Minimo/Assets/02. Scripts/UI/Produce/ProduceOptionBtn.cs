using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceOptionBtn : MonoBehaviour
{
    [Serializable]
    private struct ProduceOptionInfo
    {
        public GameObject _gameObject;
        public Image _image;
        public TextMeshProUGUI _text;
    }
    
    [SerializeField] private ProduceOptionInfo _result;
    [SerializeField] private ProduceOptionInfo[] _materials;
    [SerializeField] private TextMeshProUGUI _timeTMP;
    
    private ProduceManager _produceManager;
    private int currentIndex;

    private void Start()
    {
        _produceManager = App.GetManager<ProduceManager>();
        
        GetComponent<Button>().onClick.AddListener(StartProduce);
    }

    public void Initialize(int index, ProduceOption optionData)
    {
        currentIndex = index;

        SetResultInfo(optionData.Results[0]);
        SetMaterialInfo(optionData.Materials);
        
        _timeTMP.text = optionData.Time.ToString();
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
        _result._image.sprite = Resources.Load<Sprite>($"Item/{result.Code}");
        _result._text.text = $"X{result.Amount}";
    }

    private void StartProduce()
    {
        _produceManager.StartProduce(currentIndex);
    }
}

