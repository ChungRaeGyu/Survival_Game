using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Vector3 _Direction;

    [SerializeField]private float _Speed = 3f;

    private IObjectPool<Bullet> _ManagedPool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_Direction*Time.deltaTime*_Speed);
    }

    public void SetManagedPool(IObjectPool<Bullet> pool){
        _ManagedPool = pool;
    }
    public void Shoot(Vector3 dir){
        _Direction = dir;
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet(){
        yield return new WaitForSecondsRealtime(5f);
        _ManagedPool.Release(this);
    }
}
