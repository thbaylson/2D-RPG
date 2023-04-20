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

    public void TakeDamage(int damage)
    {
        currentHealth = (damage >= currentHealth) ? 0 : currentHealth - damage;
        // Magic number represent the amount of knockback. Bigger number means more knockback. This should come from the weapon.
        knockback.GetKnockedback(PlayerController.Instance.transform, 15f);

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0) Destroy(gameObject);
    }
}
