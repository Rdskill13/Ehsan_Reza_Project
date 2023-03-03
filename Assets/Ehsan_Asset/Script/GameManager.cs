using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State_Game 
    {
        Watching_Movie,
        Playing,
        UnderMRI,
        setting_in_game
    
    };

    public static State_Game my_state_game;

    private void Start()
    {
        my_state_game = State_Game.Watching_Movie;
    }
}
