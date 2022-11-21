using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Rewrite this to work with my health system
        if (other.gameObject.tag == "Enemy")
        {
            //other.gameObject.
            //health.TakeDamage(damage); 
            
        }
    }
}
