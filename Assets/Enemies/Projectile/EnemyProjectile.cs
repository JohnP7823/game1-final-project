using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 2;
    public Rigidbody2D myRig;
    public GameObject thePlayer;
    private Vector2 playerLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, playerLoc, .1f);
        myRig.MovePosition(newPosition);
        //Debug.Log(Mathf.Abs(myRig.position.x - playerLoc.x) + ", " + Mathf.Abs(myRig.position.y - playerLoc.y));
        if (Mathf.Abs(myRig.position.x - playerLoc.x) <= .1f && Mathf.Abs(myRig.position.y - playerLoc.y) <= .1f)
        { 
            Destroy(gameObject);
        }
    }

    protected virtual void Awake()
    {
        thePlayer = GameObject.Find("Player");
        playerLoc = thePlayer.GetComponent<Rigidbody2D>().position;
        playerLoc.y = playerLoc.y + .5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
