using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceAdvancedBack : MonoBehaviour
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private ProduceTaskBtn[] _taskBtns;
    [SerializeField] private ProduceOptionCtrl _optionCtrl;
    
    private void Start()
    {
        _taskBtns = GetComponentsInChildren<ProduceTaskBtn>(true);

        foreach (var taskBtn in _taskBtns)
        {
            taskBtn.Setup(this);
        }
    }
    
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive) 
        {
            InitTaskBtns();
        }
    }

    private void InitTaskBtns()
    {
        int i = 0;
        foreach (var task in App.GetManager<ProduceManager>().CurrentProduceObject.AllTasks)
        {
            _taskBtns[i].Initialize(task);
            i++;
        }
    }

    public void ShowOptionCtrl()
    {
        _optionCtrl.SetActive(true);
    }
}
