using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        App.RegisterManager(this);
    }
}
