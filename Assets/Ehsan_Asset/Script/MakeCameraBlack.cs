using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCameraBlack : MonoBehaviour
{
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


    public void call_coroutine()
    {
        StartCoroutine(MakeCamera_Black());
    
    }
}
