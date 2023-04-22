using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents an object that can be destroyed and does not have health. First hit immediately destroys it.
public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageSource>())
        {
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
