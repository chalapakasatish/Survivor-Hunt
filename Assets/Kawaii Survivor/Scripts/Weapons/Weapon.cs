using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack
    }
    State state;
    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private float hitDetectionRadius;
    [SerializeField] private BoxCollider2D hitCollider;

    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float aimLerp;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] Animator animator;
    [SerializeField]private List<Enemy> damagedEnemies = new List<Enemy>();
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackTimer;
    private void Start()
    {
        state = State.Idle;
    }

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
    [NaughtyAttributes.Button]
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
        if(attackTimer > attackDelay) 
        {
            attackTimer = 0;
            StartAttack();
        }
    }
    void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }
    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
        {
            return null; 
        }
        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }
    private void Attack()
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, enemyMask);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitDetectionTransform.position, hitCollider.bounds.size
            , hitDetectionTransform.localEulerAngles.z,enemyMask);

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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }
}
