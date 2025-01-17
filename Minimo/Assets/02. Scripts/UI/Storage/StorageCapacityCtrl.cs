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

    private PlayerData _playerData;
    private StorageExpandPanel _expandPanel;

    private void Start()
    {
        _playerData = App.GetData<PlayerData>();
            
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

        _capacityTMP.text = $"{Capacity}/{_playerData.MaxStorageCapacity}";
    }
    
    public void UpdateMaxCapacity()
    {
        _capacityTMP.text = $"{Capacity}/{_playerData.MaxStorageCapacity}";
    }
}
