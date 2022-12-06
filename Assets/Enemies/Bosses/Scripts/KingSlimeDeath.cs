using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDeath : MonoBehaviour
{
    public Animator myAnim;

    private void Awake()
    {
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        myAnim.SetBool("isDead", true);
        if(gameObject.name == "Dead Golem")
        {
            myAnim.SetTrigger("isDead");
            GameObject dMusic = GameObject.Find("Level 2 Music");
            Destroy(dMusic);
        }
        else
        {
            myAnim.SetBool("isDead", true);
        }
        // play level clear jingle
        yield return new WaitForSeconds(5);
        GameObject thePlayer = GameObject.Find("Player");
        int manaScore = 100 * thePlayer.GetComponent<PlayerController>().mana;
        thePlayer.GetComponent<PlayerController>().mana = 0;
        GameState.AddScore(manaScore);
        GameState.UpdateMana(0);
        // play add score jingle
        yield return new WaitForSeconds(3);
        GameState.Level++;
        GameState.NextLevel();
    }
}
