using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    // Set for the individual prefab
    public GameObject enemyType;
    public float xLoc, yLoc;
    public bool flipped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject createdEnemy = Instantiate(enemyType, new Vector2(xLoc, yLoc), Quaternion.identity);
            if (flipped)
            {
                createdEnemy.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}
