using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTrig : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameState.NextScene();
        }
    }
}
