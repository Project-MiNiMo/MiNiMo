using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PrologPanel : MonoBehaviour
{
    [SerializeField] private PrologBase[] _prologBases;
    [SerializeField] private Image _blackblurImg;

    private bool _isKeyDownZ = false;
    
    private void Start()
    {
        StartCoroutine(ShowProlog());
    }

    private void Update()
    {
        if (_isKeyDownZ) return;
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isKeyDownZ = true;
            
            StopAllCoroutines();
            _blackblurImg.DOFade(1, 0.5f)
                .OnComplete(() => App.LoadScene(SceneName.Game));
        }
    }

    private IEnumerator ShowProlog()
    {
        _blackblurImg.DOFade(0, 1f);
        
        foreach (var prolog in _prologBases)
        {
            prolog.StartProlog();
            yield return new WaitUntil(() => prolog.IsEnd);
        }
        
        _blackblurImg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        App.LoadScene(SceneName.Game);
    }
}
