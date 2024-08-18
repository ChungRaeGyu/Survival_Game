using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileManager : MonoBehaviour
{
    public GameObject gb; // 타일들의 아버지
    public List<WorldTile> w_tile = new List<WorldTile>(); // 모든 타일(프리팹)
    public List<WorldTile> w_SpawnTile = new List<WorldTile>(); // 새로 생성된 타일
    public int tileScale = 4; // 수직, 수평
    private int tileAllCount = 0;
    private void Awake() {
        tileAllCount *= tileScale;
        SpawnTiles();
    }

    private void SpawnTiles(){
        Vector3 pos;
        pos = Vector3.zero;
        int x = 0;
        int y = 0;
        int ran = 0;
        for (int i = 0; i < tileScale; i++)
        {
            for (int j = 0; j < tileScale; j++)
            {
                ran = Random.Range(0, w_tile.Count);
                GameObject.Instantiate(w_tile[ran], pos, Quaternion.identity).transform.parent = gb.transform;
                pos.x += 50;

                Debug.Log("log1");
            }
            pos.z += 50;
            pos.x = 0;
        }
    }
}
