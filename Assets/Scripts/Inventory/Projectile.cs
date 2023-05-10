using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to DamageSource, but different enough (maybe) to warrent its own class
public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    private WeaponInfo weaponInfo;
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

    public void SetWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.gameObject.GetComponent<Indestructible>();

        if(!collision.isTrigger && (enemyHealth || indestructible))
        {
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        if(Vector3.Distance(transform.position, startPosition) > weaponInfo.weaponRange)
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
