using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MRIConfig : MonoBehaviour
{

    [SerializeField]private AudioSource MRAudio;
    [SerializeField] private Transform MainChar;



    public void ActivateSound()
    { 
        MRAudio.Play();
    }

    public void DectivateSound()
    {
        MRAudio.Stop();
    }
    
}
