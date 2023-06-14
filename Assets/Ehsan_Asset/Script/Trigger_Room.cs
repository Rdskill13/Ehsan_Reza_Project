using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Room : MonoBehaviour
{
    //120
    [SerializeField] private int timer_activation_arrow = 120;

    [SerializeField]private GameObject Teleport_area_mri;

    private static short count_inside_trigger = 0;

    public static bool inside_room_triggered = false;

   // [SerializeField] private AudioSource voice_entering_mr;

    private void Start()
    {
        count_inside_trigger = 0;
    }

    
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("inside room triggered! before 1");
        count_inside_trigger++;
        if (count_inside_trigger == 1)
        {

           // Debug.Log("inside room triggered! 1");
            StartCoroutine(Record_Timer());

            //play_sound(voice_entering_mr);

        }
    }


    private IEnumerator Record_Timer()
    {

        uint Time_start = 0;

        //Time that is must search inside room
        while (Time_start <= timer_activation_arrow)
        {


            Time_start++;

            yield return new WaitForSeconds(1.0f);

        }

        Teleport_area_mri.SetActive(true);


        inside_room_triggered = true;


    }

    //private void play_sound(AudioSource my_audio_2)
    //{
    //    my_audio_2.Play();
    //}
}
