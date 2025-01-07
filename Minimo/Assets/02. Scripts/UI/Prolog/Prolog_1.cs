using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Prolog_1 : PrologBase
{
    [SerializeField] private Image _image1;
    [SerializeField] private RectTransform _image2;
    [SerializeField] private Image _image3;
    [SerializeField] private RectTransform _image4;
    [SerializeField] private GameObject _image5;
    [SerializeField] private Image _image6;
    [SerializeField] private RectTransform _image7;
    
    protected override IEnumerator ShowProlog()
    {
        yield return new WaitForSeconds(1);
        
        FadeIn(_image1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        ScaleInWithShake(_image2, 0.5f);
        yield return new WaitForSeconds(1.5f);
        
        FadeIn(_image3, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        ScaleInWithShake(_image4, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        _image5.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _image5.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        _image5.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        
        
        FadeIn(_image6, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        ScaleIn(_image7, 0.3f);
        yield return new WaitForSeconds(1.5f);
        
        EndProlog();
    }
}
