using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Calibration : MonoBehaviour
{
    private Transform camera_transform;

    [SerializeField]private Transform XROrigin_transform;
    // Start is called before the first frame update


    UnityEngine.XR.InputDevice my_head;

    List<UnityEngine.XR.XRNodeState> mynode;
    void Start()
    {
        camera_transform = GetComponent<Transform>();
       


        //my_head = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.CenterEye);
        // my_head.

        UnityEngine.XR.InputTracking.trackingAcquired += InputTracking_trackingAcquired;
        //UnityEngine.XR.InputTracking.get

    

    }

    private void InputTracking_trackingAcquired(UnityEngine.XR.XRNodeState obj)
    {
        XROrigin_transform.localPosition = new Vector3(XROrigin_transform.localPosition.x, 0.8f - camera_transform.localPosition.y, XROrigin_transform.localPosition.z);
        
    }
}
