using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class QuestBase : MonoBehaviour
{
    [SerializeField] private Button _clearBtn;
    [SerializeField] private Button _detailBtn;
    
    public abstract string ID { get; }
    protected abstract bool IsClear { get; }
    
    public virtual void StartQuest()
    {
        gameObject.SetActive(true);
        _clearBtn.onClick.AddListener(EndQuest);
        _clearBtn.gameObject.SetActive(false);
        _detailBtn.onClick.AddListener(ShowDetail);
        _detailBtn.gameObject.SetActive(true);
        
        StartCoroutine(WaitForClear());
    }
    
    private void EndQuest()
    {
        gameObject.SetActive(false);
        App.GetManager<QuestManager>().EndQuest(ID);
    }
    
    private IEnumerator WaitForClear()
    {
        yield return new WaitUntil(() => IsClear);
        
        _clearBtn.gameObject.SetActive(true);
        _detailBtn.gameObject.SetActive(false);
    }
    
    protected abstract void ShowDetail();
}
