using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RPG.Saving;
using Newtonsoft.Json.Linq;

public class PlayerHealth : MonoBehaviour, IJsonSaveable
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float invincibilityCooldown = .05f;

    const string HEALTH_SLIDER = "HealthSlider";
    const string TOWN_SCENE = "Town";

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    public bool IsDead { get; private set; }

    private Knockback knockback;
    private Flash flash;

    private Slider healthSlider;

    private int currentHealth;
    private bool canTakeDamage = true;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        IsDead = false;
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
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();

        // TODO: knockbackThrustAmount should be based on the thing hitting the player, not a constant.
        knockback.GetKnockedback(hitTransform, knockbackThrustAmount);

        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(InvincibilityCooldownRoutine());

        UpdateState();
    }

    private void UpdateState()
    {
        UpdateHealthSlider();
        CheckPlayerDeath();
    }

    private void CheckPlayerDeath()
    {
        // If health is zero or less than zero AND we aren't already dead, die
        if(currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);

            // If health happens to be less than 0, make it be 0
            currentHealth = 0;

            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Stamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(TOWN_SCENE);
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
            healthSlider = GameObject.Find(HEALTH_SLIDER).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentHealth);
    }

    public void RestoreFromJToken(JToken state)
    {
        currentHealth = state.ToObject<int>();
        UpdateState();
    }
}
