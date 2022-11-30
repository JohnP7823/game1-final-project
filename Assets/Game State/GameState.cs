using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class GameState
{
    public static int PlayerHealth { get; set; }
    public static int PlayerMana { get; set; }
    public static int Lives { get; set; }
    public static int Level { get; set; }
    public static int Checkpoint { get; set; }
    public static int Score { get; set;  }
    public static GameObject thePlayer;
    public static TextMeshProUGUI scoreText, manaText;

    public static void Start()
    {
        Lives = 2;
        Level = 1;
        Checkpoint = 1;
        Score = 0;
        thePlayer = GameObject.Find("Player");
    }

    public static void RestartAtCheckpoint()
    {
        // Load the scene according to Checkpoint
        // Transfer to the beginning of the level (To code)

        // Do player reset function
        thePlayer = GameObject.Find("Player");
        thePlayer.GetComponent<PlayerController>().ResetState();
    }

    public static void RestartLevel()
    {
        // Load the scene according to Level
        // Transfer to the beginning of the level (To code)

        Score = 0; // Reset score
        AddScore(0);
        // Player reset
        thePlayer = GameObject.Find("Player");
        thePlayer.GetComponent<PlayerController>().ResetState();
    }

    public static void NextScene()
    {
        thePlayer = GameObject.Find("Player");
        PlayerHealth = thePlayer.GetComponent<PlayerController>().hp;
        PlayerMana = thePlayer.GetComponent<PlayerController>().mana;
        // Do the scene transfer with the cut to black and back.
        thePlayer = GameObject.Find("Player");
        thePlayer.GetComponent<PlayerController>().SetStats(PlayerHealth,  PlayerMana);
    }

    public static void NextLevel()
    {
        switch (Level)
        {
            case 1:
                SceneManager.LoadScene("Level 1-1");
                break;
            case 2:
                SceneManager.LoadScene("Level 2-1");
                break;
            case 3:
                SceneManager.LoadScene("Level 3-1");
                break;
        }
        thePlayer = GameObject.Find("Player");
        thePlayer.GetComponent<PlayerController>().ResetState();
    }

    public static void AddScore(int aScore)
    {
        Score = Score + aScore;
        GameObject scoreUI = GameObject.Find("Score Value Text");
        scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
        scoreText.text = Score.ToString();
    }
   
    public static void UpdateMana(int aMana)
    {
        thePlayer = GameObject.Find("Player"); // temp
        PlayerMana = thePlayer.GetComponent<PlayerController>().mana;
        GameObject manaUI = GameObject.Find("Mana Value Text");
        manaText = manaUI.GetComponent<TextMeshProUGUI>();
        manaText.text = PlayerMana.ToString();
    }
}
