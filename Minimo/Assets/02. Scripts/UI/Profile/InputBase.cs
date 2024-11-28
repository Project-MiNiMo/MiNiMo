using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputBase : MonoBehaviour
{
    [SerializeField] protected TMP_InputField _input;
    [SerializeField] private Button _editBtn;

    protected virtual void Start()
    {
        _editBtn.onClick.AddListener(OnClickEditBtn);
        _input.onEndEdit.AddListener(OnEndEdit);
        _input.DeactivateInputField();

        _input.interactable = false;
    }

    private void OnClickEditBtn()
    {
        _input.interactable = true;
        _input.Select();
    }

    protected virtual void OnEndEdit(string text)
    {
        _input.text = text;

        //App.Data.Player.SetPlayerNickName(_input.text, value =>
        //{
        //    _input.text = value;
        //    PhotonNetwork.LocalPlayer.NickName = value;
        //},
        //(error) =>
        //{
        //    _input.text = App.Data.Player.NickName;
        //    _input.transform.DOPunchPosition(new Vector3(15, 0, 0), 0.6f, 10, 0.5f);
        //});
        // ���� �ڵ�� �ͱͻ������� �г��� ������ �� ����� �ڵ��Դϴ�.. ���� ���� �� �ʿ��� �� ���� ���� �����ص׽��ϴ�.. -����..^^
    }
}
