using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Prolog_4 : PrologBase
{
    [SerializeField] private Image _image1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _image3;
    [SerializeField] private Image _image4;
    [SerializeField] private RectTransform _image5;
    [SerializeField] private RectTransform _image6;
    [SerializeField] private Image _image7;
    [SerializeField] private Image _blackBlur;
    [SerializeField] private GameObject _background;
    
    protected override IEnumerator ShowProlog()
    {
        FadeIn(_image1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        FadeIn(_image2, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        _background.SetActive(true);
        
        FadeIn(_image3, 0.5f);
        _blackBlur.DOFade(0.5f, 0.25f).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(1);
        
        FadeIn(_image4, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        ScaleInWithShake(_image5, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        _image6.gameObject.SetActive(true);
        ScaleOut(_image6, 1f);
        FadeIn(_image7, 0.5f);
        yield return new WaitForSeconds(1f);
        
        EndProlog();
    }
}