using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected float aimLerp;

    [Header("Attack")]
    [SerializeField] protected int damage;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackTimer;

    protected Enemy GetClosestEnemy()
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
    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if(Random.Range(0,101) <= 50)
        {
            isCriticalHit = true;
            return damage * 2;
        }
        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
