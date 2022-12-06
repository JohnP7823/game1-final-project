using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    public Rigidbody2D myRig;
    public GameObject thePlayer;
    public int damage = 2;

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
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Trigger")
        {
            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        thePlayer = GameObject.Find("Player");
        if(thePlayer.GetComponent<SpriteRenderer>().flipX == true)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            myRig.velocity = new Vector2(-1, 0) * 10;
        }
        else
        {
            myRig.velocity = new Vector2(1, 0) * 10;
        }
    }
}
