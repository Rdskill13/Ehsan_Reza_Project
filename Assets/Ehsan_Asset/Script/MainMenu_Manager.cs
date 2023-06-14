using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
     public GameObject my_panel;
    [SerializeField] private XRRayInteractor MyRayInteractor;
    [SerializeField] private SnapTurnProviderBase my_snap_turn;
    [SerializeField] private ContinuousMoveProviderBase my_continuous_move;


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


        if (GameManager.my_state_game == GameManager.State_Game.setting_in_game)
        {
            GameManager.my_state_game = GameManager.State_Game.Playing;

            Activate_Turn_Movement();
        }

        else
        {
            GameManager.my_state_game = GameManager.State_Game.UnderMRI;
        }
        
    }

    private void Activate_Turn_Movement()
    {
        my_continuous_move.enabled = true;

        my_snap_turn.enabled = true;

    }
}
