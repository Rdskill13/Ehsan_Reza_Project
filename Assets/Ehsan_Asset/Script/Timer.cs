using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditorInternal;
using UnityEngine;


public class Timer : MonoBehaviour
{
 

    
    [SerializeField]private double My_FinalTime=300;
    private double My_init_time = 0;

    [SerializeField] private TextMeshProUGUI label_time;

    private Coroutine record_timer_coroutine;





    [SerializeField]private Transform MyCamera;

    private Vector3 [] head_movment_rot =  new Vector3[2];
    private Vector3[] head_movment_pos = new Vector3[2];


    private bool is_first30 = true;

    private uint Score = 0;

    [SerializeField]private TextMeshProUGUI label_Score;
    [SerializeField] private TextMeshProUGUI label_Score_2;

    [SerializeField] private AudioSource Pos_feedback;
    [SerializeField] private AudioSource Neg_feedback;
    private void Start()
    {
        label_time.gameObject.SetActive(false); //by default is must be DeActivate.



        // my_beep_sound = GameObject.FindGameObjectWithTag("Beep_sound").GetComponent<AudioSource>();

        // my_garb_sound = GameObject.FindGameObjectWithTag("Grab_sound").GetComponent<AudioSource>();


        // can_beep = true; //value of beep_sound 
        // can_beep_basket = true;


        //first transform is prrvious 
        //second transform is current


        //  head_movment = new Transform[2];




        head_movment_rot[0].x = MyCamera.localEulerAngles.x;
        head_movment_rot[0].y = MyCamera.localEulerAngles.y;
        head_movment_rot[0].z = MyCamera.localEulerAngles.z;



        head_movment_pos[0].x = MyCamera.localPosition.x;
        head_movment_pos[0].y = MyCamera.localPosition.y;
        head_movment_pos[0].z = MyCamera.localPosition.z;



        Activate_Posfeedback(false);
        Activate_Negfeedback(false);



        label_Score_2.gameObject.SetActive(false);

    }



    public IEnumerator  Record_Timer() //Timer of pick up ball.
    {
        

            double Time_Stay = My_init_time; //Time that is takes to grab ball.



            while (Time_Stay <= My_FinalTime)
            {
                if (this != null) //if this ball is not Destroyed, check this Condition (used when hand want to grab ball and there is no)
                {
                    label_time.gameObject.SetActive(true);
                    label_time.text = Time_Stay.ToString(); //update UI Label of Timer 

                   

                }

               if (Time_Stay%10==0 && Time_Stay !=0)
              {
                
                check_movement_head(8.0f);

              }


                
                Time_Stay = Time_Stay + 1;


            //Time_Stay = Math.Round(Time_Stay, 1); //we need just one precision of  Time_Stay.

            

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


        //label_time.gameObject.SetActive(false); //if Timer ends, just DeActivate it.
        //   }

        //background_music.Play();
        // background_music.volume = 1.0f;


        //if (this == null)
        //{
        //    StopCoroutine(which_hand.timer);
        //}
        // if (this!=null)
        // {


        activation_label(false);
        make_active_deactive_Score_UI_center();


    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("XR Origin"))

        {
            Debug.Log("Trigger enter!!!");
            activation_label(true);



            record_timer_coroutine = StartCoroutine(Record_Timer());

        }


    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("XR Origin"))

        {
            activation_label(false);
            StopCoroutine(record_timer_coroutine);

            make_active_deactive_Score_UI_center();


        }
    }


    private void activation_label(bool active)
    {
        label_time.gameObject.SetActive(active);


    }


    private void check_movement_head(float threshold)
    {
        if (is_first30)
        {
            head_movment_rot[0] = new Vector3(MyCamera.localEulerAngles.x, MyCamera.localEulerAngles.y, MyCamera.localEulerAngles.z);

            is_first30 = !is_first30;

        }
        else
        {
            head_movment_rot[1] = new Vector3(MyCamera.localEulerAngles.x, MyCamera.localEulerAngles.y, MyCamera.localEulerAngles.z);

            is_first30 = !is_first30;



            


        }

        if (    Mathf.Abs(head_movment_rot[1].x - head_movment_rot[0].x) < threshold &&
                Mathf.Abs(head_movment_rot[1].y - head_movment_rot[0].y) < threshold &&
                Mathf.Abs(head_movment_rot[1].z - head_movment_rot[0].z) < threshold)
        {
            if (Mathf.Abs(head_movment_pos[1].x - head_movment_pos[0].x) < 0.4f &&
               Mathf.Abs(head_movment_pos[1].y - head_movment_pos[0].y) < 0.4f &&
               Mathf.Abs(head_movment_pos[1].z - head_movment_pos[0].z) < 0.4f)
            {


                Score++;

                label_Score.text = "امتیاز:" + Score.ToString();
                label_Score_2.text = "امتیاز:" + Score.ToString();

                Activate_Posfeedback(true);
            }

            else
            {
                Activate_Negfeedback(true);
            }

        }
        else
        {
            Activate_Negfeedback(true);
        }





    }


    private void Activate_Posfeedback(bool is_active)
    { 
        Pos_feedback.gameObject.SetActive(is_active);
        Pos_feedback.Play();

        StartCoroutine(hide_UI_pos_neg());
    }
    private void Activate_Negfeedback(bool is_active)
    { 
        Neg_feedback.gameObject.SetActive(is_active);
        Neg_feedback.Play();
        StartCoroutine(hide_UI_pos_neg());
    }

    public IEnumerator hide_UI_pos_neg()
    {

        double Time_Stay = 0; //Time that is takes to grab ball.



        while (Time_Stay <= 5)
        {




            Time_Stay = Time_Stay + 1;

            yield return new WaitForSeconds(1.0f); //just wait 0.1 second.

        }


        if (Pos_feedback.isActiveAndEnabled)
        {
            Pos_feedback.gameObject.SetActive(false);
        }
        else
        {
           Neg_feedback.gameObject.SetActive(false);

        }

    }

   

    private void make_active_deactive_Score_UI_center()
    {

       // label_Score_2.text = label_Score.text;


        if (label_Score_2.gameObject.activeInHierarchy)
        {
            label_Score_2.gameObject.SetActive(false);
        }
        else
        {
            label_Score_2.gameObject.SetActive(true);
            label_Score.gameObject.SetActive(false);

        }


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
