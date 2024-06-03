using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeapon : Weapon
{
    enum State
    {
        Idle,
        Attack
    }
    State state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitCollider;

    [SerializeField] private List<Enemy> damagedEnemies = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                break;
            case State.Attack:
                Attacking();
                break;
            default:
                break;
        }
    }
    void Attacking()
    {
        Attack();
    }
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;

            transform.up = targetUpVector;
            ManageAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncrementAttackTimer();
    }
    void ManageAttack()
    {
        if (attackTimer > attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }
    void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }
    void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;
        damagedEnemies.Clear();
        animator.speed = 1f / attackDelay;
    }
    void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }
    private void Attack()
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, enemyMask);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitDetectionTransform.position, hitCollider.bounds.size
            , hitDetectionTransform.localEulerAngles.z, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            if ((!damagedEnemies.Contains(enemy)))
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);
            }
        }
    }
}
