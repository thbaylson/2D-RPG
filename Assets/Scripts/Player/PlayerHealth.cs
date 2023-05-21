using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float invincibilityCooldown = .05f;

    private Knockback knockback;
    private Flash flash;

    private int currentHealth;
    private bool canTakeDamage = true;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Detects collision on every frame
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy && canTakeDamage)
        {
            TakeDamage(1);
            knockback.GetKnockedback(collision.gameObject.transform, knockbackThrustAmount);
            StartCoroutine(flash.FlashRoutine());
        }
    }

    private void TakeDamage(int damageAmount)
    {
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(InvincibilityCooldownRoutine());
    }

    private IEnumerator InvincibilityCooldownRoutine()
    {
        yield return new WaitForSeconds(invincibilityCooldown);
        canTakeDamage = true;
    }
}
