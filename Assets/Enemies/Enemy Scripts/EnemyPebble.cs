using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPebble : EnemyBase 
{
    public Vector2 playerLoc;

    // Update is called once per frame
    void Update()
    {
        playerLoc = thePlayer.GetComponent<Rigidbody2D>().position;
        playerLoc.y = playerLoc.y + 1f;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, playerLoc, .05f);
        myRig.MovePosition(newPosition);
    }

    protected override void Attack()
    {
        Debug.Log("Pebble does not actively attack");
    }
}
