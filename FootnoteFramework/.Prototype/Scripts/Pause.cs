using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [Resolve, SerializeField]
    private Menus menus;

    private bool paused = false;

    private RectTransform currentMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Unpause();
            else
                BeginPause();
        }
    }

    public void BeginPause()
    {
        paused = true;
        Time.timeScale = 0;
        menus.PauseMenu(this);
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        menus.ClearMenus();
    }
}
