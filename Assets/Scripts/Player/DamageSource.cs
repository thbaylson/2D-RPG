using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 0;
    [SerializeField] private float knockbackAmount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Do we only ever want to hit enemies?
        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount, knockbackAmount);
        }
    }
}
