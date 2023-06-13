using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float shootCooldown = 1f;

    // TODO: Maybe make this more generic, like "IsAttacking" and add getter to interface?
    private bool isShooting = false;

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }

    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        // Shoot projectiles in defined bursts
        for(int i = 0; i < burstCount; i++)
        {
            Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.transform.right = targetDirection;

            // Not sure how this is different or better than creating a property and getting the component in start?
            if (newBullet.TryGetComponent(out Projectile projectile))
            {
                projectile.SetProjectileMoveSpeed(bulletMoveSpeed);
            }

            yield return new WaitForSeconds(timeBetweenBursts);
        }

        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }
}
