using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField]private InputActionAsset MyactionAsset;
    [SerializeField] private XRRayInteractor MyRayInteractor;

    private InputAction _thumbStick;

    [SerializeField]private TeleportationProvider MyteleportationProvider;
    // Start is called before the first frame update

    private bool _isActive = false;
    void Start()
    {

        MyRayInteractor.enabled = false;



       InputAction activate= MyactionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
        activate.Enable();

        activate.performed += OnTeleportActivate;


        InputAction cancel = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();

        cancel.performed += OnTeleportCancle;

       // _thumbStick = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
       // _thumbStick.Enable();

       
    }

    private void Update()
    {
        
        if (!_isActive)
        {
            return;
        }

        //if (_thumbStick.triggered) return;

        if (!MyRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            MyRayInteractor.enabled = false;
            _isActive = false;
            return;
        


        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,
           // destinationRotation = 
            

        };

        MyteleportationProvider.QueueTeleportRequest(request);

        }
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        MyRayInteractor.enabled= true;

        _isActive = true;
    }
    private void OnTeleportCancle(InputAction.CallbackContext context)
    {
        MyRayInteractor.enabled = false;

        _isActive = false;

    }
}
