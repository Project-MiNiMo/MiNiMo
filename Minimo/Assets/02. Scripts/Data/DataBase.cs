using UnityEngine;

public class DataBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        if (App.HasData(GetType()))
        {
            Destroy(gameObject);
        }
        else
        {
            App.RegisterData(this);
        }
    }
}