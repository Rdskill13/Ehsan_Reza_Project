using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField]private InputActionAsset MyactionAsset;
    [SerializeField] private XRRayInteractor MyRayInteractor;

    private InputAction Myactivate;

    [SerializeField]private TeleportationProvider MyteleportationProvider;
    // Start is called before the first frame update

    private bool _isActive = false;


    [SerializeField] private GameObject MyTeleportationMain_placement;
    void Start()
    {

        MyRayInteractor.enabled = false;



      // InputAction Myactivate= MyactionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");

         Myactivate = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Activate");

        Myactivate.Enable();

        Myactivate.performed += OnTeleportActivate;




        //InputAction Mycancel = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
        //  InputAction Mycancel = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Cancel");

        // Mycancel.Enable();

        //  Mycancel.performed += OnTeleportCancle;

        //_thumbStick = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
        // _thumbStick.Enable();



        Activate_Deactivate_teleportation(false);
    }

    private void Update()
    {
        
       // if (!_isActive)
       // {
       //     return;
       // }

       //// if (_thumbStick.triggered) return;

       // if (!MyRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
       // {
       //     //MyRayInteractor.enabled = false;
       //     _isActive = false;

       //     return;

       // }
       // TeleportRequest request = new TeleportRequest()
       // {
       //     destinationPosition = hit.point,
       //    // destinationRotation = 
            

       // };

       // MyteleportationProvider.QueueTeleportRequest(request);

           

        }
    

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {

        if (Myactivate.IsPressed())
        {
            MyRayInteractor.enabled = true;

            _isActive = true;
            Debug.Log("Teleport Activate!!");

            Activate_Deactivate_teleportation(true);
        }

        else
        {
            MyRayInteractor.enabled = false;
            Debug.Log("Teleport DeActivate!!");


            Activate_Deactivate_teleportation(false);

        }



    }
    private void OnTeleportCancle(InputAction.CallbackContext context)
    {
        MyRayInteractor.enabled = false;

        _isActive = false;

    }

    private void Activate_Deactivate_teleportation(bool state)
    {
        MyTeleportationMain_placement.gameObject.SetActive(state);
    }
}
