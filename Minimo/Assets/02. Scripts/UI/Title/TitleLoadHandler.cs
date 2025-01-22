using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLoadHandler : MonoBehaviour
{
    [SerializeField] private Image _fillImg;

    private int _currentLoad;
    private int _maxLoad;
    
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
    }
    
    public void UpdateLoad()
    {
        var progress = (float)++_currentLoad / _maxLoad;
        _fillImg.fillAmount = progress; 
    }
    
    public void FinishLoad()
    {
        _fillImg.fillAmount = 1;
        gameObject.SetActive(false);
    }
}
