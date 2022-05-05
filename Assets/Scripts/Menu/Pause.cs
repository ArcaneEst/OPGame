﻿using System;
using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    public float timer;
    public bool ispuse;
    public bool guipuse;

    private void Update()
    {
        Time.timeScale = timer;
        if (Input.GetKeyDown(KeyCode.Escape) && ispuse == false)
        {
            ispuse = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && ispuse == true)
        {
            ispuse = false;
        }
        if (ispuse == true)
        {
            timer = 0;
            guipuse = true;

        }
        else if (ispuse == false)
        {
            timer = 1f;
            guipuse = false;
        }
    }

    public void OnGUI()
    {
        if (guipuse == true)
        {
            Cursor.visible = true;
            if (GUI.Button(new Rect((float)(Screen.width / 2), (float)(Screen.height / 2) - 150f, 150f, 45f), "Продолжить"))
            {
                ispuse = false;
                timer = 0;
                Cursor.visible = false;
            }
            if (GUI.Button(new Rect((float)(Screen.width / 2), (float)(Screen.height / 2) - 100, 150f, 45f), "В Меню"))
            {
                ispuse = false;
                timer = 0;
                Application.LoadLevel("Menu");
            }
        }
    }
}