using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator myAnim; // DIR 1 is the idle, DIR 2 is the running anim
    public Rigidbody2D myRig;
    public GameObject attackArea, fireballProjectile, hitSound, jumpSound, guardSound, itemSound; // attackArea is the AOE of the normal attack, fireball is the subweapon
    public float Speed = 5, acceleration = 0.1F;
    public Vector2 LastInput, TempLastInput;
    private float timeToAttack = .25f; //Cooldown between attacks
    private bool grounded = true, guarding = false, pGuarding = false, isDead = false, attacking = false, sAttacking = false, moveable = true;
    public bool damagable = true;
    public int hp = 20, mhp = 20;
    public int mana = 3;
    // Start is called before the first frame update
    void Start()
    {
        myRig = GetComponent<Rigidbody2D>();
        if (myRig == null)
        {
            throw new System.Exception(name + " does not have rigid body 2D!");
        }
        myAnim = GetComponent<Animator>();
        if (myAnim == null)
        {
            throw new System.Exception(name + " does not have an animator controller!");
        }
        attackArea = transform.GetChild(0).gameObject;
        hitSound = transform.GetChild(1).gameObject;
        jumpSound = transform.GetChild(2).gameObject;
        guardSound = transform.GetChild(3).gameObject;
        itemSound = transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.Instance.UpdateHealthBar(hp);
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
        if (attacking && GetComponent<SpriteRenderer>().flipX == true) //Flips the attack hitbox if the player is facing left
        {
            attackArea.GetComponent<BoxCollider2D>().offset = new Vector2(-0.9726f, 0.783f);
        }
        else if (attacking)
        {
            attackArea.GetComponent<BoxCollider2D>().offset = new Vector2(0.9726f, 0.783f);
        }
    }
    
    // Moves on WASD press or held
    public void Move(InputAction.CallbackContext c) 
    {
        if (isDead || !moveable)
        {
            return;
        }
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

    // Jumps when the player hits spacebar, registers that the player is airborne and unable to jump again
    public void Jump(InputAction.CallbackContext c)
    {
        if (c.action.phase == InputActionPhase.Started && grounded && !guarding && !isDead)
        {
            grounded = false;
            if (!attacking)
            {
                myAnim.SetTrigger("Jump");
            }
            jumpSound.GetComponent<AudioSource>().Play();
            myAnim.SetBool("Grounded", grounded);
            myRig.velocity = new Vector2(myRig.velocity.x, 7.5f);
        }
    }

    // Swings the sword when MB1 is pressed, makes the damage hitbox active
    public void Attack(InputAction.CallbackContext c)
    {
        if (c.action.phase == InputActionPhase.Started && !guarding && !isDead && !attacking)
        {
            myAnim.SetTrigger("Attack");
            attacking = true;
            attackArea.SetActive(attacking);
            attackArea.GetComponent<AttackArea>().source.Play();
            StartCoroutine(AttackingPhase());
        }
    }
    
    // Makes the damage hitbox inactive after 0.25 seconds.
    IEnumerator AttackingPhase()
    {
        yield return new WaitForSeconds(timeToAttack);
        //Debug.Log("Player can attack again");
        attacking = false;
        attackArea.SetActive(attacking);
    }

    // Creates a fireball when MB2 is pressed if the player has mana
    public void SubAttack(InputAction.CallbackContext c)
    {
        if (c.action.phase == InputActionPhase.Started && !guarding && !isDead && !sAttacking && mana > 0)
        {
            sAttacking = true;
            mana--;
            GameState.UpdateMana(mana);
            if (GetComponent<SpriteRenderer>().flipX == false)
            {
                Instantiate(fireballProjectile, new Vector2(myRig.position.x + 0.2f, myRig.position.y+1), Quaternion.identity);
            }
            else
            {
                Instantiate(fireballProjectile, new Vector2(myRig.position.x - 0.2f, myRig.position.y+1), Quaternion.identity);
            }
            StartCoroutine(SubAttackingPhase());
        }
    }

    // Cooldown for the sub weapon
    IEnumerator SubAttackingPhase()
    {
        yield return new WaitForSeconds(.5f);
        //Debug.Log("Player can fireball again");
        sAttacking = false;
    }

    // Guards against damage when Shift is pressed && held, perfect blocks for the first 0.2 secs
    public void Block(InputAction.CallbackContext c)
    {
        if (!isDead)
        {
            if (c.action.phase == InputActionPhase.Started)
            {
                TempLastInput = LastInput;
                LastInput = Vector2.zero;
                moveable = false;              // Make it where the player can't move;
                guarding = true;
                pGuarding = true;
                myAnim.SetTrigger("Block");
                myAnim.SetBool("IdleBlock", guarding);
                StartCoroutine(PerfectGuardPhase());
            }

            if (c.action.phase == InputActionPhase.Canceled)
            {
                LastInput = TempLastInput;
                moveable = true;             // Return the player's movement
                guarding = false;
                myAnim.SetBool("IdleBlock", guarding);
            }
        }   
    }

    // Makes the player immune to damage for the first 0.2 seconds of guarding
    IEnumerator PerfectGuardPhase()
    {
        yield return new WaitForSeconds(.2f);
        Debug.Log("Player is no longer perfect guarding");
        pGuarding = false;
    }

    // Makes the player take damage according to the damage variable. Kills the player if they run out of health.
    public void TakeDamage(int damage)
    {
        // make hero take damage, is they are guarding, take half damage
        if (pGuarding)
        {
            guardSound.GetComponent<AudioSource>().Play();
        }
        else if (guarding)
        {
            guardSound.GetComponent<AudioSource>().Play();
            hp = hp - (damage/2);
        }
        else 
        {
            hitSound.GetComponent<AudioSource>().Play();
            hp = hp - damage;
        }
        damagable = false;
        HealthBar.Instance.UpdateHealthBar(hp);
        if (hp <= 0) // check if dead: if dead, go to death func,  else, play hurt anim and become temporarily immune to damage
        {
            Death();
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
            if (!guarding && !attacking)
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
        //Debug.Log("Player can be hit again");
        damagable = true;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    // Kills the player
    public void Death()
    {
        myRig.velocity = Vector2.zero;
        LastInput = Vector2.zero;
        myAnim.SetTrigger("Death");
        isDead = true;
        if (GameState.Lives > 0)
        {
            StartCoroutine(CheckpointRestart());
        }
        else
        {
            StartCoroutine(LevelRestart());
        }
    }

    IEnumerator CheckpointRestart()
    {
        //play death jingle
        yield return new WaitForSeconds(5f);
        GameState.RestartAtCheckpoint();
    }

    IEnumerator LevelRestart()
    {
        //play death jingle
        yield return new WaitForSeconds(8f);
        GameState.RestartLevel();
    }

    public void ResetState()
    {
        hp = 20;
        mana = 3;
        grounded = true;
    }

    public void SetStats(int nHP, int nMana)
    {
        //Debug.Log("Set stats ran");
        hp = nHP;
        mana = nMana;
        HealthBar.Instance.UpdateHealthBar(hp);
        //Debug.Log("Health is: " + hp + ", mana is: " + mana);
    }
   

    // Checks to see what it collided with
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform" || other.gameObject.tag == "Floor")
        {
            grounded = true;
            myAnim.SetBool("Grounded", grounded);
        }
        else if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EProjectile") && damagable)
        {
            TakeDamage(other.gameObject.GetComponent<EnemyBase>().GetDamage()); // Take damage according to the enemies dmg value
        }
        else if(other.gameObject.tag == "Potion")
        {
            int pType = other.gameObject.GetComponent<PotionBehavior>().getPotionType(), pValue = other.gameObject.GetComponent<PotionBehavior>().getPotionValue();
            if(pType == 1)
            {
                hp = hp + pValue;
                if (hp > mhp)
                {
                    hp = mhp;
                }
                HealthBar.Instance.UpdateHealthBar(hp);
            }
            else if(pType == 2)
            {
                mana = mana + pValue;
                GameState.UpdateMana(mana);
            }
            itemSound.GetComponent<AudioSource>().Play();
        }
    }

    //See above
    private void OnCollisionStay2D(Collision2D other)
    {
        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "enemyProjectile") && damagable)
        {
            TakeDamage(other.gameObject.GetComponent<EnemyBase>().GetDamage()); // Take damage according to the enemies dmg value
        }
    }

    private void Awake()
    {
        attackArea.SetActive(false);
    }
}
