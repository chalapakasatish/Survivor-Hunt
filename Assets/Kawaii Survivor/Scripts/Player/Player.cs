using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    private PlayerHealth playerHealth;
    [SerializeField] private CircleCollider2D collider;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }
    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + collider.offset;
    }
}
