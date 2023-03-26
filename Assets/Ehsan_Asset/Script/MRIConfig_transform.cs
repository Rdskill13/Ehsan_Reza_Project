using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class MRIConfig_transform : BaseTeleportationInteractable
{

    [SerializeField]
    [Tooltip("The Transform that represents the teleportation destination.")]
    Transform m_TeleportAnchorTransform;

    //*********** this is my code
    [SerializeField] private bool Under_MRI = false;
    [SerializeField] private  Transform MRI_Pos_Rot;


    [SerializeField] private GameObject MyteleportationMainPlacement;

    [SerializeField] private AudioSource MRAudio;
    //***********


    //[SerializeField] private Transform camera_transform;

    [SerializeField] private Transform XROrigin_transform;

    /// <summary>
    /// The <see cref="Transform"/> that represents the teleportation destination.
    /// </summary>
    public Transform teleportAnchorTransform
    {
        get => m_TeleportAnchorTransform;
        set => m_TeleportAnchorTransform = value;
    }


    //[SerializeField]private RectTransform my_setting_menu;

    [SerializeField] private GameObject my_setting_menu_under_MRI;

    [SerializeField] private GameObject my_setting;



    // private XRInteractorLineVisual my_right_hand_lineVisual;


    protected void OnValidate()
    {
        if (m_TeleportAnchorTransform == null)
            m_TeleportAnchorTransform = transform;
    }

    /// <inheritdoc />
    protected override void Reset()
    {
        base.Reset();
        m_TeleportAnchorTransform = transform;
    }





    /// <summary>
    /// Unity calls this when drawing gizmos.
    /// </summary>
    protected void OnDrawGizmos()
    {
        if (m_TeleportAnchorTransform == null)
            return;

        Gizmos.color = Color.blue;
        GizmoHelpers.DrawWireCubeOriented(m_TeleportAnchorTransform.position, m_TeleportAnchorTransform.rotation, 1f);

        GizmoHelpers.DrawAxisArrows(m_TeleportAnchorTransform, 1f);
    }

    /// <inheritdoc />
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {

        


        if (GameManager.my_state_game == GameManager.State_Game.Playing)
        {

        


        if (m_TeleportAnchorTransform == null)
            return false;


        //FindObjectOfType<XRInteractorLineVisual>().enabled = false;

        GameObject.FindGameObjectWithTag("RightController").GetComponent<XRRayInteractor>().enabled = false;

            if (Under_MRI)
            {
                teleportRequest.destinationPosition = new Vector3(MRI_Pos_Rot.position.x, 0.8f - Camera.main.transform.localPosition.y, MRI_Pos_Rot.position.z);

                teleportRequest.destinationRotation = MRI_Pos_Rot.rotation;

                MRAudio.Play();

                //StartCoroutine(MakeCamera_Black());

                //my_cam.call_coroutine();

                GameManager.my_state_game = GameManager.State_Game.UnderMRI;



                //GameObject.FindGameObjectWithTag("RightController").GetComponent<XRInteractorLineVisual>().enabled = false;

                FindObjectOfType<ActionBasedSnapTurnProvider>().enabled = false;
                FindObjectOfType<ContinuousMoveProviderBase>().enabled = false;



                //my_setting_menu.localPosition = new Vector3(-0.546f, 0.856f, -0.087f);
                //my_setting_menu.localEulerAngles = new Vector3(0.0f,-90f,0.0f);
                //my_setting_menu.localScale = new Vector3(0.002f, 0.002f, 0.004f);

                MainMenu_Manager my_menu = GameObject.FindGameObjectWithTag("My_SceneManager").GetComponent<MainMenu_Manager>();
                my_menu.my_panel.SetActive(false);
                my_menu.my_panel = my_setting_menu_under_MRI;

               my_setting.SetActive(false);
              //  my_setting = my_setting_menu_under_MRI;


               Hide_UnHide_Setting my_menu_hide_unhide =  GameObject.FindGameObjectWithTag("RightController").GetComponent<Hide_UnHide_Setting>();
                my_menu_hide_unhide.my_panel.SetActive(false);
                my_menu_hide_unhide.my_panel = my_setting_menu_under_MRI;


               Timer my_timer =  FindObjectOfType<Timer>();
                my_timer.Setting_ScreenSpace = my_setting_menu_under_MRI;

            }
            else
            {
            // for discrete teleportation 
            // teleportRequest.destinationPosition = m_TeleportAnchorTransform.position;
            //teleportRequest.destinationRotation = m_TeleportAnchorTransform.rotation;
            //teleportRequest.destinationPosition = raycastHit.point;

            

            teleportRequest.destinationPosition = new Vector3 (raycastHit.point.x, 1.5f - Camera.main.transform.localPosition.y, raycastHit.point.z);
            teleportRequest.destinationRotation = raycastHit.transform.rotation;


            // StartCoroutine(MakeCamera_Black());
            //my_cam.call_coroutine();

           
        }


        MyteleportationMainPlacement.gameObject.SetActive(false);
        return true;

        }
        else
        {
            return false;
        }
    }

    //protected override void OnSelectEntered(SelectEnterEventArgs args)
    //{
    //    XROrigin_transform.localPosition = new Vector3(XROrigin_transform.localPosition.x, 0.8f - camera_transform.localPosition.y, XROrigin_transform.localPosition.z);


    //    Camera.main.transform
    //    base.OnSelectEntered(args);


    //}

    public IEnumerator MakeCamera_Black()
    {
        double Time_Stay = 0; //Time that is takes to grab ball.
        GameObject MyScene = GameObject.FindGameObjectWithTag("all_3dmodel");


        State_Scene(false, MyScene);

        while (Time_Stay <= 2)
        {







            Time_Stay++;
           // Debug.Log("Time staty:" + Time_Stay);
            yield return new WaitForSeconds(0.1f); //just wait 1.0f second.



        }

        State_Scene(true, MyScene);

    }

    private void State_Scene(bool state, GameObject MyScene)
    {


        MyScene.SetActive(state);
    }
}
  

