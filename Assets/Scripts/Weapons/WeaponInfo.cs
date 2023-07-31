using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponName", menuName = "Items/New Weapon")]
public class WeaponInfo : ScriptableObject
{
    // TODO: Maybe refactor this to remove "weapon" from all these names.
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public int weaponDamage;
    public float weaponKnockback;
    public float weaponRange;
}
