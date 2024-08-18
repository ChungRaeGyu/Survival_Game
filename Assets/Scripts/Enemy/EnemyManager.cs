using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
   public class EnemyInfo{ //적 정보
    public Enemy em;
    public string name;
    public Transform e_pos;
}
public class ItemEvent{ // 아이템 정보 입력

}
 public class AreaInfo{ //구역 정보
    public int areaCode = 0; // 0 == default
    bool isRuin = false;
    public List<ItemEvent> i_event = new List<ItemEvent>();
    public List<EnemyInfo> e_info = new List<EnemyInfo>();
 }
public class EnemyManager : MonoBehaviour
{
    public List<AreaInfo> a_info = new List<AreaInfo>();
    public List<Enemy> e_list = new List<Enemy>();
    //public List<WorldTile> w_tile = new List<WorldTile>();
    private void Start() {
        // for (int i = 0; i < e_info.Count; i++) 초반 적 배열하면서 list에 저장해두기
        // {
            
        // }
    }
    void Update() {

    }
}
