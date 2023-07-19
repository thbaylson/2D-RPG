using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    [SerializeField] private float knockbackTime = 0.2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedback(Transform damageSource, float knockbackThrust)
    {
        GettingKnockedBack = true;
        Vector2 direction = FindKnockbackDirection(damageSource.position);
        Vector2 force = direction * knockbackThrust * rb.mass;

        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine());
    }

    /// <summary>
    /// Knockback will be (trasform.position - source.position) unless that equals zero. In that case, knockback will
    /// be a small, random Vector2.
    /// </summary>
    /// <returns>Vector2 representing the direction to be knocked back in.</returns>
    private Vector2 FindKnockbackDirection(Vector3 damageSource)
    {
        Vector2 direction = transform.position - damageSource;
        
        // If the x and y values end up at zero, or arbitrarily close to zero, find a new random direction
        if (direction.x <= float.Epsilon && direction.y <= float.Epsilon)
        {
            // Find a random direction.
            // We don't want this to be zero, so get a number from 0.01 - 0.1 and randomly make that negative about 50% of the time
            float xDirection = Random.Range(0.1f, 1f) * (Random.Range(0, 2) == 1 ? 1 : -1);
            float yDirection = Random.Range(0.1f, 1f) * (Random.Range(0, 2) == 1 ? 1 : -1);

            direction = new Vector2(xDirection, yDirection);
        }

        return direction.normalized;
    }

    private IEnumerator KnockbackRoutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
