using System.Collections;

using UnityEngine;

public class PrologPanel : MonoBehaviour
{
    [SerializeField] private PrologBase[] _prologBases;
    
    private void Start()
    {
        StartCoroutine(ShowProlog());
    }

    private IEnumerator ShowProlog()
    {
        foreach (var prolog in _prologBases)
        {
            prolog.StartProlog();
            yield return new WaitUntil(() => prolog.IsEnd);
        }
        
        gameObject.SetActive(false);
    }
}
