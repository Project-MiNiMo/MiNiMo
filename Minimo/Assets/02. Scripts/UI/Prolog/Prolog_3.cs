using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Prolog_3 : PrologBase
{
    [SerializeField] private Image _image1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _image3;
    
    [SerializeField] private RectTransform _image4;
    [SerializeField] private RectTransform _image1Rect;
    [SerializeField] private RectTransform _image2Rect;
    [SerializeField] private RectTransform _image3Rect;
    [SerializeField] private RectTransform _image4Rect;
    [SerializeField] private RectTransform _image5Rect;
    
    protected override IEnumerator ShowProlog()
    {
        FadeIn(_image1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        FadeIn(_image2, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image3, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        ScaleIn(_image4, 0.5f);
        yield return new WaitForSeconds(0.5f);

        _image1Rect.DOAnchorPos(new Vector2(-1985, -431), 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(0.5f);
        _image2Rect.DOAnchorPos(new Vector2(-978, -306), 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(0.5f);
        _image5Rect.DOAnchorPos(new Vector2(-1303, -157), 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(0.5f);
        _image4Rect.DOAnchorPos(new Vector2(-1897, -38), 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(0.5f);
        _image3Rect.DOAnchorPos(new Vector2(-1585, -618), 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(1);
        
        EndProlog();
    }
}
