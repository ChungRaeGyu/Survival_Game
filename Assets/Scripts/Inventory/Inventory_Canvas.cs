using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Canvas : MonoBehaviour
{
    private static Inventory_Canvas instance=null;
    private void Awake()
    {
        DontDestroyOnLoad_Singleton();
    }

    private void DontDestroyOnLoad_Singleton()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
