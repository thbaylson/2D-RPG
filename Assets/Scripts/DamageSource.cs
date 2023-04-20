using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Do we only ever want to hit enemies?
        if (collision.gameObject.GetComponent<EnemyAI>())
        {

        }
    }
}
