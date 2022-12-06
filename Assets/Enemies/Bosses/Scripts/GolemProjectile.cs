using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProjectile : MonoBehaviour
{
    public int damage = 3, direction;
    public Rigidbody2D myRig;
    public GameObject thePlayer;

    private void Awake()
    {
        thePlayer = GameObject.Find("Player");
        switch (direction){
            case 1:
                myRig.velocity = new Vector2(1, 0) * 4;
                break;
            case 2:
                myRig.velocity = new Vector2(0, 1) * 4;
                break;
            case 3:
                myRig.velocity = new Vector2(-1, 0) * 4;
                break;
            case 4:
                myRig.velocity = new Vector2(0, -1) * 4;
                break;
            case 5:
                myRig.velocity = new Vector2(.5f, .5f).normalized * 4;
                break;
            case 6:
                myRig.velocity = new Vector2(-.5f, .5f).normalized * 4;
                break;
            case 7:
                myRig.velocity = new Vector2(-.5f, -.5f).normalized * 4;
                break;
            case 8:
                myRig.velocity = new Vector2(.5f, -.5f).normalized * 4;
                break;
        }
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
