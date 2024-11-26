using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarTreeUpgradeCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _levelTMPs;
    [SerializeField] private TextMeshProUGUI _blueStarCountTMP;

    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private GameObject _upgradePopUpObj;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _denyBtn;

    private int _currentLevel = 1; //TODO : Connect with Server

    public void Initialize()
    {
        _upgradeBtn.onClick.AddListener(OnClickUpgradeBtn);
        _confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        _denyBtn.onClick.AddListener(OnClickDenyBtn);
    }

    private void OnClickUpgradeBtn()
    {
        _upgradePopUpObj.SetActive(true);
    }

    private void OnClickConfirmBtn()
    {
        UpgradeTree();
        _upgradePopUpObj.SetActive(false);
    }

    private void OnClickDenyBtn()
    {
        _upgradePopUpObj.SetActive(false);
    }

    private void UpgradeTree()
    {
        for (int i = 1; i < _levelTMPs.Length; i++)
        {
            _levelTMPs[i].text = (_currentLevel + i).ToString();
        }

        _currentLevel++;
    }
}
