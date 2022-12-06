using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossDeath : MonoBehaviour
{
    public Animator myAnim;

    private void Awake()
    {
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        myAnim.SetTrigger("isDead");
        // play level clear jingle
        yield return new WaitForSeconds(5);
        GameObject thePlayer = GameObject.Find("Player");
        int manaScore = 100 * thePlayer.GetComponent<PlayerController>().mana;
        thePlayer.GetComponent<PlayerController>().mana = 0;
        GameState.AddScore(manaScore);
        GameState.UpdateMana(0);
        // play add score jingle
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("Credits");

    }
}
