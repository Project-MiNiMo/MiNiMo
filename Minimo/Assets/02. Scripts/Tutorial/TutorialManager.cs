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
}
