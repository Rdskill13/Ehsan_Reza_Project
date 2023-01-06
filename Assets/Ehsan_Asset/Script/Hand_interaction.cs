using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_interaction : MonoBehaviour
{
    private MainMenu_Manager my_main_menu;

    private void Start()
    {
        if (!GameObject.FindGameObjectWithTag("My_SceneManager").GetComponent<MainMenu_Manager>())
        {
            return;
        }

        my_main_menu = GameObject.FindGameObjectWithTag("My_SceneManager").GetComponent<MainMenu_Manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Exit_Main_Menu")
        {
            my_main_menu.enter_MenuScene();
        }
        else if (other.gameObject.name == "ExitGame_Menu")
        {
            my_main_menu.Exit_Game();
        }
    }
}
