using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private float startingMoveSpeed = 1f;
    [SerializeField] private float dashSpeedModifier = 4f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCD = .25f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;

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
        AdjustPlayerDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
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

    private void Dash()
    {
        // TODO: Consider if we want to lock movement input while dashing. Ie, should the
        //  player be able to freely change direction mid-dash?

        // If we are already dashing, don't dash again
        if(isDashing) { return; }

        isDashing = true;
        trailRenderer.emitting = true;

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
