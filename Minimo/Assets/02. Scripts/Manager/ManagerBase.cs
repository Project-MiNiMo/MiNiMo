using UnityEngine;

[DefaultExecutionOrder(-30)]
public class ManagerBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        App.RegisterManager(this);
    }
}
