using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int Money;

    int sum = 0;
    public Text StorageMoney;
    public Text InventoryMoney;
    private static PlayerData instance = null;

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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sum != Money)
        {
            sum = Money;
            StorageMoney.text = "소지금 : " + sum;
        }

    }
}
