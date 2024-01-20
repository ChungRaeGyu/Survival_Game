using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{

    int num = 1;
    int speed = 0;
    public Text ModeText;
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            num++;
            switch(num%3){
                case 1:ModeText.text="Translate";break;
                case 2: ModeText.text ="AddForce"; break;
                case 0: ModeText.text = "Velocity"; break;
            }
        }
        if(num%3==1){
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0, 0, -speed * Time.deltaTime);
            }
        }
        if(num%3==2){
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigid.AddForce(-speed, 0, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rigid.AddForce(speed, 0, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                rigid.AddForce(0, 0, speed);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rigid.AddForce(0, 0, -speed);
            }
        }
        if(num%3==0){
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");
            // -1 ~ 1

            Vector3 velocity = new Vector3(inputX, 0, inputZ);
            velocity *= speed;
            rigid.velocity = velocity;
        }
    }
}
