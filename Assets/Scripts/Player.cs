using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    int speed = 0;
    Rigidbody rigid;
    Vector3 difference;
    Camera ViewCamera;
    string caseText;
    //Image[] Heart;

    [SerializeField] GameObject Inventory;
    public bool InventoryOpen=false;

    [SerializeField] GameObject OpenText;
    bool OpenItemBox=false;

    [SerializeField] GameObject ItemBoxGrid;

    bool OnPortal=false;

    void Start()
    {
        speed = 3;
        rigid = GetComponent<Rigidbody>();
        difference = Camera.main.transform.position - transform.position;
        ViewCamera = Camera.main;
        //Heart = GameObject.Find("Canvas").GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        move();
    }
    void Update()
    {
        CameraPostion();
        LookAt();
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOpenKey();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            InteractionMethod();
        }
    }

    private void InteractionMethod()
    {
        if(caseText == null){
            return;
        }
        switch(caseText){
            case "ItemBox" :
                if (OpenItemBox)
                {
                    if (!InventoryOpen)
                    {
                        InventoryOpenKey();
                    }
                    ItemBoxGrid.SetActive(OpenItemBox);
                    OpenItemBox = !OpenItemBox;
                    Debug.Log("열림");
                    break;
                }
                else
                {
                    ItemBoxGrid.SetActive(OpenItemBox);
                    OpenItemBox = !OpenItemBox;
                    Debug.Log("닫힘");
                    break;
                }
            case "Portal" :
                if (OnPortal)
                {
                    SceneManager.LoadScene("Lobby");
                    break;
                }
                break;
        }
    }

    private void InventoryOpenKey()
    {
        if (!InventoryOpen)
        {
            Inventory.SetActive(true);
        }
        else
        {
            Inventory.SetActive(false);
        }
        InventoryOpen = !InventoryOpen;
    }

    void move(){
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(inputX, 0, inputZ).normalized;
        velocity *= speed;
        rigid.velocity = velocity;
    }
    void CameraPostion(){
        Camera.main.transform.position = transform.position + difference;
    }
    void LookAt(){
        Ray ray = ViewCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up,Vector3.zero);
        float rayDistance;

        if(ground.Raycast(ray,out rayDistance)){
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin,point,Color.red);
            Vector3 Anti_Tilting_Position = new Vector3(point.x,transform.position.y,point.z);
            transform.LookAt(Anti_Tilting_Position);
        }
        
    }
    public void Damaged(){
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Interaction(other, true);
    }
    private void OnTriggerExit(Collider other) {
        Interaction(other, false);
        caseText = null;
    }

    private void Interaction(Collider other, bool check)
    {
        if (other.CompareTag("ItemBox"))
        {
            OpenText.GetComponent<Text>().text = "G 키를 눌러 상자를 열어보세요.";
            OpenText.SetActive(check);
            OpenItemBox=check;
            caseText = other.tag;
        }
        if(other.CompareTag("Portal")){
            OpenText.GetComponent<Text>().text = "G 키를 눌러 탈출합니다.";
            OpenText.SetActive(check);
            OnPortal=check;
            Debug.Log("탈출 :" + check);
            caseText = other.tag;
        }
    }
}
