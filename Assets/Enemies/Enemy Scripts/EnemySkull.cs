using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkull : EnemyBase
{
    public Vector2 playerLoc;
    public GameObject enemyProjectile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerLoc = thePlayer.GetComponent<Rigidbody2D>().position;
        playerLoc.y = playerLoc.y + 1f;
        Vector2 newPosition = Vector2.MoveTowards(myRig.position, playerLoc, Time.deltaTime * 3f);
        myRig.MovePosition(newPosition);
        if (!attacking && Mathf.Abs(myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x) <= 12)
        {
            attacking = true;
            InvokeRepeating("Attack", 1f, 3f);
        }
        if(myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    protected override void Attack()
    {
        myAnim.SetTrigger("Attacking");
        if (GetComponent<SpriteRenderer>().flipX == false)
        {
            Instantiate(enemyProjectile, new Vector2(myRig.position.x + 0.1f, myRig.position.y), Quaternion.identity);
            //Instantiate(fireballProjectile, new Vector2(myRig.position.x + 0.2f, myRig.position.y+1), Quaternion.identity);
        }
        else
        {
            Instantiate(enemyProjectile, new Vector2(myRig.position.x - 0.1f, myRig.position.y), Quaternion.identity);
        }
        //Debug.Log("Firing e project");
        //StartCoroutine(AttackingPhase());
    }

    /*IEnumerator AttackingPhase()
    {
        yield return new WaitForSeconds(3f);
        attacking = false;
    }*/
}
