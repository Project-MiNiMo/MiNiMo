using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Prolog_2 : PrologBase
{
    [SerializeField] private Image _image1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _image3;
    
    protected override IEnumerator ShowProlog()
    {
        FadeIn(_image1, 0.5f);
        FadeIn(_image2, 1);
        yield return new WaitForSeconds(2);
        
        FadeIn(_image3, 0.5f);
        yield return new WaitForSeconds(3);
       
        EndProlog();
    }
}
