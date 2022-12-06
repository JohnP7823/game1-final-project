using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    public GameObject bossWall, golemBoss, cameraM, bMusic;

    private void Awake()
    {
        bossWall.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            bossWall.SetActive(true);
            cameraM.GetComponent<LeftScrollingCamera>().scrolling = false;
            cameraM.GetComponent<Transform>().position = new Vector3(0, 0, -10);
            GameObject oMusic = GameObject.Find("Level 2 Music");
            Destroy(oMusic);
            StartCoroutine(ActivateBoss());
        }
    }

    IEnumerator ActivateBoss()
    {
        yield return new WaitForSeconds(1);
        Instantiate(bMusic, Vector2.zero, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Instantiate(golemBoss, new Vector2(0.4f, -0.4f), Quaternion.identity);
        Destroy(gameObject);
    }
}
