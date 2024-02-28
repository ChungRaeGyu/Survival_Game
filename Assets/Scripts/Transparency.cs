using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    Material sr;
    float r,g,b,a;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<MeshRenderer>().material;
        r = sr.color.r;
        g = sr.color.g;
        b = sr.color.b;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            sr.color=new Color(r,g,b,0.5f);
            print("들어옴");
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            sr.color = new Color(r,g,b,1);
            print("나옴");
        }
    }
}
