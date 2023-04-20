using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 1;
    
    private Knockback knockback;
    private int currentHealth;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damageAmount, float knockbackAmount)
    {
        currentHealth = (damageAmount >= currentHealth) ? 0 : currentHealth - damageAmount;
        knockback.GetKnockedback(PlayerController.Instance.transform, knockbackAmount);

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0) Destroy(gameObject);
    }
}
