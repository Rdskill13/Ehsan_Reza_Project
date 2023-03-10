using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
     public GameObject my_panel;
    [SerializeField] private XRRayInteractor MyRayInteractor;


    public void enter_MainGame()
    {
        //1 MainGame
        SceneManager.LoadScene(1);
    }
    public void enter_MenuScene()
    {
        Debug.Log("ASSASAS");
        //0 MenuScene
        SceneManager.LoadScene(0);
    }
    public void enter_RecordScene()
    { 
    //----------
    }
    public void Exit_Game()
    {

        Application.Quit();
    }

    public void hide_menu_setting()
    {

        my_panel.SetActive(false);
        MyRayInteractor.enabled = false;
        GameManager.my_state_game = GameManager.State_Game.Playing;
    }
}
