using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Data{
    //종류
    //차지할 공간
    public string 종류;
    public int size;
}
public class Inventory : MonoBehaviour
{
    Data Item = new Data() { size=1, 종류="회복약"};
    void Start()
    {
        //2.josn으로 변환
        string jsonData = JsonUtility.ToJson(Item);
        Item = JsonUtility.FromJson<Data>(jsonData);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
