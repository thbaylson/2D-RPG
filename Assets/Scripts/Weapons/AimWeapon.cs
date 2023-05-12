using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private PlayerControls playerControls;
    private SpriteRenderer playerSpriteRenderer;
    
    private Vector2 lookDirection;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Movement.Look.performed += ctx => AimInput(ctx.ReadValue<Vector2>());

        playerSpriteRenderer = PlayerController.Instance.transform.GetComponent<SpriteRenderer>();

        // Start with the weapon facing the same direction as the player
        AimForward();
    }

    private void Update()
    {
        Aim();
    }

    private void AimForward()
    {
        float playerFlipX = playerSpriteRenderer.flipX ? -1f : 1f;
        lookDirection = new Vector2(playerFlipX, 0f);
    }

    private void AimInput(Vector2 controllerInput)
    {
        // This may cause issues on controllers with drift. May need to research deadzone implementation.
        // This makes sure there is controller input and that it isn't a diagonal
        if (!controllerInput.Equals(Vector2.zero) && controllerInput.sqrMagnitude.Equals(1f))
        {
            lookDirection = controllerInput;
        }
    }

    private void Aim()
    {
        // Make sure the player isn't aiming behind their back.
        bool aimBehind = playerSpriteRenderer.flipX && lookDirection.x == 1 || !playerSpriteRenderer.flipX && lookDirection.x == -1;
        if (aimBehind)
        {
            AimForward();
        }

        // Flip the weapon gameobject
        float controllerAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, controllerAngle);
    }
}
