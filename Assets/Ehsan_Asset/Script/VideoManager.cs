using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Video;
using Sigtrap.VrTunnellingPro;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset MyactionAsset;

    private InputAction escape_keyboard;

    private VideoPlayer myVideo;

    [SerializeField]private Canvas Video_canvas;
    [SerializeField] private GameObject wholeWorld;

    [SerializeField] private OnboardingAutoStart my_onBoadring;


    [SerializeField] private AudioSource voice_entering_mr;
    private void Start()
    {

        myVideo = GetComponent<VideoPlayer>();

        //escape_keyboard = MyactionAsset.FindActionMap("KeyBoard").FindAction("MyEscape");

        escape_keyboard = MyactionAsset.FindActionMap("XRI LeftHand").FindAction("Secondary");
        escape_keyboard.Enable();
        escape_keyboard.performed += SkipVideo;

        myVideo.loopPointReached += MyVideo_loopPointReached;

        wholeWorld.SetActive(false);

        my_onBoadring.gameObject.SetActive(false);


        FindObjectOfType<ContinuousMoveProviderBase>().enabled = false;
        FindObjectOfType<ContinuousTurnProviderBase>().enabled = false;
        FindObjectOfType<Tunnelling>().enabled = false;



    }

    private void MyVideo_loopPointReached(VideoPlayer source)
    {
        this.gameObject.SetActive(false);
        Video_canvas.gameObject.SetActive(false);
        wholeWorld.SetActive(true);
        my_onBoadring.gameObject.SetActive(true);

        GameManager.my_state_game = GameManager.State_Game.Playing;


        FindObjectOfType<ContinuousMoveProviderBase>().enabled = true;
        FindObjectOfType<ContinuousTurnProviderBase>().enabled = true;


        FindObjectOfType<Tunnelling>().enabled = true;

        play_sound(voice_entering_mr);

    }

    private void SkipVideo(InputAction.CallbackContext obj)
    {
        this.gameObject.SetActive(false);
        Video_canvas.gameObject.SetActive(false);
        wholeWorld.SetActive(true);
        my_onBoadring.gameObject.SetActive(true);

        GameManager.my_state_game = GameManager.State_Game.Playing;

        FindObjectOfType<ContinuousMoveProviderBase>().enabled = true;
        FindObjectOfType<ContinuousTurnProviderBase>().enabled = true;

        FindObjectOfType<Tunnelling>().enabled = true;

        play_sound(voice_entering_mr);

    }

    private void OnDisable()
    {
        myVideo.loopPointReached -= MyVideo_loopPointReached;
        escape_keyboard.performed -= SkipVideo;
    }

    private void play_sound(AudioSource my_audio_2)
    {
        my_audio_2.Play();
    }
}
