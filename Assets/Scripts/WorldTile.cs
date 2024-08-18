using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public string name;
    public int code;
    public List<Transform> trans = new List<Transform>();
    private void Start() {
        if(trans.Count == 0){
            int a = this.transform.Find("SpawnPoint").childCount;
            for (int i = 0; i < a; i++)
            {
                trans.Add(this.transform.Find("SpawnPoint").GetChild(i));
            }
        }
    }
}
