    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] InventoryController inventoryController;
    // Start is called before the first frame update
    void Start()
    {
        
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
        SceneManager.LoadScene("PlayerTest");
    }
    public void OpenStorageBtn(){

    }
    public void OpenShopBtn(){
        
    }
}
