using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hide_UnHide_Setting : MonoBehaviour
{
    private InputAction my_primary_button;
    [SerializeField] private InputActionAsset MyactionAsset;
    [SerializeField]private GameObject my_panel;
    private void Start()
    {
        my_primary_button = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Primary");
        my_primary_button.Enable();
        my_primary_button.performed += hide_unhide_menu;


    }

    private void hide_unhide_menu(InputAction.CallbackContext context)
    {
        if (my_panel ==null)
        {
            return;
        }

        if (my_panel.activeInHierarchy)
        {
            my_panel.SetActive(false);
        }
        else
        {
            my_panel.SetActive(true);

        }


    }
}
