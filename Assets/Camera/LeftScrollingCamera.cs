using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftScrollingCamera : MonoBehaviour
{
    public Transform player;
    public float minD, maxD;
    public bool scrolling = true;

    void Update()
    {
        if (scrolling && player.position.x - 2 > minD && player.position.x - 2 < maxD)
        {
            transform.position = new Vector3(player.position.x - 2, transform.position.y, -10); // Camera follows the player but 2 to the left
        }
    }
}
