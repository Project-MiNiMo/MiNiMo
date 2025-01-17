using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Nickname : InputBase
{
    private const string PLAYER_NAME = "¥Ÿ¿∫";

    protected override void Start()
    {
        base.Start();

        _input.text = PLAYER_NAME;
        _input.characterLimit = 6;
    }
}
