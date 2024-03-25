using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to reset the player's flag when it collides with the AI.

public class FlagResetAI : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BlueFlag")
        {
            GameEventSystem.ResetFlag(collision.gameObject);
        }
    }
}
