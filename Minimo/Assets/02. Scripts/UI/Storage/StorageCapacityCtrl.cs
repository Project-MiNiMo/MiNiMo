using System.Linq;

using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageCapacityCtrl : MonoBehaviour
{
    public int Capacity { get; private set; } = 0;
    
    [SerializeField] private TextMeshProUGUI _capacityTMP;
    [SerializeField] private Button _expandBtn;
    [SerializeField] private Transform _storageParent;

    private AccountInfoManager _accountInfo;
    private StorageExpandPanel _expandPanel;

    private void Start()
    {
        _accountInfo = App.GetManager<AccountInfoManager>();
            
        App.GetManager<UIManager>().GetPanel<StoragePanel>().StorageChanged.Subscribe(_ => SetCapacity()).AddTo(gameObject);
        _expandBtn.GetComponentInChildren<TextMeshProUGUI>().text = App.GetData<TitleData>().GetString("STR_STORAGE_UI_EXPAND");
        _expandBtn.onClick.AddListener(() => _expandPanel.OpenPanel());

        _expandPanel = App.GetManager<UIManager>().GetPanel<StorageExpandPanel>();

        SetCapacity();
    }
    
    private void SetCapacity()
    {
        var storageBtns = _storageParent.GetComponentsInChildren<StorageBtn>();
        Capacity = storageBtns.Sum(storageBtn => storageBtn.CanShow ? 1 : 0);

        _capacityTMP.text = $"{Capacity}/{100}"; //TODO : 최대 창고 용량
    }
    
    public void UpdateMaxCapacity()
    {
        _capacityTMP.text = $"{Capacity}/{100}"; //TODO : 최대 창고 용량
    }
}
