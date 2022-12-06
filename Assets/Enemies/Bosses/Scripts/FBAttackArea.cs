using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBAttackArea : MonoBehaviour
{
    public int damage = 6;
    public GameObject thePlayer;

    // Damages any enemy in the attack's AOE
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && thePlayer.GetComponent<PlayerController>().damagable == true)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void Awake()
    {
        thePlayer = GameObject.Find("Player");
    }
}
