using UnityEngine;

public class GridObject : MonoBehaviour
{
    [HideInInspector] public BoundsInt Area;
    public BoundsInt PreviousArea { get; private set; }
    public bool IsPlaced { get; private set; }

    private bool _isFlipped = false;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialize(BuildingData data, Sprite sprite)
    {
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

    }
}
