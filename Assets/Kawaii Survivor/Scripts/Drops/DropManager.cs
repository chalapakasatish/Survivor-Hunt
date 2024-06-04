using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;
    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }
    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
    }
    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        Instantiate(candyPrefab,enemyPosition,Quaternion.identity,transform);
    }
}
