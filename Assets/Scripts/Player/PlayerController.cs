using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private TrailRenderer trailRenderer;
    // This weapon collider was made specifically for the sword weapon. Why would the player be the only
    //  reference point for this data? Why should Sword need to reach through PlayerController to see its own collider?
    [SerializeField] private Transform weaponCollider;

    [SerializeField] private float startingMoveSpeed = 1f;
    [SerializeField] private float dashSpeedModifier = 4f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCD = .25f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;
    private Knockback knockback;

    private float currentMoveSpeed;
    private bool facingLeft = false;
    private bool isDashing = false;

    // We are inheriting from Singleton, which has its own Awake. We need to make sure both get called.
    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        currentMoveSpeed = startingMoveSpeed;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //AdjustPlayerDirection();
        AdjustPlayerDirectionWithMouse();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        // Don't let the player move if we're being knocked back. TODO: This might be a place to add Melee-like DI.
        if (knockback.GettingKnockedBack) { return; }

        // Multiply floats first to make vector math more computationally efficient
        rb.MovePosition(rb.position + movement * (currentMoveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerDirection()
    {
        // We only care about updating the position if we're moving
        if (movement.x != 0)
        {
            if (movement.x < 0)
            {
                myRenderer.flipX = true;
                facingLeft = true;
            }
            else if (movement.x > 0)
            {
                myRenderer.flipX = false;
                facingLeft = false;
            }
        }
    }

    private void AdjustPlayerDirectionWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            myRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            myRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        // TODO: Consider if we want to lock movement input while dashing. Ie, should the
        //  player be able to freely change direction mid-dash?

        // If we are already dashing OR if we're out of stamina, don't dash
        if(isDashing || Stamina.Instance.CurrentStamina.Equals(0)) { return; }

        isDashing = true;
        trailRenderer.emitting = true;

        Stamina.Instance.UseStamina();

        currentMoveSpeed = startingMoveSpeed * dashSpeedModifier;
        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        currentMoveSpeed = startingMoveSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
