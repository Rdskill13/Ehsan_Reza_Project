using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hide_UnHide_Setting : MonoBehaviour
{
    private InputAction my_primary_button;
    [SerializeField] private InputActionAsset MyactionAsset;
    public GameObject my_panel;



    [SerializeField] private XRRayInteractor MyRayInteractor;


    [SerializeField] private ContinuousMoveProviderBase MyContinuousMoveProvider;
    [SerializeField] private SnapTurnProviderBase MySnapTurnProvider;

    private void Start()
    {
        my_primary_button = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Primary");
        my_primary_button.Enable();
        my_primary_button.performed += hide_unhide_menu;


    }

    private void hide_unhide_menu(InputAction.CallbackContext context)
    {
        if (GameManager.my_state_game == GameManager.State_Game.Playing || GameManager.my_state_game == GameManager.State_Game.setting_in_game )
        {
            if (my_panel == null)
            {
                return;
            }

            if (my_panel.activeInHierarchy)
            {
                my_panel.SetActive(false);


                GameManager.my_state_game = GameManager.State_Game.Playing;

                MyRayInteractor.enabled = false;

                
                Activate_Turn_Movement();
            }
            else
            {
                my_panel.SetActive(true);

                GameManager.my_state_game = GameManager.State_Game.setting_in_game;


                MyRayInteractor.enabled = true;


                Deactivate_Turn_Movement();


            }

        }

      else  if (GameManager.my_state_game == GameManager.State_Game.UnderMRI)
        {
            if (my_panel.activeInHierarchy)
            {
                my_panel.SetActive(false);


                GameManager.my_state_game = GameManager.State_Game.Playing;

                MyRayInteractor.enabled = false;


               
            }
            else
            {
                my_panel.SetActive(true);

                GameManager.my_state_game = GameManager.State_Game.setting_in_game;


                MyRayInteractor.enabled = true;


               

            }


        }
    }



    private void Deactivate_Turn_Movement()
    {
        MyContinuousMoveProvider.enabled = false;

        MySnapTurnProvider.enabled  = false;
    }

    private void Activate_Turn_Movement()
    {
        MyContinuousMoveProvider.enabled = true;

        MySnapTurnProvider.enabled = true;

    }
}
