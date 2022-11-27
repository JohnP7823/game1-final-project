using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D myRig;
    public Animator myAnim;
    public GameObject thePlayer, smallHealth, largeMana, smallMana;
    public int health = 4, damage = 2;
    protected bool stunned = false, attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        myRig = GetComponent<Rigidbody2D>();
        if (myRig == null)
        {
            throw new System.Exception(name + " does not have a rigidbody!");
        }
        myAnim = GetComponent<Animator>();
        if (myAnim == null)
        {
            throw new System.Exception(name + " does not have an animator controller!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void Attack()
    {
        Debug.Log("Does not have attack implemented!"); 
    }
    //Makes the enemy take damage to their health equal to the damage variable
    public virtual void TakeDamage(int damage)
    {
        Debug.Log(name + " has been hit");
        stunned = true;
        health = health - damage;
        myRig.velocity = Vector2.zero;
        if (health <= 0)
        {
            int itemRoll = Random.Range(1, 100);
            if(itemRoll >= 91)
            {
                Instantiate(largeMana, new Vector2(myRig.position.x, myRig.position.y + 1), Quaternion.identity);
            }
            else if (itemRoll >= 71)
            {
                Instantiate(smallMana, new Vector2(myRig.position.x, myRig.position.y + 1), Quaternion.identity);
            }
            else if (itemRoll <= 2)
            {
                Instantiate(smallHealth, new Vector2(myRig.position.x, myRig.position.y + 1), Quaternion.identity);
            }
            //add score
            Destroy(gameObject);
        }
        else
        {
            myAnim.SetTrigger("Hurt");
            StartCoroutine(StopPhase());
        }
    }

    // Makes the enemy stop moving on being hit
    IEnumerator StopPhase()
    {
        yield return new WaitForSeconds(.25f);
        Debug.Log("Enemy is moving again");
        stunned = false;
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable == false)
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
    }

    protected void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable == false)
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
    }

    // Returns the enemy's damage
    public int GetDamage()
    {
        return damage;
    }

    protected virtual void Awake()
    {
        thePlayer = GameObject.Find("Player");
    }
}
