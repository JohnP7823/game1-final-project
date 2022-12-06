using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossProjectile : MonoBehaviour
{
    public int damage = 4;
    public bool flipped;
    public Rigidbody2D myRig;
    public GameObject thePlayer;

    private void Awake()
    {
        if (flipped)
        {
            myRig.velocity = new Vector2(1, 0) * 5;
        }
        else
        {
            myRig.velocity = new Vector2(-1, 0) * 5;
        }
        thePlayer = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "EProjectile" && other.gameObject.name != "AttackArea")
        {
            if (other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
