using UnityEngine;
using TMPro;

public class Input_SelfIntroduction : InputBase
{
    [SerializeField] private TextMeshProUGUI _stringCountTMP;

    private const int STRING_LIMIT = 99;
    private const string STRING_COUNT = "{0} / {1}";

    protected override void Start()
    {
        base.Start();

        var placeHolder = _input.placeholder as TextMeshProUGUI;
        placeHolder.text = App.GetData<TitleData>().GetString("STR_PROFILE_SELFINTRODUCTION_DEFAULT");
        _input.characterLimit = 99;

        _input.onValueChanged.AddListener(UpdateStringCount);

        UpdateStringCount();
    }

    protected override void OnEndEdit(string text)
    {
        base.OnEndEdit(text);

        UpdateStringCount();
    }

    private void UpdateStringCount()
    {
        _stringCountTMP.text = string.Format(STRING_COUNT, _input.text.Length, STRING_LIMIT);
    }

    private void UpdateStringCount(string text)
    {
        _stringCountTMP.text = string.Format(STRING_COUNT, text.Length, STRING_LIMIT);
    }
}
