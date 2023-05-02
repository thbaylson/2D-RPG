using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private PlayerControls playerControls;
    private SpriteRenderer playerSpriteRenderer;
    private Transform weaponCollider;
    
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
        // This is going to give me an aneurysm. This is clearly a code smell.
        weaponCollider = PlayerController.Instance.GetWeaponCollider();

        playerControls.Movement.Look.performed += ctx => AimInput(ctx.ReadValue<Vector2>());

        playerSpriteRenderer = PlayerController.Instance.transform.GetComponent<SpriteRenderer>();

        // TODO: Start the weapon facing the same direction as the player
    }

    private void Update()
    {
        Aim();
    }

    private void AimInput(Vector2 controllerInput)
    {
        // This may cause issues on controllers with drift. May need to research deadzone implementation.
        // This makes sure there is control input, that it isn't a diagonal, (TODO:) and that it isn't pointing behind the player
        if (!controllerInput.Equals(Vector2.zero) && controllerInput.sqrMagnitude.Equals(1f))
        {
            Debug.Log($"Controller Input: {controllerInput};");
            lookDirection = controllerInput;
        }
    }

    private void Aim()
    {
        // If the player is "flipped" on the x axis, then the weapon "rotates" on the y axis
        float yAxisRotation = playerSpriteRenderer.flipX ? -180f : 0f;

        // Replace Quaternion.Euler's z axis with this and remove yAxisRotation to allow the
        //  sword to swing around in any direction
        //Vector2 lookDirection = playerControls.Movement.Look.ReadValue<Vector2>();
        float controllerAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;


        // Flip the weapon collider
        weaponCollider.transform.rotation = Quaternion.Euler(0f, 0f, controllerAngle);
        // Flip the weapon gameobject
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, controllerAngle);
    }
}
