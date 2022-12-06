using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyBase
{
    public GameObject spellProj, spellProjF, fbAttackArea, deadFB;
    private bool isDead = false, teleDir = false, mAttack = false;
    private int dir, waveC = 0;


    // Update is called once per frame
    void Update()
    {
        if(mAttack)
        {
            if(GetComponent<SpriteRenderer>().flipX == false)
            {
                fbAttackArea.GetComponent<BoxCollider2D>().offset = new Vector2(-0.1869736f, -0.1923672f);
            }
            else
            {
                fbAttackArea.GetComponent<BoxCollider2D>().offset = new Vector2(0.1869736f, -0.1923672f);
            }
            if(Mathf.Abs(myRig.position.x - thePlayer.GetComponent<Rigidbody2D>().position.x) <= 1.5)
            {
                StartCoroutine(MeleeAttack());
            }
        }
    }

    IEnumerator Teleport()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(.5f);
        if (!teleDir) // Teleporting right
        {
            myRig.position = new Vector2(6, -2.18f);
            GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.35f, -.18f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else { // Teleporting left
            myRig.position = new Vector2(-6, -2.18f);
            GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.35f, -.18f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        int attackCounter = Random.Range(1, 2);
        if (attackCounter == 0){
            Attack();
        }
        else
        {
            SubAttack();
        }
            
    }

    protected override void Attack()
    {
        mAttack = true;
        myAnim.SetInteger("DIR", 1);
        if (GetComponent<SpriteRenderer>().flipX == false)
        {
            myRig.velocity = new Vector2(-1, 0) * 5;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 5;
        }
    }

    IEnumerator MeleeAttack()
    {
        myRig.velocity = Vector2.zero;
        myAnim.SetTrigger("Attacking");
        yield return new WaitForSeconds(.5f);
        fbAttackArea.SetActive(true);
        yield return new WaitForSeconds(.25f);
        myAnim.SetInteger("DIR", 0);
        fbAttackArea.SetActive(false);
        yield return new WaitForSeconds(1);
        NewTeleport();
    }

    private void SubAttack()
    {
        StartCoroutine(SubAttacking());
    }

    IEnumerator SubAttacking()
    {
        for (waveC = 0; waveC < 3; waveC++)
        {
            myAnim.SetTrigger("SubAttacking");
            int projHeight = Random.Range(0, 2);
            if (projHeight == 0)
            {
                //Debug.Log("Low proc");
                if (GetComponent<SpriteRenderer>().flipX == false)
                {

                    Instantiate(spellProj, new Vector2(5.75f, -3.5f), Quaternion.identity);
                }
                else
                {
                    Instantiate(spellProjF, new Vector2(-5.75f, -3.5f), Quaternion.identity);
                }
            }
            else
            {
                //Debug.Log("High proc");
                if (GetComponent<SpriteRenderer>().flipX == false)
                {

                    Instantiate(spellProj, new Vector2(5.75f, -2), Quaternion.identity);
                }
                else
                {
                    Instantiate(spellProjF, new Vector2(-5.75f, -2), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(.85f);
        }
        waveC = 0;
        yield return new WaitForSeconds(1f);
        NewTeleport();
    }

    private void NewTeleport()
    {
        Debug.Log("Finding new teleport");
        dir = Random.Range(0, 2);
        if (dir == 0)
        {
            teleDir = true;
        }
        else
        {
            teleDir = false;
        }
        mAttack = false;
        StartCoroutine(Teleport());
        
    }

    public override void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        myAnim.SetTrigger("Hurt");
        health = health - damage;
        BossHealthBar.Instance.UpdateHealthBar(health);
        if (health <= 0)
        {
            isDead = true;
            GameState.AddScore(scoreVal);
            Instantiate(deadFB, myRig.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected override void Awake()
    {
        attacking = true;
        fbAttackArea = transform.GetChild(0).gameObject;
        fbAttackArea.SetActive(false);
        dir = Random.Range(0, 2);
        if(dir == 0)
        {
            teleDir = true;
        }
        else
        {
            teleDir = false;
        }
        thePlayer = GameObject.Find("Player");
        StartCoroutine(Teleport());
    }
}
