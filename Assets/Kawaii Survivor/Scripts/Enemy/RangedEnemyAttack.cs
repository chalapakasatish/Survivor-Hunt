using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
public class RangedEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private EnemyBullet bulletPrefab;

    [Header("Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    [Header("Bullet Pooling")]
    private ObjectPool<EnemyBullet> bulletPool;
    private void Start()
    {
        attackDelay = 1f / attackFrequency;
        attackTimer = attackDelay;
        bulletPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }
    private EnemyBullet CreateFunction()
    {
        EnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);
        return bulletInstance;
    }
    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }
    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }
    public void StorePlayer(Player player)
    {
        this.player = player;
    }
    public void AutoAim()
    {
        ManageShooting();
    }
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= attackDelay) 
        { 
            attackTimer = 0f;
            Shoot();
        }
    }
    Vector2 GizmosDirection;
    private void Shoot()
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;

        EnemyBullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, direction);

        //GizmosDirection = direction;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(shootingPoint.position,(Vector2)shootingPoint.position + GizmosDirection * 5);
    }

}
