using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rigid;
    #region [Enemy Startus]
    public float speed = 3f;
    public int MaxHp = 10;
    public int CurHp = 0;
    public int SensorRadius = 5;
#endregion
    #region [Move]
    Transform target;
    bool targetFind = false;
    public float targetFolTime = 3f;
    public int DirType = 0; // type1= horizontal, type2= vertical
    private int CurDirType;
    private bool TurnDir = false;
    private bool Movechk = false;
    private float CurMoveTime = 0; // CurrentMoveCoolDown
    public int MoveTime = 2; // Moving Time
    public int MoveCD = 5; // MoveCoolDown
    Vector3 Pos = Vector3.forward; // Current diretion
#endregion
    void Start()
    {
        CurDirType = DirType;
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EnemyFind();
        if(targetFind){
            EnemyFollow();
        }
        else{
        if(Movechk){ // wanna move?
            IdleMove();
            }
        else{ // nope! dont want move
            WaitMove();
            }
        }
    }
    void EnemyLost()
    {
        
    }
    void EnemyFind()
    {
        Collider[] collider 
        = Physics.OverlapSphere(this.transform.position,SensorRadius,1 << 6);
        if(collider != null){
            target = collider[0].gameObject.transform;
            targetFind = true;
        }
            if(targetFind)
            {
                CurMoveTime = 0f;
            }
    }
    void EnemyFollow()
    {
        if(target)
        {
            CurMoveTime += targetFolTime * Time.deltaTime;
            if(CurMoveTime >= targetFolTime)
            {
                CurMoveTime = 0f;
                targetFind = !targetFind;
                //여기서 랜덤무빙 시작 줄(피곤해서 자러가야지)
            }
        }
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = lookRotation;
        rigid.velocity = speed * direction;
    }
    void WaitMove() // MoveCoolTime
    {
        if(CurMoveTime <= MoveTime)
        CurMoveTime += MoveTime * Time.deltaTime;
        else{
        Movechk = !Movechk;
        CurMoveTime = 0;
        }
    }
    void IdleMove()
    {
        switch (DirType)// Horizontal
        {
            case 0: // Change direction in horizontal
            CurMoveTime = 0;
            if(TurnDir){
            this.transform.localRotation = Quaternion.Euler(0, 0, 0); // Right
            TurnDir = !TurnDir;
            }
            else{
            this.transform.localRotation = Quaternion.Euler(0, 180, 0); // Left
            TurnDir = !TurnDir;
            }
            CurDirType = 2;
            break;
            case 1: // Change diretion in vertical
            CurMoveTime = 0;
            if(TurnDir){
            this.transform.localRotation = Quaternion.Euler(0, 90, 0); // Up
            TurnDir = !TurnDir;
            }
            else{
            this.transform.localRotation = Quaternion.Euler(0, -90, 0); // Down
            TurnDir = !TurnDir;
            }
            CurDirType = 2;
            break;
            case 2: // Cooldown after finish line
            if(MoveTime >= CurMoveTime){ // Moving
            rigid.velocity = speed * Pos;
            CurMoveTime += MoveTime * Time.deltaTime;
            }
            else{ // EndMove
            Movechk = !Movechk;
            CurMoveTime = DirType;
            }
            break;
            default: // Over Count Solution
            CurDirType = 2;
            break;
        }
    }
}
