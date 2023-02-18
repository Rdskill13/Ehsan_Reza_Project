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

    private InputAction Myteleportating;

    [SerializeField]private TeleportationProvider MyteleportationProvider;
    // Start is called before the first frame update

    private bool _isActive = false;


    [SerializeField] private GameObject MyTeleportationMain_placement;

    [SerializeField] private GameObject my_3dScene;

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



        Myteleportating = MyactionAsset.FindActionMap("XRI RightHand").FindAction("Select");

        Myteleportating.Enable();

        Myteleportating.performed += Myteleportating_performed;
    }

    private void Myteleportating_performed(InputAction.CallbackContext obj)
    {
        StartCoroutine(MakeCamera_Black());

        
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
        //if (MyRayInteractor == null)
        //{
        //    return;
        //}

        if (Myactivate.IsPressed())
        {
            
            MyRayInteractor.enabled = true;

            _isActive = true;
            //Debug.Log("Teleport Activate!!");

            Activate_Deactivate_teleportation(true);
        }

        else
        {
            MyRayInteractor.enabled = false;
            //Debug.Log("Teleport DeActivate!!");


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



    public IEnumerator MakeCamera_Black()
    {
        double Time_Stay = 0; //Time that is takes to grab ball.
        //GameObject MyScene = GameObject.FindGameObjectWithTag("all_3dmodel");


        State_Scene(false, my_3dScene);

        while (Time_Stay <= 2)
        {







            Time_Stay++;
            //Debug.Log("Time staty:" + Time_Stay);
            yield return new WaitForSeconds(0.05f); //just wait 1.0f second.



        }

        State_Scene(true, my_3dScene);

    }

    private void State_Scene(bool state, GameObject MyScene)
    {


        MyScene.SetActive(state);
    }

    private void OnDisable()
    {
        Myactivate.performed -= OnTeleportActivate;

        Myteleportating.performed -= Myteleportating_performed;
    }
}
