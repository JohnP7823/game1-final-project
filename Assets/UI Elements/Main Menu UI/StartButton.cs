using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private bool start = false;
    public void StartGame()
    {
        if (!start)
        {
            start = true;
            GameState.Lives = 2;
            GameState.Level = 1;
            GameState.Checkpoint = 1;
            GameState.Score = 0;
            GameState.NextLevel();
        }
    }
}
