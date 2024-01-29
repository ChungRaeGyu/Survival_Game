using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    BoxCollider boxcol;
    Coroutine attackCoru;
    private void Start() {
        boxcol = GetComponent<BoxCollider>();
    }
    /*
    IEnumerator IEattack(){
        yield return new WaitForSeconds(5.0f);
        boxcol.enabled = false;
    }
    */
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("PlayerAttack!");
            boxcol.enabled = false;
            //attackCoru = StartCoroutine(IEattack());
        }
    }

}