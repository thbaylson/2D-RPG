using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : MonoBehaviour
{
    [SerializeField] WeaponInfo weapon;
    public WeaponInfo Weapon { get { return weapon; } }
    public SpriteRenderer InventorySprite { get; private set; }

    // Anti double-collision prevention
    private bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        InventorySprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with something!");

        // If this thing has a PlayerHealth, it is the player. Else do nothing
        if (collision.gameObject.GetComponent<PlayerController>() && !collided)
        {
            collided = true;

            Debug.Log("Collided with player");
            ActiveInventory.Instance.AddItem(this);

            // We only want to pick up items once
            Destroy(gameObject);
        }
    }
}
