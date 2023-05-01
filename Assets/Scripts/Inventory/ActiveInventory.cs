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

    private void ToggleActiveHighlight(int index)
    {
        activeSlotInd = index;
        foreach(Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(index).GetChild(0).gameObject.SetActive(true);
    }
}
