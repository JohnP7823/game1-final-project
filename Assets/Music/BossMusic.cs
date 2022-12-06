using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    public int level;

    private void Awake()
    {
        GameObject dMusic;
        switch (level)
        {
            case 1:
                dMusic = GameObject.Find("Level 1 Music");
                if(dMusic != null)
                {
                    Destroy(dMusic);
                }
                break;
            case 3:
                dMusic = GameObject.Find("Level 3 Music");
                if (dMusic != null)
                {
                    Destroy(dMusic);
                }
                break;
        }
    }
}
