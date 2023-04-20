using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 1;
    private int currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = (damage >= currentHealth) ? 0 : currentHealth - damage;
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0) Destroy(gameObject);
    }
}
