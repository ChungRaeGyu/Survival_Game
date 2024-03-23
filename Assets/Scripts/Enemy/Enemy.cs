using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;

// 할일 : 공격 후 공격거리에 있을 때 한번 더 공격하게 하기
public class Enemy : MonoBehaviour
{
    public enum MoveDirectionType {horizontal, vertical};
    public MoveDirectionType type;
    Rigidbody rigid;
    public BoxCollider bcol;
    private NavMeshAgent navAg;

#region [Condition]
    bool faint = false; //      move,attack,talent (x)
    bool canAttack = false; //  move, attack (o) / talent (x)
#endregion
#region [Enemy Status]
    public float speed = 3f;
    public int maxHp = 10;
    public int curHp = 0;
    public int sensorRadius = 10;
    public int attackRadius = 1;
    private Vector3 startPos;
#endregion
#region [Attack]
    bool isAttack = false;
    public int atksenseDis = 1;
    Coroutine attackCoru;
#endregion
#region [Move]
    public Transform target;
    public bool targetFind = false;
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
        startPos = this.transform.position;
        CurDirType = (int)type;
        bcol = GetComponentInChildren<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
        curHp = maxHp;
        navAg = GetComponent<NavMeshAgent>();
        navAg.enabled = false;
    }
    // 공격 중일때 상태
    // targetFind = false, target != null, isAttack = true
    void Update()
    {
        if(!isAttack){
            if(EnemyFind(sensorRadius)){
                EnemyFollow();
                AttackMode();
            }
            else{
                if(targetFind){
                    if(target != null)
                        EnemyLost();
                    else{
                        if(navAg.remainingDistance < 0.5f){
                            BackPosEnd();
                        }
                    }
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
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Bullet"))
        {
            if(target == null)
            {
                // 다시 타겟을 넣어 따라가게 하기
                // *이건 불렛이라 불렛의 주인을 알아내어 다시 써 넣어야함 꼭
                target = other.gameObject.transform;

                if(navAg.enabled){
                    BackPosEnd();
                }
                targetFind = true; // BackPosEnd에서 targetFind를 false시켜서 true로 다시 바꿔야함
            }
            // 1.피가 닳는 함수 작성필요
            // 2.
        }
    }
    void AttackMode()
    {
        // 상대와 나의 거리재기
        Vector3 offset = target.position - transform.position;
        float sqrLen = offset.sqrMagnitude;

        if(sqrLen < atksenseDis*atksenseDis){  // 반경안에 오면 행동:공격 시전
            attackCoru = StartCoroutine(IEattack());
        }
    }
    IEnumerator IEattack()
    {
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        isAttack = true;
        yield return new WaitForSeconds(2.0f);
        IsAttack();
        yield return new WaitForSeconds(1.0f);
        bcol.enabled = false;
        if(!EnemyFind(attackRadius)){
        isAttack = false;
        }
    }
    void IsAttack()
    {
        Debug.Log("Attack!!");
        bcol.enabled = true;
    }

    bool EnemyFind(int radius)
    {
        Collider[] collider 
        = Physics.OverlapSphere(this.transform.position,radius,1 << 6);
        if(collider.Length > 0){
            target = collider[0].gameObject.transform;
            CurMoveTime = 0f;
            targetFind = true;
            return true;
        }
        return false;
    }
    void EnemyLost() // range out
    {
        EnemyFollow();
        CurMoveTime += Time.deltaTime;
        if(CurMoveTime >= targetFolTime)
        {
            CurMoveTime = 0f;
            target = null;
            BackPosStart();
        }
    }
    void EnemyFollow()
    {
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = lookRotation;
        rigid.velocity = speed * direction;
    }
    void WaitMove() // MoveCoolTime
    {
        if(CurMoveTime < MoveCD){
        CurMoveTime += Time.deltaTime;
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

    void BackPosStart()
    {
        Debug.Log("Return Back!!");
        navAg.enabled = true;
        navAg.destination = startPos;
    }
    void BackPosEnd()
    {
        Debug.Log("Return End");
        navAg.ResetPath();
        navAg.enabled = false;
        targetFind = false;
    }
}
