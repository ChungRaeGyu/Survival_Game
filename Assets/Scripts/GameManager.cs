using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Active_All();
    }

    private static void Active_All()
    {
        GameObject[] inactiveObjects = GameObject.Find("Inventory_Canvas").GetComponentsInChildren<GameObject>();

        foreach (GameObject obj in inactiveObjects)
        {
            obj.SetActive(true);
        }
    }
}
