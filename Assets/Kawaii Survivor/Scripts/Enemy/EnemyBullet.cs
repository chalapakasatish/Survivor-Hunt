using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(CircleCollider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig;
    private CircleCollider2D collider;
    private RangedEnemyAttack rangedEnemyAttack;

    [Header("Settings")]
    private int damage;
    [SerializeField] private float moveSpeed;
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        LeanTween.delayedCall(gameObject, 5, () => rangedEnemyAttack.ReleaseBullet(this));
    }
    public void Configure(RangedEnemyAttack rangedEnemyAttack) 
    { 
        this.rangedEnemyAttack = rangedEnemyAttack;
    }
    public void Shoot(int damage,Vector2 direction)
    {
        this.damage = damage;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            LeanTween.cancel(gameObject);
            player.TakeDamage(damage);
            this.collider.enabled = false;
            rangedEnemyAttack.ReleaseBullet(this);
        }
    }

    public void Reload()
    {
        rig.velocity = Vector2.zero;
        collider.enabled = true;
    }
}
