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

    [SerializeField] GameObject OpenText;
    bool OpenItemBox=false;

    GameObject ItemBox;

    bool OnPortal=false;

    InventoryController inventoryController;
    
    Animator anim;
    [SerializeField] GameObject AttackRange;

    DoorOpen Door;
    void Start()
    {
        anim = GetComponent<Animator>();
        speed = 3;
        rigid = GetComponent<Rigidbody>();
        difference = Camera.main.transform.position - transform.position;
        ViewCamera = Camera.main;
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        //Heart = GameObject.Find("Canvas").GetComponentsInChildren<Image>();
        ItemBox = inventoryController.ItemBox;
        AttackRange.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate() {
        move();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            InteractionMethod();
        }
        CameraPostion();
        LookAt();
        

        if(Input.GetMouseButtonDown(0)){
            anim.SetTrigger("Attack");
            StartCoroutine(RangeSet());
        }
    }
    IEnumerator RangeSet(){
        yield return new WaitForSecondsRealtime(0.5f);
        AttackRange.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        AttackRange.SetActive(false);
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
                    if (!inventoryController.InventoryOpen)
                    {
                        inventoryController.InventoryOpenKey();
                    }
                    ItemBox.SetActive(OpenItemBox);
                    OpenItemBox = !OpenItemBox;
                    Debug.Log("열림");
                    break;
                }
                else
                {
                    ItemBox.SetActive(OpenItemBox);
                    OpenItemBox = !OpenItemBox;
                    inventoryController.SelectedItemGrid=null;
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
            case "Door": Door.OpenDoor();break;
        }
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
        switch(other.tag){
            case "Enemy" : AttackEnemy(other); break;
            case "ItemBox":
                SetItemBox(other,true);
                break;
            case "Portal":
                SetPortal(other,true);
                break;
            case "Door":
                SetDoor(other,true);
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "ItemBox":
                SetItemBox(other, false);
                break;
            case "Portal":
                SetPortal(other, false);
                break;
            case "Door":
                SetDoor(other, false);
                break;
        }
        ItemBox.SetActive(OpenItemBox);
        caseText = null;
        inventoryController.SelectedItemGrid = null;
        Door = null;
    }

    private void SetPortal(Collider other,bool check)
    {
        OpenText.GetComponent<Text>().text = "G 키를 눌러 탈출합니다.";
        OpenText.SetActive(check);
        OnPortal = check;
        caseText = other.tag;
    }

    private void SetItemBox(Collider other,bool check)
    {
        OpenText.GetComponent<Text>().text = "G 키를 눌러 상자를 열어보세요.";
        OpenText.SetActive(check);
        OpenItemBox = check;
        caseText = other.tag;
    }
    private void SetDoor(Collider other, bool check)
    {
        OpenText.GetComponent<Text>().text = "G 키를 눌러 문을 엽니다.";
        OpenText.SetActive(check);
        caseText = other.tag;
        Door = other.gameObject.GetComponent<DoorOpen>();
    }
    private void AttackEnemy(Collider other)
    {
        if (AttackRange.activeSelf)
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
        }
    }


}
