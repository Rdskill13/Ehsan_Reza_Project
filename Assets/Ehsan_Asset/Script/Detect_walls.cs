using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Detect_walls : MonoBehaviour
{

    private static Ray MyRay;


    private XRRayInteractor MyRayInteractor;

    void Start()
    {
        // MyRay = new Ray(transform.position,transform.forward);

        MyRayInteractor = GetComponent<XRRayInteractor>();
    }

    private void Update()
    {
        detect_walls();
    }



    public void  detect_walls()
    {

        MyRay = new Ray(transform.position, transform.forward);


        // Debug.DrawRay(transform.position, transform.forward, Color.blue);
        // Debug.DrawLine(transform.position, transform.forward, Color.blue);
        if (Physics.Raycast(MyRay, out RaycastHit hitInfo, 20))
        {
            // Debug.Log("inside raycast");
            // Debug.DrawRay(transform.position, transform.forward, Color.green);

            if (hitInfo.transform.gameObject.tag == "wall")
            {
                MyRayInteractor.enabled = false;

                // Debug.Log("wall triggered");
               // return true;

            }

          //  return false;

        }

       // return false;

    }
}
