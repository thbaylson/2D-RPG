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
        //TODO: Consider only showing the cursor when the bow is selected

        //Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorPos = Input.mousePosition;
        image.rectTransform.position = cursorPos;

        // Should probably cut this out before building.
        if (Application.isEditor) { Cursor.visible = false; }
    }
}
