using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public static PlayerController Instance;

    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeedModifier = 4f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCD = .25f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    private bool facingLeft = false;
    private bool isDashing = false;

    private void Awake()
    {
        Instance = this;
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
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
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
        // If we are already dashing, don't dash again
        if(isDashing) { return; }

        isDashing = true;
        trailRenderer.emitting = true;

        // This seems like a bad idea. What if the player saves/quits in the middle of a dash?
        moveSpeed *= dashSpeedModifier;
        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeedModifier;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
