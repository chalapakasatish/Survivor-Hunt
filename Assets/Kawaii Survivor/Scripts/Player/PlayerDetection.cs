using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerDetection : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Collider2D daveCollider;

    //private void FixedUpdate()
    //{
    //    Collider2D[] candyColliders = Physics2D.OverlapCircleAll(
    //        (Vector2)transform.position + daveCollider.offset, daveCollider.radius);
    //    foreach (Collider2D collider in candyColliders)
    //    {
    //        if(collider.TryGetComponent(out Candy candy))
    //        {
    //            Destroy(candy.gameObject);
    //        }
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out Candy candy))
        {
            if (!collider.IsTouching(daveCollider))
                return;

            Destroy(candy.gameObject);
        }
    }
}
