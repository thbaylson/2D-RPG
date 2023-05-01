using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currentActiveWeapon;

    private PlayerControls playerControls;
    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        Attack();
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        // TODO: Consider if we really want the player to be able to auto-swing.
        // If we are not attacking or we're already attacking, don't do the attack code
        if (!attackButtonDown || isAttacking) { return; }

        isAttacking = true;
        // Cast the current weapon to be IWeapon. Not sure why is isn't defined explicitly as IWeapon?
        (currentActiveWeapon as IWeapon).Attack();
    }
}
