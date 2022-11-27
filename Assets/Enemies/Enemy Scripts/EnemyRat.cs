using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRat : EnemyBase
{
    private bool flipped = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mathf.Abs(myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x));
        if (!attacking && Mathf.Abs(myRig.position.x-thePlayer.GetComponent<Rigidbody2D>().position.x)<=5)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        attacking = true;
        myAnim.SetTrigger("Attacking");
        //Debug.Log("Rat attack");
        if (flipped)
        {
            myRig.velocity = new Vector2(-1, 0.714f) * 7;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0.714f) * 7;
        }
        //myRig.velocity = new Vector2(myRig.velocity.x, 5);
        //Debug.Log("Current velocity is: " + myRig.velocity);
        StartCoroutine(AttackPhase()); 
    }

    IEnumerator AttackPhase()
    {
        yield return new WaitForSeconds(.5f);
        if (flipped)
        {
            myRig.velocity = new Vector2(-1, 0) * 7;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 7;
        }
        //Debug.Log("Switch velocity is: " + myRig.velocity);
    }

    protected override void Awake()
    {
        thePlayer = GameObject.Find("Player");
        if(GetComponent<SpriteRenderer>().flipX == true)
        {
            flipped = true;
            myRig.velocity = new Vector2(-1, 0) * 7;
            //Debug.Log("S Velocity: " + myRig.velocity);
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 7;
            //Debug.Log("S Velocity: " + myRig.velocity);
        }
    }
}
