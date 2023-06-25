using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileMoveSpeed;
    [SerializeField] private float startingDistance = 0.1f;

    [Header("Burst Shape")]
    [SerializeField] private int projectilesPerBurst;
    [Tooltip("Reminder: The first and last projectiles will fire at the same position if this is 359.")]
    [SerializeField][Range(0,359)] private float angleSpread;
    [Tooltip("Shoot projectiles one at a time instead of all at once.")]
    [SerializeField] private bool stagger;
    [Tooltip("Reminder: Oscillate does nothing if Stagger is False.")]
    [SerializeField] private bool oscillate;
    [SerializeField] private int burstCount;
    
    [Header("Timing")]
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

        float angleStep, startAngle, currentAngle, endAngle;
        float timeBetweenProjectiles = 0f;

        // This first call only exists to get endAngle. If we are not oscillating, all of these values get re-calculated immediately
        TargetConeOfInfluence(out angleStep, out startAngle, out currentAngle, out endAngle);

        if (stagger)
        {
            // TODO: This is easily refactored with a tertiary statement during declaration
            // This makes it so the time between each projectile in the burst is the same
            timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;
        }

        // This represents one complete burst
        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                // Recalculate cone of influence in case player moves
                // TODO: Consider if this should stop firing bursts if player is out of attack range
                TargetConeOfInfluence(out angleStep, out startAngle, out currentAngle, out endAngle);
            }

            // If we are oscillating and on every other burst
            if(oscillate && (i % 2 == 0))
            {
                // Recalculate on every other burst
                TargetConeOfInfluence(out angleStep, out startAngle, out currentAngle, out endAngle);
            }
            else if(oscillate)
            {
                // TODO: Fix every other burst potentially firing in the wrong direction
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            // This represents the individual projectiles in a burst
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 projectilePos = FindProjectileSpawnPos(currentAngle);

                GameObject newProjectile = Instantiate(projectilePrefab, projectilePos, Quaternion.identity);
                newProjectile.transform.right = newProjectile.transform.position - transform.position;

                // Not sure how this is different or better than creating a property and getting the component in start?
                if (newProjectile.TryGetComponent(out Projectile projectile))
                {
                    projectile.SetProjectileMoveSpeed(projectileMoveSpeed);
                }

                // Get the next angle in the cone of influence
                currentAngle += angleStep;

                // Short pause after each shot so they don't come all at once
                if (stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }

            // Reset currentAngle between bursts
            currentAngle = startAngle;
            // If we are not staggering projectiles, then fire continuously.
            // TODO: This could be its own property. Continuous firing doesn't necessarily mean staggering too
            if (!stagger)
            {
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        }

        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }

    // Notice that there is no return value. All references coming into this method are being modified and sent out again
    // via the "out" keyword.
    private void TargetConeOfInfluence(out float angleStep, out float startAngle, out float currentAngle, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        // Rad2Deg is a constant that converts radians to degrees. Target angle is the angle (in degrees) of the
        //  line from this object to the target it's shooting at. It is also the center of the cone of influence.
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        angleStep = 0f;
        float halfAngleSpread = 0f;

        startAngle = targetAngle;
        endAngle = targetAngle;
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

    private Vector2 FindProjectileSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }
}
