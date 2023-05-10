using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;
    
    private CapsuleCollider2D capCollider;
    private Vector2 startColliderSize;
    private Vector2 startColliderOffset;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    private float range;

    private void Awake()
    {
        capCollider = GetComponent<CapsuleCollider2D>();
        startColliderSize = capCollider.size;
        startColliderOffset = capCollider.offset;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerSpriteRenderer = PlayerController.Instance.transform.GetComponent<SpriteRenderer>();
        AdjustDirection();
    }

    public void UpdateLaserRange(float range)
    {
        this.range = range;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < range)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, range, linearT), 1f);
            
            capCollider.size = new Vector2(Mathf.Lerp(startColliderSize.x, range, linearT), startColliderSize.y);
            capCollider.offset = new Vector2(Mathf.Lerp(startColliderOffset.x, range/2, linearT), startColliderOffset.y);
            
            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void AdjustDirection()
    {
        float playerFlipX = playerSpriteRenderer.flipX ? -1f : 1f;
        Vector2 lookDirection = new Vector2(playerFlipX, 0f);

        // Flip the weapon gameobject
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }
}
