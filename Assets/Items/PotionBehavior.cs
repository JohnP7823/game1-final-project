using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{
    public Rigidbody2D myRig;
    public int potionType, potionValue;
    // Start is called before the first frame update
    void Start()
    {
        myRig = GetComponent<Rigidbody2D>();
        if (myRig == null)
        {
            throw new System.Exception(name + " does not have a rigidbody!");
        }
    }
   
    // Returns the type of potion (1 = Health, 2 = Mana)
    public int getPotionType()
    {
        return potionType;
    }

    // Returns the value of the potion (SH = 5, LH = 20, SM = 1, LM = 5)
    public int getPotionValue()
    {
        return potionValue;
    }

    //Deletes the object on collision with the player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") // Deletes the potion on collison
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Floor") // Stops the potion on collision with the floor
        {
            myRig.velocity = Vector2.zero;
        }
        else // Makes the potion ignore collisions for other objects
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
    }

    private void Awake()
    {
        myRig.velocity = new Vector2(0, -1.5f);
    }
}
