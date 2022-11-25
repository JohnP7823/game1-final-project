using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D myRig;
    public GameObject thePlayer;
    public int health = 4, damage = 2;
    private bool isShooter, stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        myRig = GetComponent<Rigidbody2D>();
        if (myRig == null)
        {
            throw new System.Exception(name + " does not have a rigidbody!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!stunned) 
            myRig.velocity = new Vector2(-1, 0).normalized * 1; // Temp
    }

    protected virtual void Attack()
    {
        Debug.Log("Does not have attack implemented!"); 
    }
    //Makes the enemy take damage to their health equal to the damage variable
    public void TakeDamage(int damage)
    {
        Debug.Log(name + " has been hit");
        stunned = true;
        health = health - damage;
        myRig.velocity = Vector2.zero;
        if (health <= 0)
        {

            //spawn item?
            //add score
            Destroy(gameObject);
        }
        else
        {
            //Play hurt anim
            StartCoroutine(StopPhase());
        }
    }


    IEnumerator StopPhase()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Enemy is moving again");
        // Make their velocity normal again
        myRig.velocity = new Vector2(-1, 0).normalized * 2; // Temp
        stunned = false;
    }


    public int GetDamage()
    {
        return damage;
    }

    protected void Awake()
    {
        thePlayer = GameObject.Find("Player");
    }
}
