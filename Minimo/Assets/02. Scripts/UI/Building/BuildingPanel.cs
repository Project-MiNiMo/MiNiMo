using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : UIBase
{
    [SerializeField] private Transform _buildingGroup;
    [SerializeField] private GameObject _buildingBtnPrefab;

    [Header("Buttons")]
    [SerializeField] private Button _closeBtn;

    private Dictionary<string, BuildingBtn> _btnDictionary = new();
    private Transform _buildingObjectParent;

    public override void Initialize()
    {
        _closeBtn.onClick.AddListener(ClosePanel);

        _buildingObjectParent = GameObject.FindWithTag("BuildingObjectParent").transform;

        InitStorageButtons();
    }

    private void InitStorageButtons()
    {
        var existingButtons = _buildingGroup.GetComponentsInChildren<BuildingBtn>(true);

        int index = 0;

        foreach (var data in App.GetData<TitleData>().Building.Values)
        {
            BuildingBtn storageBtn;

            if (index < existingButtons.Length)
            {
                storageBtn = existingButtons[index];
            }
            else
            {
                storageBtn = Instantiate(_buildingBtnPrefab, _buildingGroup).GetComponent<BuildingBtn>();
            }

            storageBtn.Initialize(data, _buildingObjectParent);
            _btnDictionary.Add(data.ID, storageBtn);

            index++;
        }
    }
}
