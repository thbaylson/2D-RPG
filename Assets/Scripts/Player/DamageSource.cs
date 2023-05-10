using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;
    private float knockbackAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
        knockbackAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponKnockback;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Do we only ever want to hit enemies?
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount, knockbackAmount);
    }
}
