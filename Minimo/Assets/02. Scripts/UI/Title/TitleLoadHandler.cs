using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleLoadHandler : MonoBehaviour
{
    [SerializeField] private Image _fillImg;
    [SerializeField] private TextMeshProUGUI _loadingText;

    private int _currentLoad;
    private int _maxLoad;
    
    private readonly string[] _loadingTexts = {"데이터 로딩 중입니다 ·..", "데이터 로딩 중입니다 .·.", "데이터 로딩 중입니다 ..·"};
    
    private void Start()
    {
        _fillImg.fillAmount = 0;
        gameObject.SetActive(false);
    }
    
    public void Setup(int maxLoad)
    {
        _maxLoad = maxLoad;
        _currentLoad = 0;
        
        _fillImg.fillAmount = 0;
        gameObject.SetActive(true);
        
        StartCoroutine(UpdateText());
    }
    
    public void UpdateLoad()
    {
        var progress = (float)++_currentLoad / _maxLoad;
        _fillImg.fillAmount = progress; 
    }
    
    public void FinishLoad()
    {
        StopAllCoroutines();
        
        _fillImg.fillAmount = 1;
        gameObject.SetActive(false);
    }

    private IEnumerator UpdateText()
    {
        var index = 0;

        while (true)
        {
            _loadingText.text = _loadingTexts[index];
            index = (index + 1) % _loadingTexts.Length;
            
            yield return new WaitForSeconds(0.3f);
        }
    }
}
