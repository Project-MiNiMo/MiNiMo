using UnityEngine;

public class DataBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        App.Instance.RegisterData(this);
    }
}

public class ManagerBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        App.Instance.RegisterManager(this);
    }
}
