using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrab : EnemyBase
{

    IEnumerator JumpingPhase()
    {
        yield return new WaitForSeconds(.8f);
        Debug.Log("Crab is falling");
        myRig.velocity = Vector2.zero;
        //myRig.velocity = new Vector2(0, -1) * 8;
    }

    protected override void Awake()
    {
        thePlayer = GameObject.Find("Player");
        myRig.velocity = new Vector2(0, 1) * 12;
        StartCoroutine(JumpingPhase());
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag != "Player")
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
