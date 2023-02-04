using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_controller : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(GameObject.Find("RightControllerPf(Clone)"));
        Destroy(GameObject.Find("LeftControllerPf(Clone)"));

    }
}
