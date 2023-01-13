using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset MyactionAsset;

    private InputAction escape_keyboard;

    private VideoPlayer myVideo;

    [SerializeField]private Canvas Video_canvas;
    [SerializeField] private GameObject wholeWorld;
    

    private void Start()
    {

        myVideo = GetComponent<VideoPlayer>();

        escape_keyboard = MyactionAsset.FindActionMap("KeyBoard").FindAction("MyEscape");
        escape_keyboard.Enable();
        escape_keyboard.performed += SkipVideo;

        myVideo.loopPointReached += MyVideo_loopPointReached;

        wholeWorld.SetActive(false);

    }

    private void MyVideo_loopPointReached(VideoPlayer source)
    {
        this.gameObject.SetActive(false);
        Video_canvas.gameObject.SetActive(false);
        wholeWorld.SetActive(true);
    }

    private void SkipVideo(InputAction.CallbackContext obj)
    {
        this.gameObject.SetActive(false);
        Video_canvas.gameObject.SetActive(false);
        wholeWorld.SetActive(true);
    }

    private void OnDisable()
    {
        myVideo.loopPointReached -= MyVideo_loopPointReached;
        escape_keyboard.performed -= SkipVideo;
    }
}
