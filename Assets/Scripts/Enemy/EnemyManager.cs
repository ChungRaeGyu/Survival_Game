using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
   public class EnemyInfo{
    public Transform[] pos; // all enemy position list 
    public List<Enemy> emlist = new List<Enemy>();
    public Enemy em;
}
public class EnemyManager : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    
}
