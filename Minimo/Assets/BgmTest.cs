using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmTest : MonoBehaviour
{
    public AK.Wwise.Bank soundBank;
    void Start()
    {
        //Load Bank
        soundBank.Load();

        AkSoundEngine.PostEvent("BGM_Title", gameObject);
    }
    private void OnDestroy()
    {
        //BGM Stop
        AkSoundEngine.StopAll();
        soundBank.Unload();
    }
}
