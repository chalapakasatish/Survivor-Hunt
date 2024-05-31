using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;

    public void Animate(int damage)
    {
        damageText.text = damage.ToString();
        animator.Play("Animate");
    }
}
