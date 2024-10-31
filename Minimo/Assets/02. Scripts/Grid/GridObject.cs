using System.Collections;

using UnityEngine;

public class GridObject : MonoBehaviour
{
    private enum GridObjectType
    {
        Production,
        Decoration,
        AutoProduction
    }

    private enum GridObjectState
    {
        Idle,
        Construct,
        Produce,
        Complete,
    }

    [HideInInspector] public BoundsInt Area;
    public BoundsInt PreviousArea { get; private set; }
    public bool IsPlaced { get; private set; }
    public BuildingData Data { get; private set; }

    private bool _isFlipped = false;
    private SpriteRenderer _spriteRenderer;

    private GridObjectState _currState = GridObjectState.Idle;

    [SerializeField] private GameObject _produceUI;
    [SerializeField] private GameObject _completeUI;
    [SerializeField] private GameObject _gridObjectUI;

    private int _remainTime;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _gridObjectUI.SetActive(false);
        UpdateStateUI();
    }

    public void Initialize(BuildingData data, Sprite sprite)
    {
        Data = data;

        var size = new Vector3Int(data.SizeX, data.SizeY, 1);
        Area = new BoundsInt(Vector3Int.zero, size);

        _spriteRenderer.sprite = sprite;

        var yPosition = (float)((data.SizeX - 1) * 0.5);
        _spriteRenderer.transform.localPosition = new Vector3(0, yPosition, 0);
    }

    #region Edit Functions
    public void StartEdit()
    {
        SetTransparency(0.5f);
    }

    public void EndEdit()
    {
        SetTransparency(1f);
    }

    public void Place()
    {
        IsPlaced = true;
        PreviousArea = Area;

        EndEdit();
    }

    public void Rotate()
    {
        transform.Rotate(0, _isFlipped ? -180 : 180, 0);
        _isFlipped = !_isFlipped;
    }

    private void SetTransparency(float alpha)
    {
        Color color = _spriteRenderer.color;
        color.a = alpha;
        _spriteRenderer.color = color;
    }
    #endregion

    public void OnClick()
    {
        Debug.Log("OnClick");
        if (Data.Type == (int)GridObjectType.Production)
        {
            switch (_currState)
            {
                case GridObjectState.Idle:
                    App.GetManager<UIManager>().GetPanel<ProducePanel>().StartManageBuilding(this);
                    break;

                case GridObjectState.Construct:
                    break;

                case GridObjectState.Produce:
                    App.GetManager<UIManager>().GetPanel<ProducePanel>().StartManageBuildingOnProduce(this, _remainTime);
                    break;

                case GridObjectState.Complete:
                    _currState = GridObjectState.Idle;
                    UpdateStateUI();
                    break;
            }
        }
    }

    public void StartProduce(int duration)
    {
        _currState = GridObjectState.Produce;
        UpdateStateUI();

        StartCoroutine(Produce(duration));
    }

    private IEnumerator Produce(int duration)
    {
        _remainTime = duration;

        while (_remainTime > 0)
        {
            yield return new WaitForSeconds(1);

            _remainTime--;
        }

        _currState = GridObjectState.Complete;
        UpdateStateUI();
    }

    private void UpdateStateUI()
    {
        switch (_currState)
        {
            case GridObjectState.Produce:
                _produceUI.SetActive(true);
                _completeUI.SetActive(false);
                break;

            case GridObjectState.Complete:
                _produceUI.SetActive(false);
                _completeUI.SetActive(true);
                break;

            default:
                _produceUI.SetActive(false);
                _completeUI.SetActive(false);
                break;
        }
    }
}
