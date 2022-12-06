using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public int damage = 2;


    public AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Damages any enemy in the attack's AOE
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
        }
    }
}
