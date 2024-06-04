using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Component")]
    protected EnemyMovement movement;

    [Header("Health")]
    [SerializeField] protected int maxHealth;
    protected int health;

    [Header("Elements")]
    protected Player player;

    [Header("Spawn Sequence Related")]
    [SerializeField] protected new SpriteRenderer renderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected new Collider2D collider;
    protected bool hasSpawned;


    [Header("Effects")]
    [SerializeField] protected ParticleSystem passAwayParticles;

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius;

    [Header("Actions")]
    public static Action<int, Vector2, bool> onDamageTaken;
    public static Action<Vector2> onPassedAway;

    [Header("Debug")]
    [SerializeField] protected bool gizmos;

    protected virtual void Start()
    {
        Debug.Log("Enemy");

        health = maxHealth;
        movement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Destroy(gameObject);
        }

        StartSpawnSequence();
    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        return renderer.enabled;
    }

    void StartSpawnSequence()
    {
        SetRenderersVisibility(false);
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);
    }
    private void SpawnSequenceCompleted()
    {
        SetRenderersVisibility(true);
        hasSpawned = true;
        collider.enabled = true;

        movement.StorePlayer(player);
    }
    private void SetRenderersVisibility(bool visibility = true)
    {
        renderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }
    public void TakeDamage(int damage,bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;


        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

        if (health <= 0)
            PassAway();
    }
    private void PassAway()
    {
        onPassedAway?.Invoke(transform.position);
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
