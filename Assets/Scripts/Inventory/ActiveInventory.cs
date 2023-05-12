using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotInd = 0;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        // For Controller. We can use the same context scale trick, but pass either 1 or -1 and move the current index accordingly.
        playerControls.Inventory.Controller.performed += ctx => CycleActiveSlot((int)ctx.ReadValue<float>());

        // Start the game with whatever is in the first slot.
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int keyboardButton)
    {
        // The keyboard controls will range from 1-5. Our indexes of course start at zero. Subtract 1 to get the index
        //  from the keyboard button press.
        ToggleActiveHighlight(keyboardButton - 1);
    }

    private void CycleActiveSlot(int controllerInput)
    {
        // Get number of inventory slots
        int numSlots = this.transform.childCount;

        // Use modular arithmetic to cycle to the correct index.
        int modulo = (activeSlotInd + controllerInput) % numSlots;
        // If we are at 0, trying to cycle backwards needs to put us on the last index.
        int newInd = modulo < 0 ? this.transform.childCount - 1 : modulo;
        
        if (newInd != activeSlotInd)
        {
            ToggleActiveHighlight(newInd);
        }
    }

    private void ToggleActiveHighlight(int index)
    {
        activeSlotInd = index;
        foreach(Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(index).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        // Destroy the old weapon prefab
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // Get the weapon to spawn from the current active inventory slot
        Transform childTransform = transform.GetChild(activeSlotInd);
        InventorySlot inventorySlot = childTransform.GetComponent<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        // Make sure the current active inventory slot actually has something to spawn
        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.SetWeaponNull();
            return;
        }

        // Instantiate the weapon prefab
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        // Assign the newly created GameObject to our ActiveWeapon instance
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
