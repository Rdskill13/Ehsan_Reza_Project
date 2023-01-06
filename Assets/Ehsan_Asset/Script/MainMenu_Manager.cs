using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
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
}
