using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator myAnim; // DIR 1 is the idle, DIR 2 is the running anim
    public Rigidbody2D myRig;
    public GameObject attackArea = default;
    public float Speed = 5, acceleration = 0.1F;
    public Vector2 LastInput;
    private float timeToAttack = .25f;
    private bool grounded = true, guarding = false, damagable = true, isDead = false, attacking = false, moveable = true;
    public int hp = 20, mhp = 20;
    public int mana = 3;
    private int tempF = 0;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        if (myAnim == null)
        {
            throw new System.Exception(name + " does not have an animator controller!");
        }
        myRig = GetComponent<Rigidbody2D>();
        if (myRig == null)
        {
            throw new System.Exception(name + " does not have a rigidbody!");
        }
        attackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        myAnim.SetFloat("AirSpeed", myRig.velocity.y);
        Vector2 newVel = LastInput * Speed + new Vector2(0, myRig.velocity.y);
        Vector2 oldVel = new Vector2(myRig.velocity.x, 0);
        myRig.velocity = Vector2.Lerp(oldVel, newVel, acceleration);
        if (myRig.velocity.y < 0) // If falling off a platform without jumping, play fall anim
        {
            grounded = false;
            myAnim.SetBool("Grounded", grounded);

        }
        else if (myRig.velocity == Vector2.zero) // Play the idle anim if the player is not moving
        {
            myAnim.SetInteger("DIR", 1);
            grounded = true;
            myAnim.SetBool("Grounded", grounded);
        }
        if (attacking)
        {
            tempF++;
        }
    }
    // Moves on WASD press or held
    public void Move(InputAction.CallbackContext c) 
    {
        if (!isDead && moveable)
        {
            if (c.phase == InputActionPhase.Started || c.phase == InputActionPhase.Performed)
            {
                Vector2 temp = c.ReadValue<Vector2>();
                LastInput = new Vector2(temp.x, 0);
                myRig.velocity = new Vector2(temp.x, 0).normalized * Speed + new Vector2(0, myRig.velocity.y);
                if (grounded)
                {
                    myAnim.SetInteger("DIR", 2);
                    if (temp.x > 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }

                }
                else
                {
                    if (temp.x > 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
            }
            if (c.phase == InputActionPhase.Canceled) // No more velocity when the key is removed.
            {
                LastInput = Vector2.zero;
            }
        }
        
    }
    // Jumps when the player hits spacebar, registers that the player is airborne and unable to jump again
    public void Jump(InputAction.CallbackContext c)
    {
        if (c.action.phase == InputActionPhase.Started && grounded && !guarding && !isDead)
        {
            grounded = false;
            myAnim.SetTrigger("Jump");
            myAnim.SetBool("Grounded", grounded);
            myRig.velocity = new Vector2(myRig.velocity.x, 7.5f);
        }
    }

    // Swings the sword when MB1 is pressed
    public void Attack(InputAction.CallbackContext c)
    {
        if (c.action.phase == InputActionPhase.Started && !guarding && !isDead && !attacking)
        {
            myAnim.SetTrigger("Attack");
            attacking = true;
            attackArea.SetActive(attacking);
            StartCoroutine(AttackingPhase());
        }
    }

    IEnumerator AttackingPhase()
    {
        yield return new WaitForSeconds(.25f);
        Debug.Log("Player can attack again");
        attacking = false;
        attackArea.SetActive(attacking);
    }

    // Guards against damage when Shift is pressed && held, perfect blocks for the first 0.5 secs
    public void Block(InputAction.CallbackContext c)
    {
        Debug.Log(tempF);
        if (!isDead)
        {
            if (c.action.phase == InputActionPhase.Started)
            {
                moveable = false;              // Make it where the player can't move;
                guarding = true;
                myAnim.SetTrigger("Block");
                myAnim.SetBool("IdleBlock", guarding);
                // Setup how to perfect block and everything
            }

            if (c.action.phase == InputActionPhase.Canceled)
            {
                moveable = true;             // Return the player's movement
                guarding = false;
                myAnim.SetBool("IdleBlock", guarding);
            }
        }
        
    }

    // Makes the player take damage according to the damage variable. Kills the player if they run out of health.
    public void TakeDamage(int damage)
    {
        // make hero take damage, is they are guarding, take half damage
        if (guarding)
        {
            hp = hp - (damage/2);
            Debug.Log("Player took 1 damage");
        }
        else
        {
            hp = hp - damage;
            Debug.Log("Player took 2 damage");
        }
        damagable = false;
        // check if dead: if dead, go to death func,  else, play hurt anim and become temporarily immune to damage
        if (hp <= 0)
        {
            Death();
        }
        else
        {
            if (!guarding)
            {
                myAnim.SetTrigger("Hurt");
            } 
            StartCoroutine(InvulnerablePhase());
        }
    }

    // Makes the player immune to damage for half a second
    IEnumerator InvulnerablePhase()
    {    
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Player can be hit again");
        damagable = true;
    }

    // Kills the player
    public void Death()
    {
        myAnim.SetTrigger("Death");
        isDead = true;
        // death code
    }

    // Checks to see what it collided with
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform" || other.gameObject.tag == "Floor")
        {
            grounded = true;
            myAnim.SetBool("Grounded", grounded);
        }
        else if(other.gameObject.tag == "Enemy" && damagable)
        {
            TakeDamage(2); // Take damage according to the enemies dmg value
        }
        else if(other.gameObject.tag == "Potion")
        {
            // Potion functionality
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && damagable)
        {
            TakeDamage(2); // Take damage according to the enemies dmg value
        }
    }
}
