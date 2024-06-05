using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : DroppableCurrency
{
    [Header("Actions")]
    public static Action<Chest> onCollected;
    protected override void Collected()
    {
        onCollected?.Invoke(this);
        Destroy(gameObject);
    }
}