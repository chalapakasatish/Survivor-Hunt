using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Elements")]
    private Player player;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private TextMeshPro healthText;

    [Header("Component")]
    private EnemyMovement movement;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;
    [SerializeField] private Collider2D collider;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackTimer;
    [SerializeField] private float playerDetectionRadius;

    [Header("Debug")]
    [SerializeField] private bool gizmos;

    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthText.text = health.ToString();

        player = FindFirstObjectByType<Player>();
        movement = GetComponent<EnemyMovement>();
        if (player == null)
        {
            Destroy(gameObject);
        }

        StartSpawnSequence();

        attackDelay = 1f / attackFrequency;
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
    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= attackDelay)
        {
            TryAttack();
        }
        else
        {
            Wait();
        }
    }
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDetectionRadius)
        {
            Attack();
            //PassAway();
        }
    }
    private void Attack()
    {
        Debug.Log("Dealing" + damage + "damage to player");
        attackTimer = 0;
        player.TakeDamage(damage);
    }
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        healthText.text = health.ToString();


        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
            PassAway();
    }
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }
    private void PassAway()
    {
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
