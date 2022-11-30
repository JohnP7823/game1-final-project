using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRat : EnemyBase
{

    // Update is called once per frame
    void Update()
    {
        if (!attacking && Mathf.Abs(myRig.position.x-thePlayer.GetComponent<Rigidbody2D>().position.x)<=4)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        attacking = true;
        myAnim.SetTrigger("Attacking");
        if (flipped)
        {
            myRig.velocity = new Vector2(-.8f, 0.75f) * 7;
        }
        else
        {
            myRig.velocity = new Vector2(.8f, 0.75f) * 7;
        }
        StartCoroutine(AttackPhase()); 
    }

    IEnumerator AttackPhase()
    {
        yield return new WaitForSeconds(1);
        if (flipped)
        {
            myRig.velocity = new Vector2(-1, 0) * 7;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 7;
        }
    }

    protected override void Awake()
    {
        thePlayer = GameObject.Find("Player");
        if(GetComponent<SpriteRenderer>().flipX == true)
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
