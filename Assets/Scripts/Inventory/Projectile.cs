using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    
    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        // Projectile rotation is handled by the weapon spawning the projectile. We just need the projectile
        //  to move "forward" relative to the xy-plane.
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
