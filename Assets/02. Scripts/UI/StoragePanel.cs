using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoragePanel : UIBase
{
    [SerializeField] private Transform storageGroup;
    [SerializeField] private GameObject storageBtnPrefab;

    [SerializeField] private Button storageOpenBtn;
    [SerializeField] private Button storageCloseBtn;

    [SerializeField] private Transform gridObjectGroup;
    private RectTransform rectTransform;

    private Dictionary<string, StorageBtn> storageButtons = new();

    private bool isOpen = true;

    public override void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();

        storageOpenBtn.onClick.AddListener(Open);
        storageCloseBtn.onClick.AddListener(Close);

        InitStorageButtons();
        UpdateOpenCloseButtons();
    }

    private void InitStorageButtons()
    {
        var existingButtons = storageGroup.GetComponentsInChildren<StorageBtn>(true);
        int index = 0;

        foreach (var data in App.Instance.GetData<TitleData>().GridObject.Values)
        {
            StorageBtn storageBtn;

            if (index < existingButtons.Length)
            {
                storageBtn = existingButtons[index];
            }
            else
            {
                storageBtn = Instantiate(storageBtnPrefab, storageGroup).GetComponent<StorageBtn>();
            }

            storageBtn.Initialize(data, gridObjectGroup);
            storageButtons.Add(data.Code, storageBtn);
            index++;
        }
    }

    private void UpdateOpenCloseButtons()
    {
        storageOpenBtn.gameObject.SetActive(!isOpen);
        storageCloseBtn.gameObject.SetActive(isOpen);
    }

    public void AddObjectCount(string objectName, int count)
    {
        string cleanName = objectName.Replace("(Clone)", "");
        if (storageButtons.TryGetValue(cleanName, out var storageBtn))
        {
            storageBtn.AddObjectCount(count);
        }
        else
        {
            Debug.LogWarning($"Storage button for '{cleanName}' not found.");
        }
    }

    public void Open()
    {
        if (isOpen) return;

        rectTransform.DOAnchorPosY(0f, 0.5f).OnComplete(() =>
        {
            isOpen = true;
            UpdateOpenCloseButtons();
        });
    }

    public void Close()
    {
        if (!isOpen) return;

        rectTransform.DOAnchorPosY(-rectTransform.sizeDelta.y, 0.2f).OnComplete(() =>
        {
            isOpen = false;
            UpdateOpenCloseButtons();
        });
    }

    private void OnDestroy()
    {
        storageOpenBtn.onClick.RemoveAllListeners();
        storageCloseBtn.onClick.RemoveAllListeners();
    }
}
