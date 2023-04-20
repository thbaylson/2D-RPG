using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform animSpawnPoint;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private SpriteRenderer playerSpriteRenderer;
    private ActiveWeapon activeWeapon;

    private GameObject animPrefab;

    private void Awake()
    {
        // This isn't great on performance. It'll change in the future.
        playerController = GetComponentInParent<PlayerController>();
        playerSpriteRenderer = playerController.GetComponent<SpriteRenderer>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    // Animation Event
    public void SwingUpFlipAnim()
    {
        animPrefab.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
        
        if (playerController.FacingLeft)
        {
            animPrefab.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Animation Event
    public void SwingDownFlipAnim()
    {
        if (playerController.FacingLeft)
        {
            animPrefab.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");

        animPrefab = Instantiate(slashAnimPrefab, animSpawnPoint.position, Quaternion.identity);
        animPrefab.transform.parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustDirection();
    }

    private void AdjustDirection()
    {
        // If the player is "flipped" on the x axis, then the sword "rotates" on the y axis
        float yAxisRotation = playerSpriteRenderer.flipX ? -180 : 0;

        // Add a little bit of directional influence in relation to the mouse/right stick
        // TODO: This is commented out until I find a way to make controller and mouse controls stop conflicting
        //Vector3 mousePos = Input.mousePosition;
        //float mouseAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        // Replace Quaternion.Euler's z axis with this and remove yAxisRotation to allow the
        //  sword to swing around in any direction
        //Vector2 lookDirection = playerControls.Movement.Look.ReadValue<Vector2>();
        //float controllerAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        activeWeapon.transform.rotation = Quaternion.Euler(0, yAxisRotation, 0);
    }
}
