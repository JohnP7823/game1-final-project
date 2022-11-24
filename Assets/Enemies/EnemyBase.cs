using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int health = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TakeDamage(int damage)
    {
        // Play the hurt animation
        
        if(health <= 0)
        {
            // Delete the game object
        }
        else
        {
            health = health - damage;
        }
    }
}
