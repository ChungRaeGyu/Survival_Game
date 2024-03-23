using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testscript : MonoBehaviour
{
    public BoxCollider boxcol;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!boxcol.enabled)
            boxcol.enabled = true;
            else{
                boxcol.enabled = false;
            }
        }
    }

}
