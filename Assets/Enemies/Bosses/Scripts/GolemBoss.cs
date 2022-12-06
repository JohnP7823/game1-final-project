using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss : EnemyBase
{
    public GameObject gProjectile1, gProjectile2, gProjectile3, gProjectile4, gProjectile5, gProjectile6, gProjectile7, gProjectile8, golemDeath;
    private int attackCounter = 0;
    private bool isDead = false;

    protected override void Attack()
    {
        myAnim.SetTrigger("Attacking");
        // If counter is %2 = 0, 90 degree attack, else 45 degree attack
        if (attackCounter % 2 == 0)
        {
            Instantiate(gProjectile1, new Vector2(1.1f, -0.535f), gProjectile1.GetComponent<Transform>().rotation);
            Instantiate(gProjectile2, new Vector2(0.61f, 0.25f), gProjectile2.GetComponent<Transform>().rotation);
            Instantiate(gProjectile3, new Vector2(-0.24f, -0.175f), gProjectile3.GetComponent<Transform>().rotation);
            Instantiate(gProjectile4, new Vector2(0.25f, -0.96f), gProjectile4.GetComponent<Transform>().rotation);
        }
        else
        {
            Instantiate(gProjectile5, new Vector2(1.1f, -0.335f), gProjectile5.GetComponent<Transform>().rotation);
            Instantiate(gProjectile6, new Vector2(0.005f, -0.07f), gProjectile6.GetComponent<Transform>().rotation);
            Instantiate(gProjectile7, new Vector2(-0.25f, -1.14f), gProjectile7.GetComponent<Transform>().rotation);
            Instantiate(gProjectile8, new Vector2(0.85f, -1.38f), gProjectile8.GetComponent<Transform>().rotation);
        }
        attackCounter++;
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
            Instantiate(golemDeath, myRig.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected override void Awake()
    {
        InvokeRepeating("Attack", 1f, 1.5f);
    }
}
