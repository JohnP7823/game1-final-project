using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator myAnim; // DIR 1 is the idle, DIR 2 is the running anim
    public Rigidbody2D myRig;
    public float Speed = 5;
    public Vector2 LastInput;
    public float acceleration = 1.1F;
    public bool grounded = true, guarding = false, damagable = true, isDead = false;
    public int hp = 20, mhp = 20;
    public int mana = 3;

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
    }
    // Moves on WASD press or held
    public void Move(InputAction.CallbackContext c) 
    {
        if (!isDead)
        {
            if (c.phase == InputActionPhase.Started || c.phase == InputActionPhase.Performed)
            {
                Vector2 temp = c.ReadValue<Vector2>();
                LastInput = new Vector2(temp.x, 0);
                myRig.velocity = new Vector3(temp.x, 0).normalized * Speed + new Vector3(0, myRig.velocity.y);
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
        if (c.action.phase == InputActionPhase.Started && !guarding && !isDead)
        {
            myAnim.SetTrigger("Attack");
            // Setup actual way to attack and deal dmg
        }
    }
    // Guards against damage when Shift is pressed && held, perfect blocks for the first 0.5 secs
    public void Block(InputAction.CallbackContext c)
    {
        if (!isDead)
        {
            if (c.action.phase == InputActionPhase.Started)
            {
                Speed = 0;              // Make it where the player can't move;
                guarding = true;
                myAnim.SetTrigger("Block");
                myAnim.SetBool("IdleBlock", guarding);
                // Setup how to perfect block and everything
            }

            if (c.action.phase == InputActionPhase.Canceled)
            {
                Speed = 5;              // Return the player's movement
                guarding = false;
                myAnim.SetBool("IdleBlock", guarding);
            }
        }
        
    }
    // Makes the player take damage according to the damage variable. Kills the player if they run out of health.
    public void TakeDamage(int damage)
    {
        // make hero take damage, check if dead, if dead, go to death func,  else, play hurt anim
        hp = hp - damage;
        if(hp <= 0)
        {
            Death();
        }
        else
        {
            myAnim.SetTrigger("Hurt");
        }
    }
    // Kills the player
    public void Death()
    {
        myAnim.SetTrigger("Death");
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
        else if(other.gameObject.tag == "Enemy")
        {
            TakeDamage(5); // Take damage according to the enemies dmg value
        }
    }
}
