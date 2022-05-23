using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMenuVisibility : MonoBehaviour
{
    [SerializeField]
    private BooleanVariable isInCursorMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isInCursorMenu.RegisterListener(OnCursorMenuChanged);
    }

    private void OnDestroy()
    {
        isInCursorMenu.UnregisterListener(OnCursorMenuChanged);
    }

    // Constantly check in case user minimises and then restores the window; the cursor will be visible again
    void Update()
    {
        if (
            !isInCursorMenu.Value
            && Application.isFocused
            && Cursor.lockState != CursorLockMode.Locked
        )
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnCursorMenuChanged(bool isInCursorMenuValue)
    {
        // We're only worrying about unhiding the cursor. See update loop for hiding cursor logic
        if (isInCursorMenuValue)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
