using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        if (App.HasManager(GetType()))
        {
            Destroy(gameObject); 
        }
        else
        {
            App.RegisterManager(this);
        }
    }
}
