using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator myAnimator;
    private SpriteRenderer playerSpriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        playerSpriteRenderer = PlayerController.Instance.GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustDirection();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    // Animation Event
    public void SpawnProjectileAnimEvent()
    {
        Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
    }

    private void AdjustDirection()
    {
        // If the player is "flipped" on the x axis, then the sword "rotates" on the y axis
        float yAxisRotation = playerSpriteRenderer.flipX ? -180 : 0;

        // Flip the weapon gameobject
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, yAxisRotation, 0);
    }
}
