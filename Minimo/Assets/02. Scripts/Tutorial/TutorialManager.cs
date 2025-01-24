using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : ManagerBase
{
    [SerializeField] private List<TutorialBase> _tutorials;

    public string Wish;

    private void Start()
    {
        _tutorials[0].StartTutorial();
    }
    
    public void NextTutorial()
    {
        _tutorials[0].EndTutorial();
        _tutorials.RemoveAt(0);

        if (_tutorials.Count > 0)
        {
            _tutorials[0].StartTutorial();
        }
    }
}
