using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D myRig;
    public Animator myAnim;
    public GameObject thePlayer, smallHealth, largeMana, smallMana;
    public int health = 4, damage = 2, scoreVal = 100;
    protected bool stunned = false, attacking = false, flipped = false, goingThrough = false;
    protected Vector2 oldVel;

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
        stunned = true;
        health = health - damage;
        oldVel = myRig.velocity;
        myRig.velocity = Vector2.zero;
        if (health <= 0)
        {
            int itemRoll = Random.Range(1, 101);   // Coroutine this to allow a death animation
            if(itemRoll >= 91)
            {
                Instantiate(largeMana, new Vector2(myRig.position.x, myRig.position.y), Quaternion.identity);
            }
            else if (itemRoll >= 71)
            {
                Instantiate(smallMana, new Vector2(myRig.position.x, myRig.position.y), Quaternion.identity);
            }
            else if (itemRoll <= 2)
            {
                Instantiate(smallHealth, new Vector2(myRig.position.x, myRig.position.y), Quaternion.identity);
            }
            GameState.AddScore(scoreVal);
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
        yield return new WaitForSeconds(.4f);
        stunned = false;
        myRig.velocity = oldVel;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (!goingThrough && other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable == false)
        {
            StartCoroutine(EnableCol());
        }
        else if (other.gameObject.tag == "Potion" || other.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
    }

    protected IEnumerator EnableCol()
    {
        goingThrough = true;
        Physics2D.IgnoreCollision(thePlayer.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>());
        yield return new WaitUntil(() => thePlayer.GetComponent<PlayerController>().damagable == true);
        Physics2D.IgnoreCollision(thePlayer.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>(), false);
        goingThrough = false;
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (!goingThrough && other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable == false)
        {
            StartCoroutine(EnableCol());
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
