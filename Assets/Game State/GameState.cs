using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class GameState
{
    public static int PlayerHealth { get; set; } = 20;
    public static int PlayerMana { get; set; } = 3;
    public static int Lives { get; set; } = 2;
    public static int Level { get; set; } = 1;
    public static int Checkpoint { get; set; } = 1;
    public static int Score { get; set; } = 0;
    public static bool Restart { get; set; } = false;
    public static bool ExtraLife { get; set; } = true;
    public static GameObject thePlayer;
    public static TextMeshProUGUI scoreText, manaText, livesText;

    public static void RestartAtCheckpoint()
    {
        // Load the scene according to Checkpoint
        string SceneName = "Level " + Level.ToString() + "-" + Checkpoint.ToString();
        SceneManager.LoadSceneAsync(SceneName);
        // Do player reset function
        Lives--;
        Restart = true;
    }

    public static void RestartLevel()
    {
        // Load the scene according to Level
        Checkpoint = 1;
        string SceneName = "Level " + Level.ToString() + "-" + Checkpoint.ToString();
        //Debug.Log(SceneName);
        SceneManager.LoadSceneAsync(SceneName);
        Score = 0; // Reset score
        Lives = 2; // Reset lives
        // Player reset
        Restart = true;
    }

    public static void ResetState()
    {
        thePlayer = GameObject.Find("Player");
        thePlayer.GetComponent<PlayerController>().ResetState();
        PlayerHealth = 20;
        PlayerMana = 3;
        AddScore(0);
        UpdateLives();
        Restart = false;
    }

    public static void NextScene()
    {
        thePlayer = GameObject.Find("Player");
        PlayerHealth = thePlayer.GetComponent<PlayerController>().hp;
        PlayerMana = thePlayer.GetComponent<PlayerController>().mana;
        Debug.Log("Player health is: " + PlayerHealth + ", Player mana is: " + PlayerMana);
        Checkpoint++;
        // Do the scene transfer
        string SceneName = "Level " + Level.ToString() + "-" + Checkpoint.ToString();
        Debug.Log(SceneName);
        SceneManager.LoadSceneAsync(SceneName);
    }

    public static void LoadSceneInfo()
    {
        thePlayer = GameObject.Find("Player");
        //Debug.Log(thePlayer.tag);
        thePlayer.GetComponent<PlayerController>().SetStats(PlayerHealth, PlayerMana);
        UpdateMana(PlayerMana);
        AddScore(0);
        UpdateLives();
    }

    public static void NextLevel()
    {
        Restart = true;
        switch (Level)
        {
            case 1:
                SceneManager.LoadSceneAsync("Level 1-1");
                break;
            case 2:
                SceneManager.LoadSceneAsync("Level 2-1");
                break;
            case 3:
                SceneManager.LoadSceneAsync("Level 3-1");
                break;
        }
        Checkpoint = 1;
    }

    public static void AddScore(int aScore)
    {
        Score = Score + aScore;
        GameObject scoreUI = GameObject.Find("Score Value Text");
        scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
        scoreText.text = Score.ToString();
        if(ExtraLife && Score > 50000)
        {
            Lives++;
            UpdateLives();
            ExtraLife = false;
        }
    }
   
    public static void UpdateMana(int aMana)
    {
        GameObject manaUI = GameObject.Find("Mana Value Text");
        manaText = manaUI.GetComponent<TextMeshProUGUI>();
        manaText.text = aMana.ToString();
    }

    public static void UpdateLives()
    {
        //Lives = Lives + aLives;
        GameObject livesUI = GameObject.Find("Lives Value Text");
        livesText = livesUI.GetComponent<TextMeshProUGUI>();
        livesText.text = Lives.ToString();
        //Debug.Log("Lives should be: " + Lives);
        //Debug.Log("Text: " + livesText.text);
    }
}
