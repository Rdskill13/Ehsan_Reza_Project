﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditorInternal;
using UnityEngine;

public class Ball : MonoBehaviour
{
 

    
    [SerializeField]private double My_FinalTime=300;
    private double My_init_time = 0;

    private TextMeshProUGUI label_time;

    private void Start()
    {
        label_time = GetComponentInChildren<TextMeshProUGUI>(); //get UI Label Element which is Childern of ball(UI Label Timer)
        label_time.gameObject.SetActive(false); //by default is must be DeActivate.



       // my_beep_sound = GameObject.FindGameObjectWithTag("Beep_sound").GetComponent<AudioSource>();

       // my_garb_sound = GameObject.FindGameObjectWithTag("Grab_sound").GetComponent<AudioSource>();


       // can_beep = true; //value of beep_sound 
       // can_beep_basket = true;


       


    

    }



   
   

   

    

   

    public IEnumerator  Record_Timer() //Timer of pick up ball.
    {
      

            double Time_Stay = My_init_time; //Time that is takes to grab ball.



            while (Time_Stay == 500)
            {
                if (this != null) //if this ball is not Destroyed, check this Condition (used when hand want to grab ball and there is no)
                {
                    label_time.gameObject.SetActive(true);
                    label_time.text = Time_Stay.ToString(); //update UI Label of Timer 

                   

                }


                
                Time_Stay = Time_Stay + 1;


            Time_Stay = Math.Round(Time_Stay, 1); //we need just one precision of  Time_Stay.

            

            //if (can_beep) //when timer of grabbing ball starts, beep_sound play. when Ball goes of collision area, this sound stop.
            //{
            //    my_beep_sound.Play();
            //    //my_beep_sound.loop = true;

            //    can_beep = false; //in order to not have unpleasent sound.

            //}
              

             
            
          
            yield return new WaitForSeconds(1.0f); //just wait 0.1 second.

            }

       // my_beep_sound.Stop(); //when time of grab finished, beep_sound must stoped.

       // my_garb_sound.Play(); //grab sound must play.

        
         label_time.gameObject.SetActive(false); //if Timer ends, just DeActivate it.
                                                        //   }

        //background_music.Play();
       // background_music.volume = 1.0f;


        //if (this == null)
        //{
        //    StopCoroutine(which_hand.timer);
        //}
        // if (this!=null)
        // {
      



            
            }




       //     TimeSpan saat = DateTime.Now.TimeOfDay; //get time when pick up ball happen.
       //         uint hour = (uint)saat.Hours;
       //         uint min = (uint)saat.Minutes;
       //         uint sec = (uint)saat.Seconds;
       //     //****


       //         for (int i = Ball_manager.index_history; i >= 1; i--) // if new record Generate, shift down all Records.
       //         {
       //             Ball_manager.history_lable_UI[i].text
       //             =
       //          Ball_manager.history_lable_UI[i - 1].text;
       //         }

       //     //    Ball_manager.history_lable_UI[0].text = //create new record and put it in zero position of array.
       //     //"Shoulder:" + shoulder + ", Elbow:" + elbow +
       //     //", Wrist:" + 360 + ", Hand:" + "Right" + ", State: " + "Pick";



       //     Ball_manager.history_lable_UI[0].text = //create new record and put it in zero position of array.
       //shoulder.ToString() + "                                " + elbow.ToString() + "                                  " + "360"
       //+ "                                         " + "R" + "                                 " + "Pick";

       //     Ball_manager.index_history++; //each new Record generate, index_history must increase.


       //     time_that_PickUp = Math.Round(Time.time , 1); //Time  the Ball grabbed.

       //     //double diff_pickup_start = Math.Round(time_that_PickUp - Ball_manager.time_start_objective_current_ball
       //     //                                                        - Ball_manager.time_setup_scenario, 1);

       //     double diff_pickup_start = Math.Round(time_that_PickUp - Ball_manager.time_start_objective_current_ball
       //                                                             , 1); //delta t1

       //     //**Write to CSV file **
       //     WriteToCSV.write_record(Get_UserName.user_name, shoulder, elbow, "360", "Right", "Pick", hour + ":" + min + ":" + sec
       //             , diff_pickup_start.ToString(),"--"
       //             ,(Ball_manager.My_Scenario.Next_Scenario-1).ToString()
       //              , this.num_clock
       //             ,"--");

            


           

          









        



    //public    void score() //Scoreing mechanism, for example when ball is in clock 5 or 7, it has 1 Score, if he puts it, into basket.
    //{
       
       

    //    switch (Mathf.RoundToInt(float.Parse(num_clock)).ToString())
    //    {
    //        case "5":
    //        case "7":
    //          //  Ball_Score = 10;
    //            break;

    //        case "4":
    //        case "8":
    //          //  Ball_Score = 20;
    //            break;

    //        case "3":
    //        case "9":
    //          //  Ball_Score = 30;
    //            break;

            
    //        case "2":
    //        case "10":
    //          //  Ball_Score = 40;
    //            break;
    //        case "1":
    //        case "11":
            
    //           // Ball_Score = 50;
    //            break;



    //        //case "12_L":  
    //        //case "12_R":
    //        //    Ball_Score = 60;
    //        //    break;

    //        //case "1.5":
    //        //case "10.5":
    //        //    Ball_Score = 50;
    //        //    break;

    //        case "0":
    //        case "12":
    //           // Ball_Score = 60;
    //            break;


    //        default:
    //           // Ball_Score = 0;
    //            break;
    //    }





    //}



    //public string set_space_try(string joint)
    //{
    //    switch (joint.Length)
    //    {
    //        case 5:
    //            return joint;
    //            //break;
    //        case 4:
    //            return joint + " ";
    //            // break;
    //        case 3:
    //            return joint + "  ";
    //        case 2:
    //            return joint + "   ";
    //        default:
    //            return joint;
    //           // break;



    //    }

        
    //}

    



    
    //private void OnMouseDrag()
    //{

    //    Debug.Log("Mouse Dragged " + this.gameObject.name);
    //}
}