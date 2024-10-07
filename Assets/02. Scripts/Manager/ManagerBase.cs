using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        App.Instance.RegisterManager(this);
    }
}
