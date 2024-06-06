using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement))]
public class MeeleEnemy : Enemy
{
    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackTimer;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackDelay = 1f / attackFrequency;
    }
    
    void Update()
    {
        if (!CanAttack())
            return;
        if (attackTimer >= attackDelay)
        {
            TryAttack();
        }
        else
        {
            Wait();
        }
        movement.FollowPlayer();
    }
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDetectionRadius)
        {
            Attack();
        }
    }
    private void Attack()
    {
        Debug.Log("Dealing" + damage + "damage to player");
        attackTimer = 0;
        player.TakeDamage(damage);
    }
    
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }
    
}
