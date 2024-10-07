using UnityEngine;

public class GridObject : MonoBehaviour
{
    public BoundsInt Area;
    public BoundsInt PreviousArea { get; private set; }
    public bool IsPlaced { get; private set; }

    private bool _isFlipped = false;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

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
}
