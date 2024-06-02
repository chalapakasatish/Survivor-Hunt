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

    [Header("Settings")]
    private int damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;

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
    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(IsInLayerMask(collider.gameObject.layer,enemyMask))
        {
            Attack(collider.GetComponent<Enemy>());
            Destroy(gameObject);
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    private bool IsInLayerMask(int layer,LayerMask layerMask)
    {
        return (layerMask.value & (1<< layer)) != 0;    
    }
}
