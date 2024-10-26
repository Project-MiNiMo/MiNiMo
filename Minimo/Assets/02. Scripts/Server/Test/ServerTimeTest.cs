using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ServerTimeTest : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _timeText;

    TimeManager _timeManager;
    private void Start()
    {
        _timeManager = App.GetManager<TimeManager>();
    }
    
    private void Update()
    {
        if(_timeManager.IsProcessing == false)
            return;
        
        _timeText.text = _timeManager.Time.ToString(CultureInfo.CurrentCulture);
    }
    
}
