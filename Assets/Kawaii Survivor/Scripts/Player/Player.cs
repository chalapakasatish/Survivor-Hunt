using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth),typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    //
    // Summary:
    //     Player Class
    public static Player instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
    }

    [Header("Components")]
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;
    [SerializeField] private CircleCollider2D collider;

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }
    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + collider.offset;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}
