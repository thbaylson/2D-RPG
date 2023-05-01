using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; set; }

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
        playerControls.Combat.Attack.started += _ => AttackInput();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
    }

    public void SetWeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void AttackInput()
    {
        attackButtonDown = true;
    }

    private void Attack()
    {
        if (!attackButtonDown || isAttacking) { return; }

        isAttacking = true;
        // Cast the current weapon to be IWeapon. Not sure why is isn't defined explicitly as IWeapon?
        (CurrentActiveWeapon as IWeapon).Attack();
        attackButtonDown = false;
    }
}
