using UnityEngine;

public class DataBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        App.Instance.RegisterData(this);
    }
}