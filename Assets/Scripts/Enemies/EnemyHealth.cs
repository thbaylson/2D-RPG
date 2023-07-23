using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 1;
    [SerializeField] private GameObject deathVFXPrefab;
    
    private Knockback knockback;
    private Flash flash;
    private int currentHealth;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damageAmount, float knockbackAmount)
    {
        //Debug.Log($"{name} took {damageAmount} damage with {knockbackAmount} knockback!");
        currentHealth = (damageAmount >= currentHealth) ? 0 : currentHealth - damageAmount;
        flash.FlashAction();
        knockback.GetKnockedback(PlayerController.Instance.transform, knockbackAmount);

        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        // Don't let the enemy die until after the flash effect happens
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>()?.SpawnItems();
            Destroy(gameObject);
        }
    }
}
