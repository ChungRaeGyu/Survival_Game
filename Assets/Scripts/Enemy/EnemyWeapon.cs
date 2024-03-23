using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    BoxCollider boxcol;
    private void Start() {
        boxcol = GetComponent<BoxCollider>();
        boxcol.enabled = false;
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
