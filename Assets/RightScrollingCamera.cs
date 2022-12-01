using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightScrollingCamera : MonoBehaviour
{
    public Transform player;
    public float minD, maxD, minH, maxH;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if(player.position.x + 2 > minD && player.position.x + 2 < maxD)
        {
            transform.position = new Vector3(player.position.x + 2, transform.position.y, -10); // Camera follows the player but 2 to the right
        }
        if(player.position.y + 3.05 > minH && player.position.y + 3.05 < maxH)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + 3.05f, -10);
        }
    }
}
