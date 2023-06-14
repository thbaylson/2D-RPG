using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0,359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
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

        float angleStep, startAngle, currentAngle;
        TargetConeOfInfluence(out angleStep, out startAngle, out currentAngle);


        // This represents one complete burst
        for (int i = 0; i < burstCount; i++)
        {
            // This represents the individual bullets in a burst
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 bulletPos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                // Not sure how this is different or better than creating a property and getting the component in start?
                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.SetProjectileMoveSpeed(bulletMoveSpeed);
                }

                // Get the next angle in the cone of influence
                currentAngle += angleStep;
            }

            // Reset currentAngle between bursts
            currentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBursts);

            // Recalculate cone of influence in case player moves
            // TODO: Consider if this should stop firing bursts if player is out of attack range
            TargetConeOfInfluence(out angleStep, out startAngle, out currentAngle);
        }

        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float angleStep, out float startAngle, out float currentAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        // Rad2Deg is a constant that converts radians to degrees. Target angle is the angle (in degrees) of the
        //  line from this object to the target it's shooting at. It is also the center of the cone of influence.
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        angleStep = 0f;
        float halfAngleSpread = 0f;

        startAngle = targetAngle;
        float endAngle = targetAngle;
        currentAngle = targetAngle;

        // In other words, if angle spread IS 0, the object will shoot in a simple straight line
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;

            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }
}
