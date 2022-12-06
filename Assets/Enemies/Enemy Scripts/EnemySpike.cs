using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpike : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("Spikes don't take damage");
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Spikes have no special interaction on col");
    }

    protected override void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Spikes have no special interaction on col");
    }
}
