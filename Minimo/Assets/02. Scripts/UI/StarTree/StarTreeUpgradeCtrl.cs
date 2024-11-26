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
    [SerializeField] private TextMeshProUGUI _upgradeTMP;

    [SerializeField] private StarTreeLevelInfoCtrl _levelInfoCtrl;

    private int _currentLevel = 1; //TODO : Connect with Server
    private int _currentBlueStarCount = 99; //TODO : Connect with Server

    private void Start()
    {
        _upgradeBtn.onClick.AddListener(OnClickUpgradeBtn);
        _confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        _denyBtn.onClick.AddListener(OnClickDenyBtn);

        _upgradeTMP.text = App.GetData<TitleData>().GetString("STR_STARTREE_UPGRADE_DESC");

        UpdateLevelInfo();
        UpdateBlueStarCount();
    }

    private void OnClickUpgradeBtn()
    {
        _upgradePopUpObj.SetActive(true);
    }

    private void OnClickConfirmBtn()
    {
        _upgradePopUpObj.SetActive(false);

        if (_currentBlueStarCount <= 0)
        {
            return;
        }

        _currentBlueStarCount--;

        UpdateLevelInfo();
        UpdateBlueStarCount();
    }

    private void OnClickDenyBtn()
    {
        _upgradePopUpObj.SetActive(false);
    }

    private void UpdateLevelInfo()
    {
        for (int i = 0; i < _levelTMPs.Length; i++)
        {
            _levelTMPs[i].text = (_currentLevel + i).ToString();
        }

        _levelInfoCtrl.Initialize(++_currentLevel);
    }

    private void UpdateBlueStarCount()
    {
        _blueStarCountTMP.text = _currentBlueStarCount.ToString();
    }
}
