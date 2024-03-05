using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    // Start is called before the first frame update
    bool isOpen = false;
    bool Optimization=true;
    float smoot = 2f;
    float doorOpenAngle = 90f;
    float doorOpenAngle2 = 0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Optimization)return;
        if (isOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle2, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoot * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoot * Time.deltaTime);
        }
    }
    IEnumerator time(){
        Optimization = false;
        yield return new WaitForSecondsRealtime(2f);
        Optimization = true;
        Debug.Log("멈춤");
    }
    public void OpenDoor(){
        StartCoroutine(time());
        isOpen = !isOpen;
    }
}
