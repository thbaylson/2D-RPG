using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : Singleton<CursorManager>
{
    private Image image;
    private bool ShowCursor { get; set; }

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }

    void Start()
    {
        // Hides the OS cursor
        Cursor.visible = false;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Is only true if playing from the editor.
        if (Application.isEditor)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // This will be hit when we actually build/deploy the application.
        else
        {
            // Locks the cursor to the confines of the game window.
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void Update()
    {
        // Should probably cut this out before building.
        if (Application.isEditor) { Cursor.visible = false; }

        // If a controller is being used, hide the custom cursor
        if(PlayerController.Instance.IsControllerControls) { image.enabled = false; }

        // If we're using a controller, don't interact with the mouse
        if(PlayerController.Instance.IsControllerControls) { return; }
        
        //TODO: Consider only showing the cursor when the bow is selected

        //Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorPos = Input.mousePosition;
        image.rectTransform.position = cursorPos;
    }
}
