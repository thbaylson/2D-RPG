using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float invincibilityCooldown = .05f;

    private Knockback knockback;
    private Flash flash;

    private Slider healthSlider;

    private int currentHealth;
    private bool canTakeDamage = true;

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    // Detects collision on every frame
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if(!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();

        // TODO: knockbackThrustAmount should be based on the thing hitting the player, not a constant.
        knockback.GetKnockedback(hitTransform, knockbackThrustAmount);
        
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(InvincibilityCooldownRoutine());

        Debug.Log($"Player Health: {currentHealth}");
        UpdateHealthSlider();
        CheckPlayerDeath();
    }

    private void CheckPlayerDeath()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Death");
        }
    }

    private IEnumerator InvincibilityCooldownRoutine()
    {
        yield return new WaitForSeconds(invincibilityCooldown);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
