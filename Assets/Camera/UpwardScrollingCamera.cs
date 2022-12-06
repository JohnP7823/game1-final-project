using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardScrollingCamera : MonoBehaviour
{
    public Transform player;
    public float minH, maxH;

    void Update()
    {
        if (player.position.y + 2 > minH && player.position.y + 2 < maxH)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + 2, -10); // Camera follows the player but 2 up
        }
    }
}
