using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D rb;
    private Knockback knockback;
    private Vector2 moveDir;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
    }

    private void FixedUpdate()
    {
        // If we're getting knocked back, do let the gameobject move on its own
        if (knockback.GettingKnockedBack) { return; }

        // TODO: Every enemy on the screen will move in sync. Maybe make this a coroutine with random start/end times?
        float randomInfluence = Random.Range(-3, 3)/10;
        rb.MovePosition(rb.position + moveDir * ((moveSpeed + randomInfluence ) * Time.fixedDeltaTime));

        spriteRenderer.flipX = moveDir.x < 0;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }

    public void StopMoving()
    {
        moveDir = Vector2.zero;
    }
}
