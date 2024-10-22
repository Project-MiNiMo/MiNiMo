using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    private float _spawnCooltime;
    private ShootingStarType _currentType;

    private void Start()
    {
        
    }

    public void Initialized(ShootingStarType _type)
    {
        _currentType = _type;


    }
}
