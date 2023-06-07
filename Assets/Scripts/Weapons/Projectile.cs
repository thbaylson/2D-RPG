using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to DamageSource, but different enough (maybe) to warrent its own class
public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void SetProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if(!collision.isTrigger && (enemyHealth || indestructible || player))
        {
            // TODO: Maybe just make this "If the other thing isn't the same thing as me" ?
            if (player && isEnemyProjectile)
            {
                player.TakeDamage(1, transform);
            }

            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        if(Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            // Destroy this projectile once it exceeds its set range.
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        // Projectile rotation is handled by the weapon spawning the projectile. We just need the projectile
        //  to move "forward" relative to the xy-plane.
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
