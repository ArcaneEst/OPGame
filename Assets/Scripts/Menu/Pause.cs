using System;
using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    public float timer;
    public bool ispuse;
    public bool guipuse;
    public bool isdead;

    private void Start()
    {
        Player.OnPlayerDeath += OnDeath;
    }

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

            var width = Screen.width / 2;
            var height = Screen.height / 8;
            
            
            var x = Screen.width / 4;
            var y = Screen.height / 2 - (float)(1.5 * height);
            
            var guiStyle = GUI.skin.button;
            guiStyle.fontSize = 30;
            
            
            if (GUI.Button(new Rect(x, y, width, height), "Продолжить", guiStyle))
            {
                ispuse = false;
                timer = 0;
                Cursor.visible = false;
            }
            
            if (GUI.Button(new Rect(x, y + height, width, height), "Рестарт", guiStyle))
            {
                ispuse = false;
                timer = 0;
                Application.LoadLevel("TestLevel");
            }
            
            if (GUI.Button(new Rect(x, y + 2 * height, width, height), "В Меню", guiStyle))
            {
                ispuse = false;
                timer = 0;
                Application.LoadLevel("Menu");
            }
        }

        if (isdead)
        {
            isdead = true;
        
            Cursor.visible = true;

            var width = Screen.width / 2;
            var height = Screen.height / 8;
        
        
            var x = Screen.width / 4;
            var y = Screen.height / 2;
        
            var guiStyle = GUI.skin.button;
            guiStyle.fontSize = 30;

            var labelStyle = GUI.skin.label;
            labelStyle.fontSize = 40;
            labelStyle.alignment = TextAnchor.UpperCenter;
            
            GUI.Label(new Rect(x, y, width, height), "You Died", labelStyle);


            if (GUI.Button(new Rect(x, y + 0.5f * height, width, height), "Рестарт", guiStyle))
            {
                ispuse = false;
                timer = 0;
                Application.LoadLevel("TestLevel");
            }
        
            if (GUI.Button(new Rect(x, y + 1.5f * height, width, height), "В Меню", guiStyle))
            {
                ispuse = false;
                timer = 0;
                Application.LoadLevel("Menu");
            }
        }
    }

    private void OnDeath()
    {
        isdead = true;
    }
}