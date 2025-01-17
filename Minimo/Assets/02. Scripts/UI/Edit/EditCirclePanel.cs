using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class EditCirclePanel : UIBase
{
    [SerializeField] private RectTransform _rect;
    
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _cancelBtn;

    private EditManager _editManager;
    private Transform _target;

    public override void Initialize()
    {
        _editManager = App.GetManager<EditManager>();
        
        _editManager.IsEditing
            .Subscribe((isEditing) =>
            {
                gameObject.SetActive(isEditing);
            }).AddTo(gameObject);
        
        _editManager.CurrentCellPosition
            .Subscribe((position) =>
            {
                position.y += 0.5f;
                var screenPos = Camera.main.WorldToScreenPoint(position);
                _rect.position = screenPos;
            }).AddTo(gameObject);
        
        _confirmBtn.onClick.AddListener(() => _editManager.ConfirmEdit());
        _cancelBtn.onClick.AddListener(() => _editManager.CancelEdit());
    }
}
