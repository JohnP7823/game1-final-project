using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyBase
{

    // Update is called once per frame
    void Update()
    {
    }

    protected override void Attack()
    {
        Debug.Log("Slime does not actively attack");
    }

    protected override void Awake()
    {
        thePlayer = GameObject.Find("Player");
        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            flipped = true;
            myRig.velocity = new Vector2(-1, 0) * 7;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 7;
        }
    }
}
