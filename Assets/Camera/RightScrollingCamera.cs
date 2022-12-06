using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightScrollingCamera : MonoBehaviour
{
    public Transform player;
    public float minD, maxD;

    void Update()
    {
        if(player.position.x + 2 > minD && player.position.x + 2 < maxD)
        {
            transform.position = new Vector3(player.position.x + 2, transform.position.y, -10); // Camera follows the player but 2 to the right
        }
    }
}
