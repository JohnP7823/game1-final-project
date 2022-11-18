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
    public bool grounded = true;

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
        }
    }

    public void Move(InputAction.CallbackContext c) // Moves on WASD press or held
    {
        if (c.phase == InputActionPhase.Started || c.phase == InputActionPhase.Performed)
        {
            Vector2 temp = c.ReadValue<Vector2>();
            LastInput = new Vector2(temp.x, 0);
            myRig.velocity = new Vector3(temp.x, 0).normalized * Speed + new Vector3(0, myRig.velocity.y);
            if (grounded == true && temp.x > 0)
            {
                myAnim.SetInteger("DIR", 2);
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (grounded == true && temp.x < 0)
            {
                myAnim.SetInteger("DIR", 2);
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (temp.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (temp.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (c.phase == InputActionPhase.Canceled) // No more velocity when the key is removed.
        {
            LastInput = Vector2.zero;
        }
    }
    // Jumps when the player hits spacebar, registers that the player is airborne and unable to jump again
    public void Jump(InputAction.CallbackContext c)
    {
        if (c.action.phase == InputActionPhase.Started && grounded)
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
        if (c.action.phase == InputActionPhase.Started)
        {
            myAnim.SetTrigger("Attack");
            // Setup actual way to attack and deal dmg
        }
    }

    public void Block(InputAction.CallbackContext c)
    {
        if(c.action.phase == InputActionPhase.Started)
        {
            myAnim.SetTrigger("Block");
            myAnim.SetBool("IdleBlock", true);
            // Setup how to perfect block and everything
        }

        if(c.action.phase == InputActionPhase.Canceled)
        {
            myAnim.SetBool("IdleBlock", false);
        }
    }

    public void TakeDamage(int damage)
    {
        // make hero take damage, check if dead, if dead, go to death func,  else, play hurt anim
    }

    // Checks to see what it collided with
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Platform" || other.gameObject.tag == "Floor")
        {
            grounded = true;
            myAnim.SetBool("Grounded", grounded);
        }
        if(other.gameObject.tag == "Enemy")
        {
            TakeDamage(1); // Take damage according to the enemies dmg value
        }
    }
}
