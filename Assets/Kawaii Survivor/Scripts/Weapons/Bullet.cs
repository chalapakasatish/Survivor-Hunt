using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]

public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig;
    private CircleCollider2D collider;
    RangeWeapon rangeWeapon;

    [Header("Settings")]
    private int damage;
    private bool isCriticalHit;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    private Enemy target;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        //LeanTween.delayedCall(gameObject, 5, () => rangedEnemyAttack.ReleaseBullet(this));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(int damage, Vector2 direction,bool isCriticalHit)
    {
        Invoke("Release", 1);
        this.damage = damage;
        this.isCriticalHit = isCriticalHit; 
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (target != null)
        {
            return;
        }
        if (IsInLayerMask(collider.gameObject.layer,enemyMask))
        {
            target = collider.GetComponent<Enemy>();

            CancelInvoke();
            Attack(target);
            Release();
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage, isCriticalHit);
    }

    private bool IsInLayerMask(int layer,LayerMask layerMask)
    {
        return (layerMask.value & (1<< layer)) != 0;    
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    internal void Reload()
    {
        target = null;
        rig.velocity = Vector2.zero;
        collider.enabled = true;
    }
    public void Release()
    {
        //if(!gameObject.activeSelf)
        rangeWeapon.ReleaseBullet(this);
    }
}
