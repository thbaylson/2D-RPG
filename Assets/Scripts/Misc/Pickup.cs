using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Tooltip("A pickup distance of 0 will disable movement towards the player.")]
    [SerializeField] private float startPickupDistance = 0f;
    [Tooltip("Acceleration of the pickup as it moves towards the player.")]
    [SerializeField] private float accelerationRate = 0.2f;
    [Tooltip("Starting move speed of the pickup.")]
    [SerializeField] private float startMoveSpeed = 0f;

    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    private Rigidbody2D rb;
    private Vector3 moveDir;
    private float pickupDistance = 0f;
    private float moveSpeed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        pickupDistance = startPickupDistance;
        moveSpeed = startMoveSpeed;
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    // Each frame we get information about move direction and speed, but we don't actually move
    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        // Moving only matters if this is a pickup that gravitates towards the player
        if(pickupDistance > 0)
        {
            // When the player is within pickupDistance, gravitate towards the player
            if (Vector3.Distance(transform.position, playerPos) < pickupDistance)
            {
                // Once we start gravitating, double the pickup distance to better gravitate towards the player
                pickupDistance = (pickupDistance > startPickupDistance) ? pickupDistance : pickupDistance * 2;

                moveDir = (playerPos - transform.position).normalized;
                moveSpeed += accelerationRate;
            }
            else
            {
                // Stop moving and reset moveSpeed if the player is too far
                moveDir = Vector3.zero;
                moveSpeed = startMoveSpeed;
            }
        }
    }

    // This is where we actually set the velocity of the pickup
    private void FixedUpdate()
    {
        // Velocity only matters if this is a pickup that gravitates towards the player
        if (startPickupDistance > 0)
        {
            rb.velocity = moveDir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(startPos.x + Random.Range(-0.5f, 0.5f), startPos.y + Random.Range(-0.5f, 0.5f));

        float timePassed = 0f;
        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float timeOverDuration = timePassed / popDuration;
            float heightOverTime = animCurve.Evaluate(timeOverDuration);
            float currentHeight = Mathf.Lerp(0f, heightY, heightOverTime);

            transform.position = Vector2.Lerp(startPos, endPos, timeOverDuration) + new Vector2(0f, currentHeight);

            yield return null;
        }
    }
}
