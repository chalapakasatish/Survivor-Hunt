using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    //
    // Summary:
    //     A stack based
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab;
    [Header("Pooling")]
    [SerializeField]private ObjectPool<DamageText> damageTextPool;
    private void Awake()
    {
        MeeleEnemy.onDamageTaken += EnemyHitCallback;
    }
    private void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }
    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform);
    }
    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }
    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }
    private void OnDestroy()
    {
        MeeleEnemy.onDamageTaken -= EnemyHitCallback;
    }
    private void EnemyHitCallback(int damage,Vector2 enemyPos)
    {
        DamageText damageTextInstantiate = damageTextPool.Get();

        Vector3 spawnPosition = enemyPos + Vector2.up * 1f;
        damageTextInstantiate.transform.position = spawnPosition;

        damageTextInstantiate.Animate(damage);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstantiate));
    }
}
