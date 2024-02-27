using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    public enum MoveDirectionType {horizontal, vertical};
    public MoveDirectionType type;
    Rigidbody rigid;
    public BoxCollider bcol;

#region [Condition]
    bool faint = false; //      move,attack,talent (x)
    bool canAttack = false; //  move, attack (o) / talent (x)
#endregion
#region [Enemy Status]
    public float speed = 3f;
    public int MaxHp = 10;
    public int CurHp = 0;
    public int sensorRadius = 10;
#endregion
#region [Attack]
    bool isAttack = false;
    public int atksenseDis = 1;
    Coroutine attackCoru;
#endregion
#region [Move]
    Transform target;
    bool targetFind = false;
    public float targetFolTime = 3f;
    private int CurDirType;
    private bool TurnDir = false;
    private bool Movechk = false;
    public float CurMoveTime = 0; // CurrentMoveCoolDown
    public int MoveTime = 5; // Moving Time
    public int MoveCD = 5; // MoveCoolDown
    Vector3 Pos = Vector3.forward; // Current diretion
#endregion
    void Start()
    {
        CurDirType = (int)type;
        bcol = GetComponentInChildren<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
        CurHp = MaxHp;
    }

    void Update()
    {
        if(!isAttack){
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
    }
    IEnumerator IEattack()
    {
        isAttack = true;
        yield return new WaitForSeconds(3.0f);
        IsAttack();
        yield return new WaitForSeconds(1.0f);
        isAttack = false;
    }
    void IsAttack()
    {
        Debug.Log("Attack!!");
        bcol.enabled = true;
    }
    void EnemyFind() // 범위 감지-> 범위 안에있다면 공격코루틴시작 -> 찾았지만 범위 밖에 있다면 특정시간 추적-> 원상태
    {
        Collider[] collider 
        = Physics.OverlapSphere(this.transform.position,sensorRadius,1 << 6);
        if(collider.Length > 0){
            target = collider[0].gameObject.transform;
            CurMoveTime = 0f;
            targetFind = true;

            Vector3 offset = target.position - transform.position;
            float sqrLen = offset.sqrMagnitude;
            
            if(sqrLen < atksenseDis*atksenseDis){ 
                attackCoru = StartCoroutine(IEattack());
            }
        }
        else{ // this play line when isnt find to player
            if(targetFind){
                CurMoveTime += Time.deltaTime;
                if(CurMoveTime >= targetFolTime)
                {
                    CurMoveTime = 0f;
                    targetFind = false;
                    target = null;
                }
            }
        }
    }
    void AttacMode()
    {
        
    }
    void EnemyFollow()
    {
        Debug.Log("EnemyFollow");
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = lookRotation;
        rigid.velocity = speed * direction;
    }
    void WaitMove() // MoveCoolTime
    {
        if(CurMoveTime < MoveCD){
        CurMoveTime += Time.deltaTime;
        Debug.Log("stop move");
        }
        else{
        Movechk = !Movechk;
        CurMoveTime = 0;
        }
    }
    void IdleMove()
    {
        switch (CurDirType)
        {
            case 0: // Change direction in horizontal
            CurMoveTime = 0;
            if(TurnDir){
            this.transform.rotation = Quaternion.Euler(0, -90, 0); // forward
            Pos = Vector3.left;
            TurnDir = !TurnDir;
            }
            else{
            this.transform.rotation = Quaternion.Euler(0, 90, 0); // back
            Pos = Vector3.right;
            TurnDir = !TurnDir;
            }
            CurDirType = 2;
            break;
            case 1: // Change diretion in vertical
            CurMoveTime = 0;
            if(TurnDir){
            this.transform.rotation = Quaternion.Euler(0, 0, 0); // right
            Pos = Vector3.forward;
            TurnDir = !TurnDir;
            }
            else{
            this.transform.rotation = Quaternion.Euler(0, 180, 0); // left
            Pos = Vector3.back;
            TurnDir = !TurnDir;
            }
            CurDirType = 2;
            break;
            case 2: // Cooldown after finish line
            if(MoveTime > CurMoveTime){ // Moving
            Debug.Log("movemove");
            rigid.velocity = speed * Pos;
            CurMoveTime += Time.deltaTime;
            }
            else{ // EndMove
            Movechk = !Movechk;
            CurMoveTime = 0f;
            CurDirType = (int)type;
            }
            break;
            default: // Over Count Solution
            CurDirType = 2;
            break;
        }
    }
}
