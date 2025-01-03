using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceTaskBtn : MonoBehaviour
{
    [SerializeField] private Button _taskBtn;
    [SerializeField] private TextMeshProUGUI _taskText;
    
    private ProduceTask _produceTask;
    [SerializeField] private RemainTimeUpdater _remainTimeUpdater;

    private int _currentRemainTime;
    private PlantPanel _plantPanel;

    private ProduceManager _produceManager;
    private int _taskIndex;
    private ProduceObject _produceObject;

    private void Start()
    {
        _taskIndex = transform.GetSiblingIndex();
        _plantPanel = App.GetManager<UIManager>().GetPanel<PlantPanel>();
        
        _taskBtn.onClick.AddListener(() =>
        {
            if (_produceTask == null) 
            {
                _plantPanel.OpenPanel();
                return;
            }
            
            if (_produceTask?.CurrentState is PendingState)
            {
                //취소?
            }
            else if (_produceTask?.CurrentState is ActiveState)
            {
                //중간 수확 기능 구현
            }
            else if (_produceTask?.CurrentState is CompletedState)
            {
                //수확 기능 구현
            }
        });
        
        _produceManager = App.GetManager<ProduceManager>();
  
        _produceManager.CurrentRemainTime
            .Subscribe(SetRemainTime)
            .AddTo(gameObject);
    }

    private void Update()
    {
        if (gameObject.activeSelf == false) 
        {
            return;
        }
        
        if (_produceObject == null)
        {
            return;
        }

        if (_produceObject.AllTasks.Count <= _taskIndex) 
        {
            return;
        }
        
        var tempTask = _produceObject.AllTasks[_taskIndex];
        
        if (!Equals(_produceTask, tempTask))
        {
            _produceTask = tempTask;
            SetRemainTime(_produceTask.RemainTime);
        }
    }
    
    public void Initialize(ProduceObject produceObject)
    {
        _produceObject = produceObject;
    }
    
    private void SetRemainTime(int remainTime)
    {
        if (_produceObject == null)
        {
            return;
        }
 
        if (_produceTask?.CurrentState is PendingState)
        {
            _remainTimeUpdater.SetPending();
        }
        else if (_produceTask?.CurrentState is ActiveState)
        {
            _remainTimeUpdater.SetRemainTime(remainTime, _produceTask.Data.Time);
        }
        else if (_produceTask?.CurrentState is CompletedState)
        {
            _remainTimeUpdater.SetComplete();
        }
        else
        {
            _remainTimeUpdater.SetEmpty();
        }
    }
}
