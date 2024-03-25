using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to manage the collision of the bullet with the player and AI.

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AI" )
        {
            GameEventSystem.ResetCharacter(collision.gameObject);
            Destroy(this.gameObject);
        }
        
    }
}
