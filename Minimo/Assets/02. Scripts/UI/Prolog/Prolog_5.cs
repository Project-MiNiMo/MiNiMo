using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Prolog_5 : PrologBase
{
    [SerializeField] private Image _image1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _image3;
    [SerializeField] private Image _image4;
    [SerializeField] private Image _image5;
    [SerializeField] private RectTransform _image6;
    [SerializeField] private Image _image7;
    [SerializeField] private Image _image8;

    protected override IEnumerator ShowProlog()
    {
        FadeIn(_image1, 0.5f);
        ShakeCamera(0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image2, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        FadeIn(_image3, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image4, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        ScaleInWithShake(_image6, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image7, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image8, 0.5f);
        yield return new WaitForSeconds(3f);
        
        EndProlog();
    }
}
