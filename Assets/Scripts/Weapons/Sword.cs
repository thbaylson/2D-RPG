using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform animSpawnPoint;
    // CD can't be too short or there will be issues with collision detection
    [SerializeField] private float attackCoolDown = 1f;
    [SerializeField] private WeaponInfo weaponInfo;

    private Animator myAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private Transform weaponCollider;

    private GameObject animPrefab;

    private void Awake()
    {
        playerSpriteRenderer = PlayerController.Instance.GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        // This is going to give me an aneurysm. This is clearly a code smell.
        weaponCollider = PlayerController.Instance.GetWeaponCollider();

        // I thought the above code smelled bad. This is really, really bad.
        //  TODO: Refactor this for the same reasons as above. Only the sword cares about this gameobject.
        animSpawnPoint = GameObject.Find("AnimSpawnPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance.IsControllerControls){
            AdjustDirection();
        }
        else
        {
            MouseFollowWithOffset();
        }
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        animPrefab = Instantiate(slashAnimPrefab, animSpawnPoint.position, Quaternion.identity);
        animPrefab.transform.parent = this.transform.parent;
    }

    // Animation Event
    public void SwingFinishAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    // Animation Event
    public void SwingUpFlipAnimEvent()
    {
        animPrefab.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
        
        if (PlayerController.Instance.FacingLeft)
        {
            animPrefab.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Animation Event
    public void SwingDownFlipAnimEvent()
    {
        animPrefab.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (PlayerController.Instance.FacingLeft)
        {
            animPrefab.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, 0);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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

        // Flip the weapon collider
        weaponCollider.transform.rotation = Quaternion.Euler(0, yAxisRotation, 0);
        // Flip the weapon gameobject
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, yAxisRotation, 0);
    }
}
