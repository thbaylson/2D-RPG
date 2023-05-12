using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks;
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

        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void SetWeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        // Make sure if this coroutine is already running (possibly from a different weapon) to stop it first
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void AttackInput()
    {
        attackButtonDown = true;
    }

    private void Attack()
    {
        // Do not attack if we are not pressing the attack button, already attacking, or if we don't have a weapon
        if (!attackButtonDown || isAttacking || (CurrentActiveWeapon == null)) { return; }

        AttackCooldown();
        // Cast the current weapon to be IWeapon. Not sure why is isn't defined explicitly as IWeapon?
        // May get a null exception for this line if player quickly spams Attack and ChangeActiveWeapon
        (CurrentActiveWeapon as IWeapon).Attack();
        attackButtonDown = false;
    }
}
