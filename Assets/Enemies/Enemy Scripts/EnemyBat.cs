using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : EnemyBase
{

    // Update is called once per frame
    void Update()
    {
        if (!attacking && Mathf.Abs(myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x) <= 5.5)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        attacking = true;
        if (flipped)
        {
            myRig.velocity = new Vector2(-.8f, -1f) * 9;
        }
        else
        {
            myRig.velocity = new Vector2(.8f, -1f) * 9;
        }
        StartCoroutine(AttackPhase());
    }

    IEnumerator AttackPhase()
    {
        yield return new WaitForSeconds(.55f);
        if (flipped)
        {
            myRig.velocity = new Vector2(-1, 0) * 10;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 10;
        }
    }

    protected override void Awake()
    {
        thePlayer = GameObject.Find("Player");
        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            flipped = true;
            myRig.velocity = new Vector2(-1, 0) * 10;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 10;
        }
    }
}
