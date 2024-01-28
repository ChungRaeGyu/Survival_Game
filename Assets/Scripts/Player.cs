using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    int speed = 0;
    public Text ModeText;
    Rigidbody rigid;
    Vector3 difference;
    Camera ViewCamera;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        rigid = GetComponent<Rigidbody>();
        difference = Camera.main.transform.position - transform.position;
        ViewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        //CameraPostion();
        LookAt();
        
    }

    void move(){
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(inputX, 0, inputZ);
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
}
