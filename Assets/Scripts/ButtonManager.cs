using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] InventoryController inventoryController;
    public GameObject StorePanel;
    public GameObject StorageGrid;

    //Don'tDestroyOnLoad
    private static ButtonManager instance = null;
    // Start is called before the first frame update
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
        //InitSetting();
        Debug.Log("실행 버튼 매니저");
    }

    private void InitSetting()
    {
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        StorePanel = inventoryController.StorePanel;
        StorageGrid = inventoryController.Storage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStartBtn()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void GameExitBtn()
    {
        Application.Quit();
    }
    public void EnterRoomBtn()
    {
        if (StorePanel.activeSelf)
        {
            print("상점을 닫아주세요");
            return;
        }
        SceneManager.LoadScene("PlayerTest");
    }
    public void OpenStorageBtn(){
        StorageGrid.SetActive(true);
    }
    public void OpenShopBtn(){
        StorePanel.SetActive(true);
        StorageGrid.SetActive(true);
    }
    public void CloseShopBtn(){
        StorePanel.SetActive(false);
        StorageGrid.SetActive(false);
    }
}
