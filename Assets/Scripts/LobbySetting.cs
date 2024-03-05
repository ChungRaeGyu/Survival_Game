using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySetting : MonoBehaviour
{
    GameObject LobbyCanvas;
    Button[] Canvasobj;
    ButtonManager buttonManager;
    // Start is called before the first frame update
    void Start()
    {
        buttonManager = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();
        LobbyCanvas = GameObject.Find("LobbyCanvas");
        Canvasobj = GetComponentsInChildren<Button>();
        Canvasobj[0].onClick.AddListener(buttonManager.OpenShopBtn);
        Canvasobj[1].onClick.AddListener(buttonManager.OpenStorageBtn);
        Canvasobj[2].onClick.AddListener(buttonManager.EnterRoomBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
