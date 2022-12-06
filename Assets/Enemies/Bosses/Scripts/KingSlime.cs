using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : EnemyBase
{
    private int attackCounter = 0;
    private bool isDead = false;
    public GameObject kingSlimeDeath;

    // Update is called once per frame
    void Update()
    {
        if (!attacking && !isDead)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        attacking = true;
        if(attackCounter >= 2)
        {
            attackCounter = 0;
            if(myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x <= 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                myRig.velocity = new Vector2(1, 12f);
            }
            else
            {
                myRig.velocity = new Vector2(-1, 9f);
            }
        }
        else
        {
            attackCounter++;
            if (myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x <= 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                myRig.velocity = new Vector2(1, 6f);
            }
            else
            {
                myRig.velocity = new Vector2(-1, 6f);
            }
        }
        StartCoroutine(AttackingPhase());
    }

    IEnumerator AttackingPhase()
    {
        yield return new WaitForSeconds(.5f);
        myRig.velocity = new Vector2(myRig.velocity.x, 0);
        yield return new WaitForSeconds(1.5f);
        attacking = false;
    }

    public override void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        health = health - damage;
        BossHealthBar.Instance.UpdateHealthBar(health);
        if (health <= 0)
        {
            isDead = true;
            myRig.velocity = Vector2.zero;
            GameState.AddScore(scoreVal);
            Instantiate(kingSlimeDeath, myRig.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead && other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
        else if (!goingThrough && other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable == false)
        {
            StartCoroutine(EnableCol());
        }
    }
}
