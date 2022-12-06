using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneInfo : MonoBehaviour
{

    private void MusicCheck()
    {
        GameObject[] dMusic = GameObject.FindGameObjectsWithTag("Music");
        if (dMusic.Length > 1)
        {
            Destroy(dMusic[1]);
        }
        dMusic[0].GetComponent<MusicClass>().PlayMusic();
    }
    private void Awake()
    {
        if(GameState.Restart == true)
        {
            GameState.ResetState();
            MusicCheck();
        }
        else
        {
            GameState.LoadSceneInfo();
            MusicCheck();
        }
        
    }
}
