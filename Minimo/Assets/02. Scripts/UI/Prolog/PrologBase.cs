using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class PrologBase : MonoBehaviour
{
    public bool IsEnd { get; private set; }
    
    public virtual void StartProlog()
    {
        StartCoroutine(ShowProlog());
    }

    protected abstract IEnumerator ShowProlog();
    
    protected void FadeIn(Image image, float duration)
    {
        image.DOFade(1, duration).SetEase(Ease.Linear);
    }
    
    protected void FadeOut(Image image, float duration)
    {
        image.DOFade(0, duration).SetEase(Ease.Linear);
    }
    
    protected void ScaleIn(RectTransform image, float duration)
    {
        image.DOScale(Vector2.one, duration).SetEase(Ease.OutBack);
    }
    
    protected void ScaleOut(RectTransform image, float duration)
    {
        image.DOScale(Vector2.zero, duration).SetEase(Ease.Linear);
    }
    
    protected void ScaleInWithShake(RectTransform image, float duration)
    {
        image.DOScale(Vector2.one, duration).SetEase(Ease.OutBack);
        image.DOShakePosition(duration, 0.1f, 10, 90);
    }
    
    protected void ShakeCamera(float duration)
    {
        transform.parent.DOShakePosition(duration, 20, 10, 90);
    }
    
    protected void EndProlog()
    {
        IsEnd = true;
    }
}
