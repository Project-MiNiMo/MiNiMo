using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener
{
    void OnEvent(EventCode code, Component sender, object param = null);
}
