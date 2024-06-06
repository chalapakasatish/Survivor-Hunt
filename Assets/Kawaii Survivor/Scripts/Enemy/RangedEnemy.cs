using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement),typeof(RangedEnemyAttack))]
public class RangedEnemy : Enemy
{
    private RangedEnemyAttack attack;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attack = GetComponent<RangedEnemyAttack>();

        attack.StorePlayer(player);
    }
    void Update()
    {
        if (!CanAttack())
            return;
        ManageAttack();

        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
    }
    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > playerDetectionRadius)
            movement.FollowPlayer();
            else
            TryAttack();
    }
    
    private void TryAttack()
    {
        attack.AutoAim();
    }
    
    
    
}
