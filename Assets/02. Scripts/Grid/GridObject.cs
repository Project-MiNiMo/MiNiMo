using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public BoundsInt area;
    public BoundsInt PreviousArea { get; private set; }

    public bool IsPlaced { get; private set; }
    private bool isFlipped = false;

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
        PreviousArea = area;
        EndEdit();
    }

    public void Rotate()
    {
        transform.Rotate(0, isFlipped ? -180 : 180, 0);
        isFlipped = !isFlipped;
    }

    private void SetTransparency(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
